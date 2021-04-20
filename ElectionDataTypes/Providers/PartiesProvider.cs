namespace ElectionDataTypes.Providers
{
    using System.Collections.Generic;
    using System.Linq;

    using Interfaces;
    using Results;

    public class PartiesProvider: IPartiesProvider
    {
        #region Properties

        public Dictionary<string, string> PartiesByAbbreviation { get; }

        public List<string> PartyNames
        {
            get { return PartiesByAbbreviation.Select(p => p.Value).ToList(); }
        }

        public List<string> PartyAbbreviations
        {
            get { return PartiesByAbbreviation.Select(p => p.Key).ToList(); }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PartiesProvider"/> class.
        /// </summary>
        public PartiesProvider()
        {
            PartiesByAbbreviation = new Dictionary<string, string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PartiesProvider"/> class.
        /// </summary>
        /// <param name="partyNotes">The party notes it is constructed from.</param>
        public PartiesProvider(List<PartyNote> partyNotes) : this()
        {
            foreach (PartyNote party in partyNotes)
            {
                if (!PartiesByAbbreviation.ContainsKey(party.Abbreviation))
                {
                    PartiesByAbbreviation.Add(party.Abbreviation, party.FullName);
                }
            }
        }
    }
}
