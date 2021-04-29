namespace ScotPolWpfApp.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using ElectionDataParser;

    using ElectionDataTypes.Polling;
    using ElectionDataTypes.Results;

    using Models;

    /// <summary>
    /// This class will display the polling predictions.
    /// </summary>
    public class PredictionsViewModel : Caliburn.Micro.PropertyChangedBase
    {
        #region Properties

        private ElectionPredictionSet _electionPredictions;

        private DateTime _lastUpdated;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the election results for a year.
        /// </summary>
        public ElectionResult ElectionResults { get; set; }

        /// <summary>
        /// Gets or sets the election predictions for a set of polls.
        /// </summary>
        public ElectionPredictionSet ElectionPredictions
        {
            get
            {
                return _electionPredictions;
            }

            set
            {
                _electionPredictions = value;
                _electionPredictions.PollsUpdated += PredictionsPollsUpdated;
            }
        }

        public DateTime LastUpdated
        {
            get => _lastUpdated;
            private set { _lastUpdated = value; NotifyOfPropertyChange(() => LastUpdated); }
        }

        #endregion

        #region Local Methods

        private void PredictionsPollsUpdated(object sender, EventArgs e)
        {
            LastUpdated = DateTime.Now;
        }

        #endregion

        public PredictionsViewModel()
        {
            LastUpdated = DateTime.Now;
        }
    }
}
