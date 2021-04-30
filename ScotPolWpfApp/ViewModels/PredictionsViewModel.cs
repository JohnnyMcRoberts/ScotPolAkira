namespace ScotPolWpfApp.ViewModels
{
    using System;
    using System.Windows.Input;
    
    using ElectionDataTypes.Polling;
    using ElectionDataTypes.Results;

    using Utilities;


    /// <summary>
    /// This class will display the polling predictions.
    /// </summary>
    public class PredictionsViewModel : Caliburn.Micro.PropertyChangedBase
    {
        #region Private Data

        private ElectionPredictionSet _electionPredictions;

        private DateTime _lastUpdated;

        private readonly OxyPlotViewModel[] _plotViewModels;

        /// <summary>
        /// The load notes command.
        /// </summary>
        private ICommand _refreshChartsCommand;

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

        #region Commands

        /// <summary>
        /// Load the notes command.
        /// </summary>
        public ICommand RefreshChartsCommand =>
            _refreshChartsCommand ??
            (_refreshChartsCommand = new CommandHandler(RefreshChartsCommandAction, true));

        #endregion

        #region Command handlers

        /// <summary>
        /// The load notes file command action.
        /// </summary>
        private void RefreshChartsCommandAction()
        {
            Console.WriteLine("RefreshChartsCommandAction");

            lock (PlotListVotesWithTime.Model.SyncRoot)
            {
                PlotListVotesWithTime.Update(ElectionResults, ElectionPredictions);
            }

            PlotListVotesWithTime.Model.InvalidatePlot(true);

            NotifyOfPropertyChange(() => PlotListVotesWithTime);

        }

        #endregion

        #region Plots

        public OxyPlotViewModel PlotListVotesWithTime { get; private set; }

        public OxyPlotViewModel PlotConstituencyVotesWithTime { get; private set; }

        #endregion

        #region Local Methods

        private void PredictionsPollsUpdated(object sender, EventArgs e)
        {
            LastUpdated = DateTime.Now;
            //PlotListVotesWithTime.Update(ElectionResults, ElectionPredictions);
            //NotifyOfPropertyChange(() => PlotListVotesWithTime);

            //PlotConstituencyVotesWithTime.Update(ElectionResults, ElectionPredictions);
            //NotifyOfPropertyChange(() => PlotConstituencyVotesWithTime);

            foreach (var plot in _plotViewModels)
            {
                plot.Update(ElectionResults, ElectionPredictions);
            }
        }

        #endregion

        public PredictionsViewModel()
        {
            LastUpdated = DateTime.Now;
            PlotListVotesWithTime =
                new OxyPlotViewModel(PlotType.ListVotesWithTime);
            PlotConstituencyVotesWithTime =
                new OxyPlotViewModel(PlotType.ConstituencyVotesWithTime);

            _plotViewModels = 
                new[]
                {
                    PlotListVotesWithTime, 
                    PlotConstituencyVotesWithTime
                };
        }
    }
}
