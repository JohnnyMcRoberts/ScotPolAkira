namespace ElectionDataParser
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using CsvHelper;

    using ElectionDataTypes;

    public abstract class BaseParser
    {
        #region Properties

        /// <summary>
        /// Gets the full file path.
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Gets the full file path.
        /// </summary>
        public string ErrorsFound { get; private set; }

        /// <summary>
        /// Gets if the file was read correctly.
        /// </summary>
        public bool ReadSuccessfully { get; private set; }

        #endregion

        #region Abstract Data and Methods

        /// <summary>
        /// Default filename to override
        /// </summary>
        public abstract string DefaultFileName { get; }

        /// <summary>
        /// Reads the data for this import from the file specified.
        /// </summary>
        /// <param name="filename">The file to read from.</param>
        /// <param name="errorMessage">The error message if unsuccessful.</param>
        /// <returns>True if read successfully, false otherwise.</returns>
        public abstract bool ReadFromFile(string filename, out string errorMessage);

        #endregion

        #region Public Static Utility Methods

        /// <summary>
        /// Parses an integer from a csv field.
        /// </summary>
        /// <param name="value">The value string.</param>
        /// <param name="style">The number style.</param>
        /// <param name="provider">The Format provider.</param>
        /// <returns>The value of the field if parsed, zero otherwise.</returns>
        /// <returns></returns>
        public static int ConvertInt(
            string value,
            NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands,
            IFormatProvider provider = null)
        {
            if (provider == null)
            {
                provider = new CultureInfo("en-GB");
            }

            if (string.IsNullOrWhiteSpace(value) || value.StartsWith(" -"))
            {
                return 0;
            }

            try
            {
                int number = Int32.Parse(value, style, provider);
                return number;
            }
            catch (FormatException)
            {
                Console.WriteLine("Unable to convert '{0}'.", value);
            }
            catch (OverflowException)
            {
                Console.WriteLine("'{0}' is out of range of the Int32 type.", value);
            }

            return 0;
        }

        /// <summary>
        /// Parses a float from a csv field.
        /// </summary>
        /// <param name="value">The value string.</param>
        /// <param name="style">The number style.</param>
        /// <param name="provider">The Format provider.</param>
        /// <returns>The value of the field if parsed, zero otherwise.</returns>
        public static float ConvertFloat(
            string value,
            NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint,
            IFormatProvider provider = null)
        {
            if (provider == null)
            {
                provider = new CultureInfo("en-GB");
            }

            if (string.IsNullOrWhiteSpace(value) || value.StartsWith(" -"))
            {
                return 0;
            }

            try
            {
                float number = Single.Parse(value, style, provider);
                return number;
            }
            catch (FormatException)
            {
                Console.WriteLine("Unable to convert '{0}'.", value);
            }
            catch (OverflowException)
            {
                Console.WriteLine("'{0}' is out of range of the Single type.", value);
            }

            return 0;
        }

        /// <summary>
        /// Gets a constituency result from a row.
        /// </summary>
        /// <param name="csv">The csv row.</param>
        /// <param name="onsCode">The ONS code of the constituency.</param>
        /// <param name="partyNames">The party names.</param>
        /// <returns>The parsed constituency result.</returns>
        public static ConstituencyResult GetConstituencyResultFromRecord(
            CsvReader csv,
            string onsCode,
            string[] partyNames)
        {
            // If here on a line with data convert it into a record - the format is:
            // (0) ONS Code	
            // (1) Constituency
            // (2) Region
            // (3) Electorate
            // (4) Total ballots at the count
            // (5) Total valid votes cast
            // (6) Rejected ballots
            // (7) Ballot Box Turnout(%)
            // (8) Rejected ballots(%)	
            // (9-n) CON LAB LIB SNP etc (votes)
            // (9+n+1-2n) CON LAB LIB SNP etc (%)

            ConstituencyResult constituencyResult =
                new ConstituencyResult
                {
                    Code = onsCode,
                    Constituency = csv.GetField<string>(1),
                    Region = csv.GetField<string>(2),
                    Electorate = ConvertInt(csv.GetField<string>(3)),
                    TotalBallotsAtTheCount = ConvertInt(csv.GetField<string>(4)),
                    TotalValidVotesCast = ConvertInt(csv.GetField<string>(5)),
                    RejectedBallots = ConvertInt(csv.GetField<string>(6)),
                    BallotBoxTurnoutPercentage = ConvertFloat(csv.GetField<string>(7)),
                    RejectedBallotsPercentage = ConvertFloat(csv.GetField<string>(8))
                };

            constituencyResult.PartyResults = 
                GetPartyConstituencyResults(csv, partyNames, constituencyResult.TotalValidVotesCast);

            List<PartyConstituencyResult> sortedResults =
                constituencyResult.PartyResults.OrderByDescending(x => x.Votes).ToList();

            constituencyResult.Win = sortedResults[0].PartyAbbreviation;
            constituencyResult.Second = sortedResults[1].PartyAbbreviation;
            constituencyResult.Majority = sortedResults[0].Votes - sortedResults[1].Votes;
            constituencyResult.MajorityPercentage =
                (constituencyResult.Majority * 100.0f) / constituencyResult.TotalValidVotesCast;

            return constituencyResult;
        }

        /// <summary>
        /// Gets part results for a constituency result from a row.
        /// </summary>
        /// <param name="csv">The csv row.</param>
        /// <param name="partyNames">The party names.</param>
        /// <param name="totalVotesCast">The total votes cast in the constituency.</param>
        /// <returns>The parsed constituency result.</returns>
        public static List<PartyConstituencyResult> GetPartyConstituencyResults(
            CsvReader csv, 
            string[] partyNames,
            int totalVotesCast)
        {
            int partiesOffset = 9;
            float percentageMultiplier = 100f / totalVotesCast;
            List<PartyConstituencyResult> partyResults = new List<PartyConstituencyResult>();
            for (int i = 0; i < partyNames.Length; i++)
            {
                PartyConstituencyResult partyResult =
                    new PartyConstituencyResult
                    {
                        PartyAbbreviation = partyNames[i],
                        Votes =
                            ConvertInt(csv.GetField<string>(partiesOffset + i)),
                        PercentageOfVotes =
                            ConvertFloat(csv.GetField<string>(partiesOffset + i)) * percentageMultiplier
                    };

                partyResults.Add(partyResult);
            }

            return partyResults;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Reads the data for this import from the file specified.
        /// </summary>
        /// <param name="resultsDir">The full path of the the results files.</param>
        /// <param name="fileName">The full path of the  file, or null if the default is to be read.</param>
        public void Parse(string resultsDir, string fileName = null)
        {
            ErrorsFound = string.Empty;
            ReadSuccessfully = false;

            FilePath = Path.Combine(resultsDir, fileName ?? DefaultFileName);

            ReadSuccessfully = ReadFromFile(FilePath, out string errorsFound);
            ErrorsFound = errorsFound;
        }

        #endregion
    }
}
