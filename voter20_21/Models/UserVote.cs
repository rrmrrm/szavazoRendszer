
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voter20_21.Models
{
    public class UserVote
    {
        [Key]
        public Int32 Id { get; set; }

        [Required]
        public Int32 userId { get; set; }
        public virtual User user { get; set; }

        public Boolean Voted { get; set; } = false;

        [Required]
        public Int32 assignedUsersId { get; set; }
        public virtual AssignedUser assignedUsers { get; set; }
    }
}
