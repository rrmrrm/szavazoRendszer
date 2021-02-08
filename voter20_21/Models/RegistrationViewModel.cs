using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace voter20_21.Models
{
    //TODO: szerveroldalon is ellenőrizni kell a modelleket
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "A mév megadása kötelező")]
        [StringLength(100, ErrorMessage = "A név legfeljebb 100 karakter lehet")]
        [EmailAddress(ErrorMessage = "az e-mail cím rossz formázumú")]
        [DataType(DataType.EmailAddress)]
        public String email { get; set; }

        [Required(ErrorMessage = "A jelszó megadása kötelező")]
        [RegularExpression("^[a-zA-Z1-9 _0-]{6,50}$", ErrorMessage = "A jelszó formátuma nem megfelelő: minimum 6, maximum 50 karakter hosszú lehet. A jelszó csak az angol ábécé kis -és nagy- betűit, szóközt, számokat és a '-' és '_' karaktereket tartalmazhatja")]
        [DataType(DataType.Password)]
        public String password { get; set; }

        [Required(ErrorMessage = "Megerősítő jelszó megadása kötelező")]
        [Compare(nameof(password), ErrorMessage = "A két jelszó nem egyezik")]
        [DataType(DataType.Password)]
        public String confirmPassword { get; set; }
    }
}
