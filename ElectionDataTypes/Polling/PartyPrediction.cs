using System;
using System.Globalization;

namespace ElectionDataTypes.Polling
{
    public class PartyPrediction
    {
        #region Constants

        public enum Parties
        {
            SNP = 0,
            CON,
            LAB,
            LIB,
            SGRN,
            ALBA
        };

        public readonly string[] PartiesList =
        {
            "SNP",
            "CON",
            "LAB",
            "LIB",
            "SGRN",
            "ALBA"
        };

        #endregion

        #region Public Properties

        public float SNP { get; set; }
        public float CON { get; set; }
        public float LAB { get; set; }
        public float LIB { get; set; }
        public float SGRN { get; set; }
        public float ALBA { get; set; }

        #endregion

        #region Public Properties

        public void SetPartyValue(string party, float value)
        {
            for (int i = 0; i < PartiesList.Length; i++)
            {
                if (PartiesList[i] == party)
                {
                    // Convert to enum.
                    Parties partyValue = (Parties)Enum.ToObject(typeof(Parties), i);
                    SetValue(value, partyValue);
                }
            }
        }

        public void SetValue(float value, Parties partyValue)
        {
            switch (partyValue)
            {
                case Parties.SNP:
                    SNP = value;
                    break;

                case Parties.CON:
                    CON = value;
                    break;

                case Parties.LAB:
                    LAB = value;
                    break;

                case Parties.LIB:
                    LIB = value;
                    break;

                case Parties.SGRN:
                    SGRN = value;
                    break;

                case Parties.ALBA:
                    ALBA = value;
                    break;
            }
        }

        public float GetPartyValue(string party)
        {
            for (int i = 0; i < PartiesList.Length; i++)
            {
                if (PartiesList[i] == party)
                {
                    // Convert to enum.
                    Parties partyValue = 
                        (Parties)Enum.ToObject(typeof(Parties), i);

                    switch (partyValue)
                    {
                        case Parties.SNP:
                            return SNP;

                        case Parties.CON:
                            return CON;

                        case Parties.LAB:
                            return LAB;

                        case Parties.LIB:
                            return LIB;

                        case Parties.SGRN:
                            return SGRN;

                        case Parties.ALBA:
                            return ALBA;
                    }
                }
            }

            return 0f;
        }

        #endregion

        public PartyPrediction()
        {
            SNP = 0f;
            CON = 0f;
            LAB = 0f;
            LIB = 0f;
            SGRN = 0f;
            ALBA = 0f;
        }

        public PartyPrediction(PartyPrediction src)
        {
            SNP = src.SNP;
            CON = src.CON;
            LAB = src.LAB;
            LIB = src.LIB;
            SGRN = src.SGRN;
            ALBA = src.ALBA;
        }
    }
}
