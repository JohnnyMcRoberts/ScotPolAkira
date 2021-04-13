using System.Linq;

namespace ElectionDataTypes.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ElectionDataTypes.Interfaces;

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

        public PartiesProvider()
        {
            PartiesByAbbreviation = new Dictionary<string, string>();
        }

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
