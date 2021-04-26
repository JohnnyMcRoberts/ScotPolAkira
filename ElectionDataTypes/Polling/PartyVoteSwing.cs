namespace ElectionDataTypes.Polling
{
    using System.Linq;

    using Results;

    public class PartyVoteSwing : PartyVote
    {
        public int TotalConstituencyVoteSwing { get; set; }
        public int TotalListVoteSwing { get; set; }
        public int TotalSeatSwing { get; set; }

        public float PercentageConstituencyVoteSwing { get; set; }
        public float PercentageListVoteSwing { get; set; }

        public PartyVoteSwing()
        {
            TotalConstituencyVoteSwing = 0;
            TotalListVoteSwing = 0;
            TotalSeatSwing = 0;

            PercentageConstituencyVoteSwing = 0f;
            PercentageListVoteSwing = 0f;
        }

        public PartyVoteSwing(PartyVoteSwing src) : base(src)
        {
            TotalConstituencyVoteSwing = src.TotalConstituencyVoteSwing;
            TotalListVoteSwing = src.TotalListVoteSwing;
            TotalSeatSwing = src.TotalListVoteSwing;
            PercentageConstituencyVoteSwing = src.PercentageConstituencyVoteSwing;
            PercentageListVoteSwing = src.PercentageListVoteSwing;
        }

        public PartyVoteSwing(PartyVote partyVote, ElectionResult previousResult): base(partyVote)
        {
            // See if this party stood in the previous election.
            PartyVote previousVote = 
                previousResult.PartyVotes.FirstOrDefault(x => x.Abbreviation == partyVote.Abbreviation);
            if (previousVote != null)
            {
                // Set up the swings based on the previous
                PercentageConstituencyVoteSwing = 
                    partyVote.PercentageConstituencyVote - previousVote.PercentageConstituencyVote;
                PercentageListVoteSwing = 
                    partyVote.PercentageListVote - previousVote.PercentageListVote;
            }
            else
            {
                // Otherwise the swing is the current predicted vote
                PercentageConstituencyVoteSwing = partyVote.PercentageConstituencyVote;
                PercentageListVoteSwing = partyVote.PercentageListVote;
            }
        }
    }
}
