using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace voter20_21.Models
{
    public class VotingsAssignedToUserViewModel
    {
        [Key]
        public Int32 Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string question { get; set; }
        /// <summary>
        /// TODO: validálni a start és end szerint a Voting-ot létrehozáskor
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime start { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime end { get; set; }

        [Required]
        [DisplayName("user")]/////<--valtoz
        public Int32 userId { get; set; }
        public virtual User user { get; set; }

        public Boolean Voted { get; set; } = false;
    }
}
