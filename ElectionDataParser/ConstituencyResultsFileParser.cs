using Newtonsoft.Json;

namespace ElectionDataParser
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using CsvHelper;

    using ElectionDataTypes;
    using ElectionDataTypes.Interfaces;
    using ElectionDataTypes.Providers;
    using ElectionDataTypes.Settings;

    public class ConstituencyResultsFileParser : BaseParser
    {
        #region Abstract Data and Methods Overloads

        /// <summary>
        /// Default filename to override
        /// </summary>
        public override string DefaultFileName => DefaultFileNames.ConstituenciesFileName;

        /// <summary>
        /// Reads the data for this import from the file specified.
        /// </summary>
        /// <param name="filename">The file to read from.</param>
        /// <param name="errorMessage">The error message if unsuccessful.</param>
        /// <returns>True if read successfully, false otherwise.</returns>
        public override bool ReadFromFile(string filename, out string errorMessage)
        {
            _constituencyResultProvider = new ConstituencyResultProvider();
            errorMessage = string.Empty;

            // Check the file exists.
            if (!File.Exists(filename))
            {
                errorMessage = $"File {filename} does not exist";
                return false;
            }

            // Try to deserialize the file into the constituency results.
            List<ConstituencyResult> constituencyResults = new List<ConstituencyResult>();

            try
            {
                using (StreamReader sr = new StreamReader(filename, Encoding.Default))
                {
                    CsvReader csv = new CsvReader(sr, CultureInfo.CurrentCulture);

                    bool foundStartDataTag = false;
                    while (csv.Read())
                    {
                        // Get the first field.
                        string onsCode = csv.GetField<string>(0);

                        // Ignore empty entries.
                        if (string.IsNullOrWhiteSpace(onsCode))
                            continue;

                        // If haven't found the header line see if this is it
                        if (!foundStartDataTag)
                        {
                            if (onsCode.Contains(DataStartTag))
                            {
                                foundStartDataTag = true;
                            }

                            continue;
                        }

                        // If here on a line with data convert it into a record.
                        constituencyResults.Add(GetConstituencyResultFromRecord(csv, onsCode));
                    }
                }
            }
            catch (Exception e)
            {
                errorMessage = e.ToString();
                return false;
            }

            if (constituencyResults.Any())
            {
                _constituencyResultProvider =
                    new ConstituencyResultProvider(constituencyResults);
            }

            return true;
        }

        #endregion

        #region Constants

        /// <summary>
        /// The tag that after which party data is.
        /// </summary>
        private readonly string DataStartTag = @"ONS Code";

        /// <summary>
        /// The tag that after which party data is.
        /// </summary>
        private static readonly string[] PartyNames =
            { @"CON", @"LAB", @"LIB", @"SNP", @"IND", @"TUSC", @"SGRN", @"Others" };

        #endregion

        #region Private Data

        /// <summary>
        /// On exit the provider.
        /// </summary>
        private ConstituencyResultProvider _constituencyResultProvider;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the constituency results  provider or null if it did not load.
        /// </summary>
        public IConstituencyResultProvider ConstituencyResultProvider => _constituencyResultProvider;

        #endregion

        #region Public Methods

        private static int ConvertInt(
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

        private static float ConvertFloat(
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

        public static ConstituencyResult GetConstituencyResultFromRecord(CsvReader csv, string onsCode)
        {

            // if here on a line with data convert it into a record the - format is:
            // (0) ONS Code	
            // (1) Constituency
            // (2) Region
            // (3) Electorate
            // (4) Total ballots at the count
            // (5) Total valid votes cast
            // (6) Rejected ballots
            // (7) Ballot Box Turnout(%)
            // (8) Rejected ballots(%)	
            // (9-16) CON LAB LIB SNP IND TUSC    SGRN Others (votes)
            // (17-24) CON LAB LIB SNP IND TUSC    SGRN Others (%)
            // (25) Win
            // (26) Second

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
                    RejectedBallotsPercentage = ConvertFloat(csv.GetField<string>(8)),
                    PartyResults = new List<PartyConstituencyResult>
                    {
                        new PartyConstituencyResult
                        {
                            PartyAbbreviation = PartyNames[0],
                            Votes = ConvertInt(csv.GetField<string>(9)),
                            PercentageOfVotes = ConvertFloat(csv.GetField<string>(17)),

                        },
                        new PartyConstituencyResult
                        {
                            PartyAbbreviation = PartyNames[1],
                            Votes = ConvertInt(csv.GetField<string>(10)),
                            PercentageOfVotes = ConvertFloat(csv.GetField<string>(18)),

                        },
                        new PartyConstituencyResult
                        {
                            PartyAbbreviation = PartyNames[2],
                            Votes = ConvertInt(csv.GetField<string>(11)),
                            PercentageOfVotes = ConvertFloat(csv.GetField<string>(19)),

                        },
                        new PartyConstituencyResult
                        {
                            PartyAbbreviation = PartyNames[3],
                            Votes = ConvertInt(csv.GetField<string>((12))),
                            PercentageOfVotes = ConvertFloat(csv.GetField<string>(20)),

                        },
                        new PartyConstituencyResult
                        {
                            PartyAbbreviation =PartyNames[4],
                            Votes = ConvertInt(csv.GetField<string>(13)),
                            PercentageOfVotes = ConvertFloat(csv.GetField<string>(21)),

                        },
                        new PartyConstituencyResult
                        {
                            PartyAbbreviation = PartyNames[5],
                            Votes = ConvertInt(csv.GetField<string>(14)),
                            PercentageOfVotes = ConvertFloat(csv.GetField<string>(22)),

                        },
                        new PartyConstituencyResult
                        {
                            PartyAbbreviation = PartyNames[6],
                            Votes = ConvertInt(csv.GetField<string>(15)),
                            PercentageOfVotes = ConvertFloat(csv.GetField<string>(23)),

                        },
                        new PartyConstituencyResult
                        {
                            PartyAbbreviation = PartyNames[7],
                            Votes = ConvertInt(csv.GetField<string>(16)),
                            PercentageOfVotes = ConvertFloat(csv.GetField<string>(24))
                        }
                    }
                };

            List<PartyConstituencyResult> partResults = 
                constituencyResult.PartyResults.OrderByDescending(x => x.Votes).ToList();

            constituencyResult.Win = partResults[0].PartyAbbreviation;
            constituencyResult.Second = partResults[1].PartyAbbreviation;
            constituencyResult.Majority = partResults[0].Votes - partResults[1].Votes;
            constituencyResult.MajorityPercentage =
                (constituencyResult.Majority * 100.0f) / constituencyResult.TotalValidVotesCast;

            return constituencyResult;
        }
        
        #endregion

        /// <summary>
        /// Loads the constituency results from a file.
        /// </summary>
        /// <param name="resultsDir">The full path of the the results files.</param>
        /// <param name="fileName">The full path of the notes notes file, or null if the default is to be read.</param>
        public ConstituencyResultsFileParser(string resultsDir, string fileName = null)
        {
            Parse(resultsDir, fileName);
        }

    }
}
