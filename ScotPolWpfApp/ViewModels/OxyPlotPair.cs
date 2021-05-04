﻿namespace ScotPolWpfApp.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;

    using OxyPlot;

    using ElectionDataTypes.Polling;
    using ElectionDataTypes.Results;

    using Interfaces;

    public class OxyPlotPair : INotifyPropertyChanged
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
        public IPlotController _viewController;
        public PlotModel _model;

        #endregion

        #region Public Properties

        public IPlotGenerator PlotGenerator
        {
            get => _plotGenerator;
            set { _plotGenerator = value; }
        }

        public IPlotController ViewController
        {
            get => _viewController;
            set { _viewController = value; OnPropertyChanged(() => ViewController); }
        }

        public PlotModel Model
        {
            get => _model;
            set { _model = value; OnPropertyChanged(() => Model); }
        }

        #endregion

        #region INotifyPropertyChanged Members

        void OnPropertyChanged<T>(Expression<Func<T>> sExpression)
        {
            if (sExpression == null) throw new ArgumentNullException("sExpression");

            MemberExpression body = sExpression.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("Body must be a member expression");
            }
            OnPropertyChanged(body.Member.Name);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged Members

        #region Public Methods

        public void UpdateData(
            ElectionResult electionResults, ElectionPredictionSet electionPredictions)
        {
            Model = _plotGenerator.SetupPlot(electionResults, electionPredictions);
        }

        #endregion

        #region Constructor

        public OxyPlotPair(IPlotGenerator plotGenerator, string title, bool hoverOver = false)
        {
            _plotGenerator = plotGenerator;

            // Create the plot model & controller 
            var tmp = new PlotModel { Title = title, Subtitle = "using OxyPlot only" };

            // Set the Model property, the INotifyPropertyChanged event will 
            //  make the WPF Plot control update its content
            Model = tmp;
            var controller = new CustomPlotController();
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
