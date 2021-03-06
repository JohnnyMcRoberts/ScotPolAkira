namespace ScotPolWpfApp.Utilities
{
    using ScotPolWpfApp.PlotGenerators;

    public enum PlotType
    {
        [PlotType(Title = "List votes with time",
            CanHover = false,
            GeneratorClass = typeof(ListVotesWithTime))]
        ListVotesWithTime,

        [PlotType(Title = "Constituency votes with time",
            CanHover = false,
            GeneratorClass = typeof(ConstituencyVotesWithTime))]
        ConstituencyVotesWithTime,

        [PlotType(Title = "Total seats with time",
            CanHover = false,
            GeneratorClass = typeof(TotalSeatsWithTime))]
        TotalSeatsWithTime,

        [PlotType(Title = "Total stacked seats with time",
            CanHover = false,
            GeneratorClass = typeof(TotalSeatsStackedWithTime))]
        TotalSeatsStackedWithTime,

    }
}
