using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.MessageService.Models.Db
{
    public class DbMessage
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int Status { get; set; }

        public Guid SenderUserId { get; set; }
        public ICollection<DbMessageRecipientUser> RecipientUsersIds { get; set; }
        public ICollection<DbMessageFile> FilesIds { get; set; }
    }
}
