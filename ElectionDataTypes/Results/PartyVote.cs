namespace ElectionDataTypes.Results
{
    public class PartyVote
    {
        public string Abbreviation { get; set; }
        public string FullName { get; set; }

        public int TotalConstituencyVote { get; set; }
        public int TotalListVote { get; set; }

        public float PercentageConstituencyVote { get; set; }
        public float PercentageListVote { get; set; }

        public int ConstituencySeats { get; set; }
        public int ListSeats { get; set; }

        public int TotalSeats => (ConstituencySeats + ListSeats);

        public PartyVote()
        {
            Abbreviation = string.Empty;
            FullName = string.Empty;

            TotalConstituencyVote = 0;
            TotalListVote = 0;

            PercentageConstituencyVote = 0f;
            PercentageListVote = 0f;
        }

        public PartyVote(PartyVote src)
        {
            Abbreviation = src.Abbreviation;
            FullName = src.FullName;

            TotalConstituencyVote = src.TotalConstituencyVote;
            TotalListVote = src.TotalListVote;

            PercentageConstituencyVote = src.PercentageConstituencyVote;
            PercentageListVote = src.PercentageListVote;
        }
    }
}
