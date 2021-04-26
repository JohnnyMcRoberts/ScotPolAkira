namespace ElectionDataTypes.Results
{
    /// <summary>
    /// This is note for a party from the data file
    /// </summary>
    public class PartyNote
    {
        /// <summary>
        /// Gets and sets the full name of a party.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets and sets the abbreviation for a party.
        /// </summary>
        public string Abbreviation { get; set; }

        public PartyNote()
        {
            FullName = string.Empty;
            Abbreviation = string.Empty;
        }
    }
}
