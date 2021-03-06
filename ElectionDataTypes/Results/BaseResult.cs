namespace ElectionDataTypes.Results
{
    using System.Collections.Generic;

    public abstract class BaseResult
    {
        public string Region { get; set; }
        public int Electorate { get; set; }
        public int TotalBallotsAtTheCount { get; set; }
        public int TotalValidVotesCast { get; set; }
        public int RejectedBallots { get; set; }
        public float BallotBoxTurnoutPercentage { get; set; }
        public float RejectedBallotsPercentage { get; set; }
        
        public List<PartyResult> PartyResults { get; set; }

        protected BaseResult()
        {
            Region = string.Empty;

            Electorate = 0;
            TotalBallotsAtTheCount = 0;
            TotalValidVotesCast = 0;
            RejectedBallots = 0;

            BallotBoxTurnoutPercentage = 0f;
            RejectedBallotsPercentage = 0f;

            PartyResults = new List<PartyResult>();
        }

        protected BaseResult(BaseResult src)
        {
            Region = src.Region;

            Electorate = src.Electorate;
            TotalBallotsAtTheCount = src.TotalBallotsAtTheCount;
            TotalValidVotesCast = src.TotalValidVotesCast;
            RejectedBallots = src.RejectedBallots;

            BallotBoxTurnoutPercentage = src.BallotBoxTurnoutPercentage;
            RejectedBallotsPercentage = src.RejectedBallotsPercentage;

            PartyResults = new List<PartyResult>();
            foreach (PartyResult result in src.PartyResults)
            {
                PartyResults.Add(new PartyResult(result));
            }
        }
    }
}
