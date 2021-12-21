namespace LT.DigitalOffice.MessageService.Models.Dto.Configurations
{
  public record MemoryCacheConfig
  {
    public const string SectionName = "MemoryCache";
    public double CacheLiveInMinutes { get; set; }
  }
}
