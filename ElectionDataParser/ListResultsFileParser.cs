namespace ElectionDataParser
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using CsvHelper;

    using ElectionDataTypes.Results;
    using ElectionDataTypes.Interfaces;
    using ElectionDataTypes.Providers;
    using ElectionDataTypes.Settings;

    public class ListResultsFileParser : BaseParser
    {
        #region Abstract Data and Methods Overloads

        /// <summary>
        /// Default filename to override
        /// </summary>
        public override string DefaultFileName => DefaultFileNames.RegionalListsFileName;

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
                        constituencyResults.Add(
                            GetConstituencyResultFromRecord(csv, onsCode, PartyNames));
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
        public static readonly string DataStartTag = @"ONS Code";

        /// <summary>
        /// The tag that after which party data is.
        /// </summary>
        public static readonly string[] PartyNames =
        {
            @"CON",
            @"LAB",
            @"LIB",
            @"SNP",
            @"SGRN", 
            @"UKIP",
            @"IND", 
            @"SOL", 
            @"RISE",
            @"CHP",
            @"WEP", 
            @"SLBR",
            @"ABBUP", 
            @"COMP",
            @"NF",
            @"ANWP",
            @"Others"
        };

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

        /// <summary>
        /// Loads the constituency results from a file.
        /// </summary>
        /// <param name="resultsDir">The full path of the the results files.</param>
        /// <param name="fileName">The full path of the notes notes file, or null if the default is to be read.</param>
        public ListResultsFileParser(string resultsDir, string fileName = null)
        {
            Parse(resultsDir, fileName);
        }
    }
}
