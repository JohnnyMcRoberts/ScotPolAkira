namespace ElectionDataTypes.Interfaces
{
    using System.Collections.Generic;

    public interface IPartiesProvider
    {
        Dictionary<string,string> PartiesByAbbreviation { get; }

        List<string> PartyNames { get; }

        List<string> PartyAbbreviations { get; }
    }
}
