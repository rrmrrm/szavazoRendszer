using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace voter20_21.Models
{
    public class Answer
    {
        [Key]
        public Int32 Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string text { get; set; }

        [Required]
        [DisplayName("voting")]
        public Int32 votingId { get; set; }
        public virtual Voting voting { get; set; }
    }
}
