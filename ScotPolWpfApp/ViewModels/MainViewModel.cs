using ElectionDataTypes;
using ElectionDataTypes.Settings;

namespace ScotPolWpfApp.ViewModels
{
    public class MainViewModel : Caliburn.Micro.PropertyChangedBase
    {
        #region Constants

        private const string WindowTitleDefault = "The non-web scot pol app!";

        #endregion

        #region Private Data

        private string _windowTitle = WindowTitleDefault;

        private readonly ElectionResult _electionResult = 
            new ElectionResult();

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
                    ElectionResults = _electionResult
                };
        }
    }
}
