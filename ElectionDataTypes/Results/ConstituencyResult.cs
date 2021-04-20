namespace ElectionDataTypes.Results
{
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
    }
}
