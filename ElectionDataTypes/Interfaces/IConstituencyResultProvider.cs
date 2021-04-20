namespace ElectionDataTypes.Interfaces
{
    using System.Collections.Generic;
    
    using Results;
    
    public interface IConstituencyResultProvider
    {
        /// <summary>
        /// Gets the constituency results by constituency code.
        /// </summary>
        Dictionary<string, ConstituencyResult> ResultsByCode { get; }

        /// <summary>
        /// Getsthe constituency results by constituency name.
        /// </summary>
        Dictionary<string, ConstituencyResult> ResultsByName { get; }

        /// <summary>
        /// Gets the constituency results by regions.
        /// </summary>
        Dictionary<string, List<ConstituencyResult>> ResultSetsByRegion { get; }

        /// <summary>
        /// Gets the names of the constituencies.
        /// </summary>
        List<string> ConstituencyNames { get; }
        
        /// <summary>
        /// Gets the codes of the constituencies.
        /// </summary>
        List<string> ConstituencyCodes { get; }

        /// <summary>
        /// Gets the names of the regions.
        /// </summary>
        List<string> RegionNames { get; }
    }
}
