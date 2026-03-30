namespace PMS.Web.Helpers
{
    /// <summary>
    /// Generates unique Petition Application IDs in format: PMS-YYYY-NNNNN
    /// e.g. PMS-2024-00001
    /// </summary>
    public static class PetitionIdGenerator
    {
        public static string Generate(int sequenceNumber, int? year = null)
        {
            int y = year ?? DateTime.UtcNow.Year;
            return $"PMS-{y}-{sequenceNumber:D5}";
        }
    }
}
