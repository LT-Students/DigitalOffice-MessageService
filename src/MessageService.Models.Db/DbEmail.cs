using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.MessageService.Models.Db
{
    public class DbEmail
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? SenderId { get; set; }
        [Required]
        public DateTime Time { get; set; }
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }

        [Required]
        public ICollection<DbEmailReciever> Receivers { get; set; }
    }
}