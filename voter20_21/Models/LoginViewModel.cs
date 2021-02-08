using System;
using System.ComponentModel.DataAnnotations;

namespace voter20_21.Models
{
    /// <summary>
    /// Felhasználóval kapcsolatos információk.
    /// </summary>
    public class LoginViewModel
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
        /// <summary>
        /// Bejelentkezés megjegyzése.
        /// </summary>
        public Boolean RememberLogin { get; set; }
	}
}