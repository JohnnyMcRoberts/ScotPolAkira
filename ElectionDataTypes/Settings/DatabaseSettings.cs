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
        public string ExportDirectory { get; set; }

        /// <summary>
        /// Gets and sets the results directory.
        /// </summary>
        public string ResultsDirectory { get; set; }

        /// <summary>
        /// Gets and sets the predictions directories.
        /// </summary>
        public string PredictionsDirectory { get; set; }

        /// <summary>
        /// Gets and sets the database connection string.
        /// </summary>
        public string DatabaseConnectionString { get; set; }

        public DatabaseSettings()
        {
            ExportDirectory = string.Empty;
            ResultsDirectory = string.Empty;
            PredictionsDirectory = string.Empty;
            DatabaseConnectionString = string.Empty;
        }
    }
}
