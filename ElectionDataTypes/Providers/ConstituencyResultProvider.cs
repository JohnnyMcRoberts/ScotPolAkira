namespace ElectionDataTypes.Providers
{
    using System.Collections.Generic;
    using System.Linq;


    using Interfaces;
    using Results;

    public class ConstituencyResultProvider : IConstituencyResultProvider
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the constituency results by constituency code.
        /// </summary>
        public Dictionary<string, ConstituencyResult> ResultsByCode { get; set; }

        /// <summary>
        /// Gets or sets the constituency results by constituency name.
        /// </summary>
        public Dictionary<string, ConstituencyResult> ResultsByName { get; set; }

        /// <summary>
        /// Gets or sets the constituency results by regions.
        /// </summary>
        public Dictionary<string, List<ConstituencyResult>> ResultSetsByRegion { get; set; }

        /// <summary>
        /// Gets the names of the constituencies.
        /// </summary>
        public List<string> ConstituencyNames
        {
            get { return ResultsByName.Select(p => p.Key).ToList(); }
        }

        /// <summary>
        /// Gets the codes of the constituencies.
        /// </summary>
        public List<string> ConstituencyCodes
        {
            get { return ResultsByCode.Select(p => p.Key).ToList(); }
        }

        /// <summary>
        /// Gets the names of the regions.
        /// </summary>
        public List<string> RegionNames
        {
            get { return ResultSetsByRegion.Select(p => p.Key).ToList(); }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstituencyResultProvider"/> class.
        /// </summary>
        public ConstituencyResultProvider()
        {
            ResultsByCode = new Dictionary<string, ConstituencyResult>();
            ResultsByName = new Dictionary<string, ConstituencyResult>();
            ResultSetsByRegion = new Dictionary<string, List<ConstituencyResult>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PartiesProvider"/> class.
        /// </summary>
        /// <param name="constituencyResults">The party notes it is constructed from.</param>
        public ConstituencyResultProvider(List<ConstituencyResult> constituencyResults) : this()
        {
            ResultsByCode = new Dictionary<string, ConstituencyResult>();
            ResultsByName = new Dictionary<string, ConstituencyResult>();
            ResultSetsByRegion = new Dictionary<string, List<ConstituencyResult>>();

            foreach (ConstituencyResult constituency in constituencyResults)
            {
                if (!ResultsByName.ContainsKey(constituency.Constituency))
                {
                    ResultsByName.Add(constituency.Constituency, constituency);
                }

                if (!ResultsByCode.ContainsKey(constituency.Code))
                {
                    ResultsByCode.Add(constituency.Code, constituency);
                }

                if (!ResultSetsByRegion.ContainsKey(constituency.Region))
                {
                    ResultSetsByRegion.Add(constituency.Region, new List<ConstituencyResult>{ constituency });
                }
                else
                {
                    ResultSetsByRegion[constituency.Region].Add(constituency);
                }
            }
        }
    }
}
