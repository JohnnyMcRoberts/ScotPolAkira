namespace ScotPolWpfApp.ViewModels
{
    using ElectionDataTypes.Polling;
    using ElectionDataTypes.Results;

    using OxyPlot;

    using Interfaces;

    public class OxyPlotPairViewModel : BaseViewModel
    {
        #region Nested Classes

        public class CustomPlotController : PlotController
        {
            public CustomPlotController()
            {
                this.BindKeyDown(OxyKey.Left, PlotCommands.PanRight);
                this.BindKeyDown(OxyKey.Right, PlotCommands.PanLeft);
            }
        }

        #endregion

        #region Private Data

        private IPlotGenerator _plotGenerator;

        private IPlotController _viewController;

        private PlotModel _model;

        #endregion

        #region Public Properties

        public IPlotGenerator PlotGenerator
        {
            get
            {
                return _plotGenerator;
            }
            set
            {
                _plotGenerator = value;
            }
        }

        public IPlotController ViewController
        {
            get
            {
                return _viewController;
            }
            set
            {
                _viewController = value;
                OnPropertyChanged(() => ViewController);
            }
        }

        public PlotModel Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
                OnPropertyChanged(() => Model);
            }
        }

        /// <summary>
        /// Gets the geography data for the plots.
        /// </summary>
        public ElectionResult ElectionResults { get; private set; }

        /// <summary>
        /// Gets the books read data for the plots.
        /// </summary>
        public ElectionPredictionSet ElectionPredictions { get; private set; }

        #endregion


        #region Public Methods

        public void UpdateData(
            ElectionResult electionResults, ElectionPredictionSet electionPredictions)
        {
            ElectionResults = electionResults;
            ElectionPredictions = electionPredictions;
            Model = _plotGenerator.SetupPlot(electionResults, electionPredictions);
        }

        #endregion

        #region Constructor

        public OxyPlotPairViewModel(IPlotGenerator plotGenerator, string title, bool hoverOver = false)
        {
            _plotGenerator = plotGenerator;

            // Create the plot model & controller 
            PlotModel plotModel = new PlotModel { Title = title, Subtitle = "using OxyPlot only" };

            // Set the Model property, the INotifyPropertyChanged event will 
            //  make the WPF Plot control update its content
            Model = plotModel;
            CustomPlotController controller = new CustomPlotController();
            if (hoverOver)
            {
                controller.UnbindMouseDown(OxyMouseButton.Left);
                controller.BindMouseEnter(PlotCommands.HoverSnapTrack);
            }
            ViewController = controller;
        }

        #endregion

    }
}
