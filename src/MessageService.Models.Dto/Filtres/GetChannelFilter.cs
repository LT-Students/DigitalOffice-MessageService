using LT.DigitalOffice.Kernel.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.MessageService.Models.Dto.Filtres
{
  public record GetChannelFilter : BaseFindFilter
  {
    [FromQuery(Name = "includeUsers")]
    public bool IncludeUsers { get; set; }

    [FromQuery(Name = "includeMessages")]
    public bool IncludeMessages { get; set; }
  }
}
