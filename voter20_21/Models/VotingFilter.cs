using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace voter20_21.Models
{
    public class VotingFilter
    {

        public String title { get; set; }
        [DataType(DataType.Date)]
        public DateTime? minStart { get; set; }


        [DataType(DataType.Date)]
        [DateMoreThan(nameof(minStart), ErrorMessage = "A vég időpont szűrőfeltételnek későbbinek kell lennie a kezdeti időpont szűrőfeltételnél.")]
        public DateTime? maxEnd { get; set; }
    }
}
