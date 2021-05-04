namespace ElectionDataTypes.Polling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Results;

    public class PartyPollingResultsSet
    {
        #region Nested Classes

        public struct PollingResult
        {
            public DateTime Date;
            public int DaysSinceStart;
            public float Percentage;
        }

        #endregion

        #region Private Data

        private DateTime? _startDate;

        private readonly alglib.spline1dinterpolant _splineParameters;

        #endregion

        #region Properties

        public string PartyAbbreviation { get; set; }
        
        public bool IsConstituency { get; set; }

        public List<PollingResult> PollingResults { get; set; }

        #endregion

        #region Public Methods

        public float GetInterploatedValue(DateTime date)
        {
            // Check valid data has been set up
            if (!_startDate.HasValue || _splineParameters == null)
            {
                return 0f;
            }

            // Check that interpolating a value after the start of the data set
            int daysSinceStart = (date - _startDate.Value).Days;
            if (daysSinceStart < 0)
            {
                return 0f;
            }

            // calculated the interpolation value
            double interpolatedValue = alglib.spline1dcalc(_splineParameters, daysSinceStart);

            // Don't allow negative percentages of the vote
            if (interpolatedValue < 0d)
            {
                return 0f;
            }

            // return the interpolated value as a float
            return Convert.ToSingle(interpolatedValue);
        }

        #endregion

        public PartyPollingResultsSet(
            string party,
            bool isConstituency,
            List<OpinionPoll> polls
        )
        {
            PartyAbbreviation = party;
            IsConstituency = isConstituency;
            PollingResults = new List<PollingResult>();
            _startDate = null;
            _splineParameters = null;

            // loop through the polls getting the polling values
            SetupPollingResults(party, isConstituency, polls);

            // get the x and y values 
            double[] xValues = PollingResults.Select(x => (double) x.DaysSinceStart).ToArray();
            double[] yValues = PollingResults.Select(x => (double)x.Percentage).ToArray();

            // fit the curves
            if (xValues.Length > 0 && yValues.Length > 0)
            {
                alglib.spline1dfitpenalized(
                    xValues,
                    yValues,
                    yValues.Length, 
                    1d,
                    out _,
                    out alglib.spline1dinterpolant splineParameters,
                    out alglib.spline1dfitreport _);

                _splineParameters = splineParameters;
            }
        }

        private void SetupPollingResults(string party, bool isConstituency, List<OpinionPoll> polls)
        {
            foreach (OpinionPoll poll in polls)
            {
                if (!_startDate.HasValue)
                {
                    _startDate = poll.PublicationDate;
                }

                List<PartyResult> pollingPercentages =
                    isConstituency ? poll.ConstituencyPredictions : poll.ListPredictions;

                foreach (PartyResult pollingPercentage in pollingPercentages)
                {
                    if (pollingPercentage.PartyAbbreviation == party)
                    {
                        PollingResults.Add(
                            new PollingResult
                            {
                                Date = poll.PublicationDate,
                                DaysSinceStart = (poll.PublicationDate - _startDate.Value).Days,
                                Percentage = pollingPercentage.PercentageOfVotes
                            });

                        break;
                    }
                }
            }
        }
    }
}
