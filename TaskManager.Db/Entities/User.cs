using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Db.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public string Login { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
