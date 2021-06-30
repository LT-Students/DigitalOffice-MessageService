namespace LT.DigitalOffice.MessageService.Models.Dto.Configurations
{
    public class EmailEngineConfig
    {
        public const string SectionName = "EmailEngineConfig";

        public int ResendIntervalInMinutes { get; set; }
    }
}
