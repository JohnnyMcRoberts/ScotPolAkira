namespace ElectionDataTypes.Settings
{
    /// <summary>
    /// THe settings for the database and the input and output directories.
    /// </summary>
    public class DatabaseSettings
    {
        /// <summary>
        /// Gets and sets the export directory.
        /// </summary>
        public string ExportDirectory { get; private set; }

        /// <summary>
        /// Gets and sets the results directory.
        /// </summary>
        public string ResultsDirectory { get; private set; }

        /// <summary>
        /// Gets and sets the predictions directories.
        /// </summary>
        public string PredictionsDirectory { get; private set; }

        /// <summary>
        /// Gets and sets the database connection string.
        /// </summary>
        public string DatabaseConnectionString { get; private set; }

    }
}
