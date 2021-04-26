namespace ElectionDataTypes.Results
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ConstituencyResult : BaseResult
    {
        public string Code { get; set; }
        public string Constituency { get; set; }

        public string Win { get; set; }
        public string Second { get; set; }
        public int Majority { get; set; }
        public float MajorityPercentage { get; set; }

        public ConstituencyResult()
        {
            Code = string.Empty;
            Constituency = string.Empty;

            Win = string.Empty;
            Second = string.Empty;
            Majority = 0;
            MajorityPercentage = 0f;
        }

        public ConstituencyResult(ConstituencyResult src) : base(src)
        {
            Code = src.Code;
            Constituency = src.Constituency;

            Win = src.Win;
            Second = src.Second;
            Majority = src.Majority;
            MajorityPercentage = src.MajorityPercentage;
        }

        public ConstituencyResult(
            ConstituencyResult src, 
            Dictionary<string, float> constituencySwingsByParty) : this(src)
        {
            Code = src.Code;
            Constituency = src.Constituency;

            // Loop through the party results adding the swings to the previous totals.
            for (int i = 0; i < PartyResults.Count; i++)
            {
                string partyAbbreviation =
                    PartyResults[i].PartyAbbreviation;
                float percentageMultiplier = 100f / TotalValidVotesCast;
                if (constituencySwingsByParty.ContainsKey(partyAbbreviation))
                {
                    double swingMultiplier = constituencySwingsByParty[partyAbbreviation] / 100d;
                    int swingVotes = (int)Math.Round(swingMultiplier * TotalValidVotesCast);
                    PartyResults[i].Votes += swingVotes;
                    if (PartyResults[i].Votes < 1)
                    {
                        PartyResults[i].Votes = 0;
                    }

                    PartyResults[i].PercentageOfVotes = 
                        percentageMultiplier * PartyResults[i].Votes;
                }
            }

            // Loop through the swings adding any that did not stand previously.
            foreach (string partyName in constituencySwingsByParty.Keys)
            {
                if (PartyResults.All(x => x.PartyAbbreviation != partyName))
                {
                    float percentageOfVotes = constituencySwingsByParty[partyName];
                    double swingMultiplier = percentageOfVotes / 100d;
                    int swingVotes = (int)Math.Round(swingMultiplier * TotalValidVotesCast);

                    PartyResults.Add(
                        new PartyResult
                        {
                            PartyAbbreviation = partyName, 
                            PercentageOfVotes = percentageOfVotes,
                            Votes = swingVotes
                        });
                }
            }

            // Sort the result to get the winner and the majority
            List<PartyResult> sortedResults = PartyResults.OrderByDescending(x => x.Votes).ToList();

            PartyResult first = sortedResults[0];
            PartyResult second = sortedResults[1];

            Win = first.PartyAbbreviation;
            Second = second.PartyAbbreviation;
            Majority = first.Votes - second.Votes;
            MajorityPercentage = first.PercentageOfVotes - second.PercentageOfVotes;
        }
    }
}
