namespace ElectionDataTypes.Interfaces
{
    using System.Collections.Generic;

    using Polling;

    public interface IPollsProvider
    {
        /// <summary>
        /// Gets the polls by date of publication.
        /// </summary>
        List<OpinionPoll> PollsByDate { get; }

        /// <summary>
        /// Gets the polls by pollster.
        /// </summary>
        Dictionary<string, List<OpinionPoll>> PollsByPollster { get; }

        /// <summary>
        /// Gets the names of the polling companies.
        /// </summary>
        List<string> PosterNames { get; }
    }
}
