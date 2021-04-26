namespace ScotPolWpfApp.ViewModels
{
    using ElectionDataTypes.Polling;
    using ElectionDataTypes.Results;
    using ElectionDataTypes.Settings;

    public class MainViewModel : Caliburn.Micro.PropertyChangedBase
    {
        #region Constants

        private const string WindowTitleDefault = "The non-web scot pol app!";

        #endregion

        #region Private Data

        private string _windowTitle = WindowTitleDefault;

        private readonly ElectionResult _electionResult =
            new ElectionResult();

        private readonly ElectionPredictionSet _electionPredictions =
            new ElectionPredictionSet();

        private ResultsImporterViewModel _resultsImporterViewModel = 
            new ResultsImporterViewModel();

        #endregion

        #region Properties

        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                _windowTitle = value;
                NotifyOfPropertyChange(() => WindowTitle);
            }
        }

        public ResultsImporterViewModel ResultsImporter
        {
            get => _resultsImporterViewModel;
            set
            {
                _resultsImporterViewModel = value;
                NotifyOfPropertyChange(() => ResultsImporter);
            }
        }

        #endregion

        public MainViewModel(DatabaseSettings dbSettings)
        {
            ResultsImporter = 
                new ResultsImporterViewModel
                {
                    ElectionResults = _electionResult,
                    ElectionPredictions = _electionPredictions
                };
        }
    }
}
