using System;
using ElectionDataTypes.Results;

namespace ElectionDataTypes.Polling
{
    public class PollingPrediction
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

        public PartyPrediction ConstituencyPercentages { get; set; }
        public PartyPrediction ListPercentages { get; set; }

        public PartyPrediction ConstituencySeats { get; set; }
        public PartyPrediction ListSeats { get; set; }

        public PartyPrediction TotalSeats { get; set; }

        #endregion

        #region Public Methods


        #endregion

        public PollingPrediction()
        {
            PollingCompany = string.Empty;
            PublicationDate = DateTime.MinValue;
            Link = string.Empty;

            ConstituencyPercentages = new PartyPrediction();
            ListPercentages = new PartyPrediction();
            ConstituencySeats = new PartyPrediction();
            ListSeats = new PartyPrediction();
            TotalSeats = new PartyPrediction();
        }

        public PollingPrediction(ElectionPrediction electionPrediction, OpinionPoll poll)
        {
            // Copy the polly
            PollingCompany = poll.PollingCompany;
            PublicationDate = poll.PublicationDate;
            Link = poll.Link;

            // First the percentages.
            ConstituencyPercentages = new PartyPrediction();
            ListPercentages = new PartyPrediction();
            foreach (PartyResult prediction in poll.ConstituencyPredictions)
            {
                ConstituencyPercentages.SetPartyValue(prediction.PartyAbbreviation, prediction.PercentageOfVotes);
            }

            foreach (PartyResult prediction in poll.ListPredictions)
            {
                ListPercentages.SetPartyValue(prediction.PartyAbbreviation, prediction.PercentageOfVotes);
            }

            // Finally the seats.
            ConstituencySeats = new PartyPrediction();
            ListSeats = new PartyPrediction();
            TotalSeats = new PartyPrediction();

            foreach (PartyVoteSwing partyPrediction in electionPrediction.PartyPredictions)
            {
                ConstituencySeats.SetPartyValue(partyPrediction.Abbreviation, partyPrediction.ConstituencySeats);
                ListSeats.SetPartyValue(partyPrediction.Abbreviation, partyPrediction.ListSeats);
                TotalSeats.SetPartyValue(partyPrediction.Abbreviation, partyPrediction.TotalSeats);
            }
        }
    }
}
