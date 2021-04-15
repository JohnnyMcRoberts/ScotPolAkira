namespace ElectionDataTypes
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ConstituencyResult
    {
        public string Code { get; set; }
        public string Constituency { get; set; }
        public string Region { get; set; }
        public int Electorate { get; set; }
        public int TotalBallotsAtTheCount { get; set; }
        public int TotalValidVotesCast { get; set; }
        public int RejectedBallots { get; set; }
        public float BallotBoxTurnoutPercentage { get; set; }
        public float RejectedBallotsPercentage { get; set; }

        public List<PartyConstituencyResult> PartyResults { get; set; }
        public string Win { get; set; }
        public string Second { get; set; }
        public int Majority { get; set; }
        public float MajorityPercentage { get; set; }

        public ConstituencyResult()
        {
            Code = string.Empty;
            Constituency = string.Empty;
            Region = string.Empty;

            Electorate = 0;
            TotalBallotsAtTheCount = 0;
            TotalValidVotesCast = 0;
            RejectedBallots = 0;

            BallotBoxTurnoutPercentage = 0f;
            RejectedBallotsPercentage = 0f;

            PartyResults = new List<PartyConstituencyResult>();

            Win = string.Empty;
            Second = string.Empty;
            Majority = 0;
            MajorityPercentage = 0f;
        }
    }
}
