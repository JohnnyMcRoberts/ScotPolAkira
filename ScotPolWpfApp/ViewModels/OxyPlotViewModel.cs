namespace ScotPolWpfApp.ViewModels
{
    using System;
    
    using OxyPlot;

    using ElectionDataTypes.Polling;
    using ElectionDataTypes.Results;

    using PlotGenerators;
    using Utilities;

    public class OxyPlotViewModel : BaseViewModel
    {
        private readonly OxyPlotPairViewModel _plotPairViewModel;

        public IPlotController ViewController => _plotPairViewModel.ViewController;

        public PlotModel Model => _plotPairViewModel.Model;

        public void Update(
            ElectionResult electionResults, 
            ElectionPredictionSet electionPredictions)
        {
            _plotPairViewModel.UpdateData(electionResults, electionPredictions);
            Model.InvalidatePlot(true);

            OnPropertyChanged(() => ViewController);
            OnPropertyChanged(() => Model);

        }

        public OxyPlotViewModel(Utilities.PlotType plotType)
        {
            // Get the plot generator type etc and create the plot pair.
            string title = plotType.GetTitle();
            bool? canHover = plotType.GetCanHover();
            Type plotGeneratorType = plotType.GetGeneratorClass();
            BasePlotGenerator plotGenerator =
                (BasePlotGenerator)Activator.CreateInstance(plotGeneratorType);
            
            _plotPairViewModel = 
                new OxyPlotPairViewModel(
                    plotGenerator, title, canHover.HasValue && canHover.Value);
        }
    }
}
