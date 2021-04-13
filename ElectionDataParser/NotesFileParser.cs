

using ElectionDataTypes;

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
    using ElectionDataTypes.Providers;

    using ElectionDataTypes.Settings;

    public class NotesFileParser
    {
        #region Constants

        private const string DataStartTag = @"Party abbreviations";

        #endregion

        #region Private Data

        /// <summary>
        /// On exit the provider.
        /// </summary>
        private PartiesProvider _partiesProvider;

        /// <summary>
        /// The full file path of file read.
        /// </summary>
        private string _filePath;

        /// <summary>
        /// The full file path of file read.
        /// </summary>
        private string _errorsFound;

        /// <summary>
        /// The full file path of file read.
        /// </summary>
        private bool _readSuccessfully;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the parties provider or null if it did not load.
        /// </summary>
        public IPartiesProvider PartiesProvider => _partiesProvider;

        /// <summary>
        /// Gets the full file path.
        /// </summary>
        public string FilePath => _filePath;

        /// <summary>
        /// Gets the full file path.
        /// </summary>
        public string ErrorsFound => _errorsFound;

        /// <summary>
        /// Gets the full file path.
        /// </summary>
        public bool ReadSuccessfully => _readSuccessfully;

        #endregion

        #region Public Methods

        /// <summary>
        /// Reads the data for this import from the file specified.
        /// </summary>
        /// <param name="filename">The file to read from.</param>
        /// <param name="errorMessage">The error message if unsuccessful.</param>
        /// <returns>True if read successfully, false otherwise.</returns>
        public bool ReadFromFile(string filename, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Check the file exists.
            if (!File.Exists(filename))
            {
                errorMessage = $"File {filename} does not exist";
                return false;
            }

            // Try to deserialize the file into the party notes.
            List<PartyNote> partyNotes = new List<PartyNote>();

            try
            {
                using (StreamReader sr = new StreamReader(filename, Encoding.Default))
                {
                    CsvReader csv = new CsvReader(sr, CultureInfo.CurrentCulture);

                    bool foundStartDataTag = false;
                    while (csv.Read())
                    {
                        // Get the first field.
                        string fullName = csv.GetField<string>(0);

                        // Ignore empty entries.
                        if (string.IsNullOrWhiteSpace(fullName))
                            continue;

                        // If haven't found the header line see if this is it
                        if (!foundStartDataTag)
                        {
                            if (fullName.Contains(DataStartTag))
                            {
                                foundStartDataTag = true;
                            }

                            continue;
                        }

                        // if here on a line with data after the start tag has been found so get the abbreviation
                        string abbreviation = csv.GetField<string>(1);


                        if (!string.IsNullOrWhiteSpace(abbreviation))
                        {
                            partyNotes.Add(new PartyNote { FullName = fullName, Abbreviation = abbreviation });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errorMessage = e.ToString();
                return false;
            }

            if (partyNotes.Any())
            {
                _partiesProvider = new PartiesProvider(partyNotes);
            }

            return true;
        }

        #endregion


        /// <summary>
        /// Loads the party notes from a file.
        /// </summary>
        /// <param name="resultsDir">The full path of the the results files.</param>
        /// <param name="fileName">The full path of the notes notes file, or null if te default is to be read.</param>
        public NotesFileParser(string resultsDir, string fileName = null)
        {
            _errorsFound = string.Empty;
            _readSuccessfully = false;
            _partiesProvider = new PartiesProvider();

            _filePath = Path.Combine(resultsDir, fileName ?? DefaultFileNames.NotesFileName);

            _readSuccessfully = ReadFromFile(_filePath, out _errorsFound);
        }
    }
}
