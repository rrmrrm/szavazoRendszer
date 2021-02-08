using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace voter20_21.Models
{
    public class Voting
    {
        [Key]
        public Int32 Id { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string question { get; set; }
        //TODO: megnézni, hogy a saját validációimat is végrhajtja e a kliens és a szerver
        //TODO: validálni a start és end szerint a Voting-ot létrehozáskor
        [Required]
        [DataType(DataType.Date)]
        [DateMoreThanCurrentDate(ErrorMessage = "A kezdőidőpontnak jövőbeninek kell lennie.")]
        public DateTime start { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DateMoreThan(nameof(start),ErrorMessage ="A lejárati időnek nagyobbnak kell lennie, mint a kezdeti időnek(mindkettő megadása kötelező.)")]
        public DateTime end { get; set; }

        public ICollection<AssignedUser> assignedUsers { get; set; }
        public ICollection<Answer> answers { get; set; }

        public Voting()
        {
            assignedUsers = new HashSet<AssignedUser>();
            answers = new HashSet<Answer>();
        }
        [Required]
        public Int32 creatorUserId { get; set; }
    }
    //TODO: mi van, ha az egyik property nem required, és ezért nincs megadva? hibát jelez a validáció?
    public class DateMoreThan : ValidationAttribute, IClientModelValidator
    {
        protected readonly string comparisonProperty;
        public DateMoreThan(string _comparisonProperty)
        {
            comparisonProperty = _comparisonProperty;
        }
        protected override ValidationResult IsValid(object value, ValidationContext vContext)
        {
            if(value == null)
            {
                return ValidationResult.Success;
            }
            ErrorMessage = ErrorMessageString;
            var currentValue = (DateTime)value;

            var property = vContext.ObjectType.GetProperty(comparisonProperty);
            if (property == null)
            {
                throw new ArgumentException("Property with this name not found");
            }
            var comparisonValue = (DateTime)property.GetValue(vContext.ObjectInstance);
            if(comparisonValue == null)
            {
                return ValidationResult.Success;
            }
            if (currentValue < comparisonValue)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
        public void AddValidation(ClientModelValidationContext context)
        {
            var error = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-error", error);
        }
    }
    public class DateMoreThanCurrentDate : DateMoreThan
    {
        public DateMoreThanCurrentDate() :
            base("A DateMoreThanCurrentDate osztaly nem hasznal comparitionProperty-t")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext vContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            ErrorMessage = ErrorMessageString;
            var currentValue = (DateTime)value;
            /*ebben az osztályban nem használjuk a comparisionProperty-t, helyette a jelenlegi idővel hasonlítjuk össze a currentValue-t:
            var property = vContext.ObjectType.GetProperty(comparisonProperty);
            
            if (property == null)
            {
                throw new ArgumentException("Property with this name not found");
            }
            var comparisonValue = (DateTime)property.GetValue(vContext.ObjectInstance);
            */
            //a jelenlegi idővel hasonlítjuk össze a currentValue-t:
            if (currentValue < DateTime.Now)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
