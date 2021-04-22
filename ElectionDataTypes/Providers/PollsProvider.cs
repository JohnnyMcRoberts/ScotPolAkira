namespace ElectionDataTypes.Providers
{
    using System.Collections.Generic;
    using System.Linq;

    using Interfaces;
    using Polling;

    public class PollsProvider : IPollsProvider
    {
        #region Properties

        public List<OpinionPoll> PollsByDate { get; }
        public Dictionary<string, List<OpinionPoll>> PollsByPollster { get; }
        public List<string> PosterNames { get; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PollsProvider"/> class.
        /// </summary>
        public PollsProvider()
        {
            PollsByDate = new List<OpinionPoll>();
            PollsByPollster = new Dictionary<string, List<OpinionPoll>>();
            PosterNames = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PollsProvider"/> class.
        /// </summary>
        /// <param name="polls">The polls it is constructed from.</param>
        public PollsProvider(List<OpinionPoll> polls) : this()
        {
            PollsByDate = new List<OpinionPoll>();
            PollsByPollster = new Dictionary<string, List<OpinionPoll>>();
            PosterNames = new List<string>();

            foreach (OpinionPoll poll in polls)
            {
                if (!PollsByPollster.ContainsKey(poll.PollingCompany))
                {
                    PollsByPollster.Add(poll.PollingCompany, new List<OpinionPoll> { new OpinionPoll(poll) });
                }
                else
                {
                    PollsByPollster[poll.PollingCompany].Add(new OpinionPoll(poll));
                }
            }

            PosterNames = PollsByPollster.Keys.OrderBy(x => x).ToList();
            PollsByDate = polls.OrderBy(x => x.PublicationDate).ToList();
        }
    }
}
