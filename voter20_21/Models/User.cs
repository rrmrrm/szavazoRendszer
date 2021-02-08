using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Security;
namespace voter20_21.Models
{
    public class User
    {
        [Key]
        public Int32 Id { get; set; }

        [Required]
        [MaxLength(100)]
        public String email { get; set; }

        [Required]
        public Byte[] password { get; set; }

        public ICollection<AssignedUser> assignedUsers { get; set; }

        public string userChallenge { get; set; }
        public User()
        {
            assignedUsers = new HashSet<AssignedUser>();
        }
    }
}
