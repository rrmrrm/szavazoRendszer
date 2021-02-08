
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voter20_21.Models
{
    public class AssignedUser
    {
        [Key]
        public Int32 Id { get; set; }

        [Required]
        [DisplayName("user")]
        public Int32 userId { get; set; }
        public virtual User user { get; set; }

        public Boolean Voted { get; set; } = false;

        [Required]/////<--valtoz
        [DisplayName("voting")]
        public Int32 votingId { get; set; }
        public virtual Voting voting { get; set; }

    }
}
