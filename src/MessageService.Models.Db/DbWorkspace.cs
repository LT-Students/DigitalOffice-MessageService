using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.MessageService.Models.Db
{
    public class DbWorkspace
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
