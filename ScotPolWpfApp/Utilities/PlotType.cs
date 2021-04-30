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

    }
}
