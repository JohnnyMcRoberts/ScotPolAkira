namespace ElectionDataTypes.Polling
{
    using System;
    using System.Collections.Generic;

    using Results;

    public class OpinionPoll
    {
        public string PollingCompany { get; set; }

        public DateTime PublicationDate { get; set; }

        public string Link { get; set; }

        public List<PartyResult> ConstituencyPredictions { get; set; }

        public List<PartyResult> ListPredictions { get; set; }

        public OpinionPoll()
        {
            PollingCompany = string.Empty;
            PublicationDate = DateTime.MinValue;
            Link = string.Empty;
            ConstituencyPredictions = new List<PartyResult>();
            ListPredictions = new List<PartyResult>();
        }

        public OpinionPoll(OpinionPoll src)
        {
            PollingCompany = src.PollingCompany;
            PublicationDate = src.PublicationDate;
            Link = src.Link;
            ConstituencyPredictions = new List<PartyResult>();
            foreach (PartyResult partyPrediction in src.ConstituencyPredictions)
            {
                ConstituencyPredictions.Add(new PartyResult(partyPrediction));
            }

            ListPredictions = new List<PartyResult>();
            foreach (PartyResult partyPrediction in src.ListPredictions)
            {
                ListPredictions.Add(new PartyResult(partyPrediction));
            }
        }
    }
}
