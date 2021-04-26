namespace ElectionDataParser
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using CsvHelper;

    using ElectionDataTypes.Interfaces;
    using ElectionDataTypes.Polling;
    using ElectionDataTypes.Providers;
    using ElectionDataTypes.Results;
    using ElectionDataTypes.Settings;

    public class PollingFileParser : BaseParser
    {
        #region Constants

        /// <summary>
        /// The tag that after which polling data begins.
        /// </summary>
        private const string DataStartTag = @"Polling Company @";

        /// <summary>
        /// The constituency party name abbreviations.
        /// </summary>
        public static readonly string[] ConstituencyPartyNames =
            { @"SNP", @"CON", @"LAB", @"LIB", @"SGRN" };

        public static readonly int ConstituencyPartyNamesOffset = 3;

        /// <summary>
        /// The list party name abbreviations.
        /// </summary>
        public static readonly string[] ListPartyNames =
            { @"SNP", @"CON", @"LAB", @"SGRN", @"LIB", @"ALBA" };

        public static readonly int ListPartyNamesOffset = 8;

        #endregion

        #region Private Data

        /// <summary>
        /// On exit the provider.
        /// </summary>
        private PollsProvider _pollsProvider;

        #endregion

        #region Properties

        public IPollsProvider PollsProvider => _pollsProvider;

        #endregion

        #region Local Methods

        private OpinionPoll GetOpinionPollFromRecord(CsvReader csv, string pollingCompany)
        {
            // if here on a line with data after the start tag has been found so get the abbreviation
            string link = csv.GetField<string>(2);

            if (
                csv.TryGetField<DateTime>(1, out DateTime publicationDate) &&
                !string.IsNullOrWhiteSpace(link))
            {
                // Get the constituency party support predictions.
                List<PartyResult> constituencyPartyPredictions = new List<PartyResult>();
                for (int i = 0; i < ConstituencyPartyNames.Length; i++)
                {
                    PartyResult partyResult =
                        new PartyResult
                        {
                            PartyAbbreviation = ConstituencyPartyNames[i],
                            Votes =0,
                            PercentageOfVotes =
                                ConvertFloat(csv.GetField<string>(ConstituencyPartyNamesOffset + i))
                        };

                    constituencyPartyPredictions.Add(partyResult);
                }


                // Get the list party support predictions.
                List<PartyResult> listPartyPredictions = new List<PartyResult>();
                for (int i = 0; i < ListPartyNames.Length; i++)
                {
                    PartyResult partyResult =
                        new PartyResult
                        {
                            PartyAbbreviation = ListPartyNames[i],
                            Votes = 0,
                            PercentageOfVotes =
                                ConvertFloat(csv.GetField<string>(ListPartyNamesOffset + i))
                        };

                    listPartyPredictions.Add(partyResult);
                }

                // Make up the poll.
                return new OpinionPoll
                {
                    PollingCompany =  pollingCompany, 
                    Link =  link, 
                    PublicationDate = publicationDate,
                    ConstituencyPredictions = constituencyPartyPredictions,
                    ListPredictions = listPartyPredictions
                };
            }

            return null;
        }

        #endregion

        #region Abstract Data and Methods Overloads

        /// <summary>
        /// Default filename to override
        /// </summary>
        public override string DefaultFileName => DefaultFileNames.PollingFileName;

        /// <summary>
        /// Reads the data for this import from the file specified.
        /// </summary>
        /// <param name="filename">The file to read from.</param>
        /// <param name="errorMessage">The error message if unsuccessful.</param>
        /// <returns>True if read successfully, false otherwise.</returns>
        public override bool ReadFromFile(string filename, out string errorMessage)
        {
            _pollsProvider = new PollsProvider();
            errorMessage = string.Empty;

            // Try to deserialize the file into the constituency results.
            List<OpinionPoll> opinionPolls = new List<OpinionPoll>();

            try
            {
                using (StreamReader sr = new StreamReader(filename, Encoding.Default))
                {
                    CsvReader csv = new CsvReader(sr, CultureInfo.CurrentCulture);

                    bool foundStartDataTag = false;
                    while (csv.Read())
                    {
                        // Get the first field.
                        string pollingCompany = csv.GetField<string>(0);

                        // Ignore empty entries.
                        if (string.IsNullOrWhiteSpace(pollingCompany))
                            continue;

                        // If haven't found the header line see if this is it
                        if (!foundStartDataTag)
                        {
                            if (pollingCompany.Contains(DataStartTag))
                            {
                                foundStartDataTag = true;
                            }

                            continue;
                        }

                        // If here on a line with data try to convert it into a record.
                        OpinionPoll poll = GetOpinionPollFromRecord(csv, pollingCompany);
                        if (poll != null)
                        {
                            opinionPolls.Add(poll);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errorMessage = e.ToString();
                return false;
            }

            if (opinionPolls.Any())
            {
                _pollsProvider = new PollsProvider(opinionPolls);
            }

            return true;
        }

        #endregion

        /// <summary>
        /// Loads the polls from a file.
        /// </summary>
        /// <param name="resultsDir">The full path of the the results files.</param>
        /// <param name="fileName">The full path of the notes notes file, or null if the default is to be read.</param>
        public PollingFileParser(string resultsDir, string fileName = null)
        {
            _pollsProvider = new PollsProvider();
            Parse(resultsDir, fileName);
        }
    }
}
