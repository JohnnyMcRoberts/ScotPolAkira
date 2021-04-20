namespace ElectionDataTypes.Results
{
    public class PartyResult
    {
        public string PartyAbbreviation { get; set; }
        public int Votes { get; set; }
        public float PercentageOfVotes { get; set; }

        public PartyResult()
        {
            PartyAbbreviation = string.Empty;
            Votes = 0;
            PercentageOfVotes = 0f;
        }

        public PartyResult(PartyResult src)
        {
            PartyAbbreviation = src.PartyAbbreviation;
            Votes = src.Votes;
            PercentageOfVotes = src.PercentageOfVotes;
        }
    }
}
