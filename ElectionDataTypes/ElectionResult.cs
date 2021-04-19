using System.Linq;

namespace ElectionDataTypes
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ElectionDataTypes.Interfaces;
    using ElectionDataTypes.Providers;

    public class ElectionResult
    {
        #region Private Data

        private IPartiesProvider _parties;
        private IConstituencyResultProvider _firstVotes;
        private IConstituencyResultProvider _secondVotes;

        #endregion region

        #region Public Properties

        public int Year { get; set; }
        public IPartiesProvider Parties
        {
            get => _parties;
            set
            {
                _parties = value;
                CalculateResults();
            }
        }
        public IConstituencyResultProvider FirstVotes
        {
            get => _firstVotes;
            set
            {
                _firstVotes = value;
                CalculateResults();
            }
        }
        public IConstituencyResultProvider SecondVotes
        {
            get => _secondVotes;
            set
            {
                _secondVotes = value;
                CalculateResults();
            }
        }

        public List<RegionResult> RegionResults { get; }

        #endregion region

        #region Public Methods

        public void CalculateResults()
        {
            if (Parties == null || FirstVotes == null || SecondVotes == null)
            {
                return;
            }

            List<string> regionNames =
                FirstVotes.ResultSetsByRegion.Keys.OrderBy(x => x).ToList();

            RegionResults.Clear();
            foreach (string regionName in regionNames)
            {
                if (SecondVotes.ResultSetsByRegion.ContainsKey(regionName))
                {
                    RegionResults.Add(
                        new RegionResult(
                            regionName,
                            FirstVotes.ResultSetsByRegion[regionName],
                            SecondVotes.ResultSetsByRegion[regionName]
                            ));
                }
            }

            Console.WriteLine("doing it");

        }

        #endregion region

        public ElectionResult()
        {
            Year = 2016;

            Parties = null;
            FirstVotes = null;
            SecondVotes = null;

            RegionResults = new List<RegionResult>();
        }
    }
}
