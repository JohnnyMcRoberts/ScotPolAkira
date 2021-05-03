namespace ElectionDataTypes.Polling
{
    using System;
    using System.Collections.Generic;

    using Results;

    public class OpinionPoll
    {
        #region Private Data

        private string _link = string.Empty;

        #endregion

        #region Public Properties

        public string PollingCompany { get; set; }

        public DateTime PublicationDate { get; set; }

        public string Link
        {
            get => _link;
            set
            {
                _link = value;
                LinkUri = string.IsNullOrWhiteSpace(_link) ? null : new Uri(_link);
            }
        }

        public Uri LinkUri { get; private set; }

        public List<PartyResult> ConstituencyPredictions { get; set; }

        public List<PartyResult> ListPredictions { get; set; }

        #endregion


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
