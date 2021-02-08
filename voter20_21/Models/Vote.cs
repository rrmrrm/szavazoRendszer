using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace voter20_21.Models
{
    public class Vote
    {
        [Key]
        public Int32 Id { get; set; }

        [Required]
        [DisplayName("answer")]
        public Int32 answerId { get; set; }
        public virtual Answer answer{ get; set; }

        [Required]
        public Int32 votingId { get; set; }
    }
}
