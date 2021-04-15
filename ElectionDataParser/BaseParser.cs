
namespace ElectionDataParser
{
    using System.IO;

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
