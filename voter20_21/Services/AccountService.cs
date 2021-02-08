using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using voter20_21.Models;

/// TODO: a mintabedando megmutatja, hogyan lehet szepen szamontartani a bejelentkezett user emailcímét és azt megjeleníteni minden oldalon(bár megjeleniteni az emailcimet asszem asszem nem elvárt
namespace voter20_21.Services
{
    public class AccountService
    {
        private readonly voterContext vC;
        private readonly HttpContext httpContext;
        private readonly ApplicationState applicationState;
        public AccountService(voterContext _vC, IHttpContextAccessor _httpContextAccessor,
            ApplicationState _applicationState)
        {
            vC = _vC;
            httpContext = _httpContextAccessor.HttpContext;
            applicationState = _applicationState;

            //ha van sütije a felhasználonak és nincs bejelentkeztetve, akkor bejelentkezünk neki:
            if( httpContext.Request.Cookies.ContainsKey("user_challenge") &&
                     !httpContext.Session.Keys.Contains("user") ){

                //ha nincs sessionbe bejelentkezve, de van cookija, akkor felvesszük 
                User user = vC.Users.FirstOrDefault(u =>
                   u.userChallenge == httpContext.Request.Cookies["user_challenge"]);
                if(user != null)
                {
                    httpContext.Session.SetString("user", user.email);
                    UserCount++;
                }
            }
        }
        public Boolean Login(LoginViewModel user)
        {
            if(user == null)
            {
                return false;
            }
            //validáció:

            //data annotációk ellenőrzése:
            if (!Validator.TryValidateObject(user, new ValidationContext(user, null, null), null))
            {
                return false;
            }

            //autentikáció:

            //megnézzük regisztrálva van-e már az e-mail cím:
            User foundUser = vC.Users.FirstOrDefault(c => c.email == user.email);
            if(foundUser == null)
            {
                return false;
            }
            Byte[] passwordBytes = null;
            using (SHA512CryptoServiceProvider provider = new SHA512CryptoServiceProvider())
            {
                passwordBytes = provider.ComputeHash(Encoding.UTF8.GetBytes(user.password));
            }
            if(passwordBytes == null)
            {
                return false;
            }

            if (!passwordBytes.SequenceEqual(foundUser.password))
            {
                return false;
            }
            httpContext.Session.SetString("user", user.email);
            /*
            if (user.RememberLogin)
            {
                httpContext.Response.Cookies.Append("user_challenge", foundUser.userChallenge,
                    new CookieOptions
                    {
                        Expires = DateTime.Today.AddDays(365),
                        HttpOnly = true
                    });
            }*/
            UserCount++;
            return true;
        }
        public Int32 UserCount
        {
            get => (Int32)applicationState.UserCount;
            set => applicationState.UserCount = value;
        }
        public String CurrentUserName => httpContext.Session.GetString("user");
        public Boolean Logout()
        {
            if (!httpContext.Session.Keys.Contains("user"))
            {
                return false;
            }
            httpContext.Session.Remove("user");
            httpContext.Response.Cookies.Delete("user-challenge");
            UserCount--;
            return true;
        }
        public Boolean Register(RegistrationViewModel user)
        {
            if(user == null)
            {
                return false;
            }
            if (!Validator.TryValidateObject(user, new ValidationContext(user, null, null), null))
            {
                return false;
            }//ez a szamolas lassu, valszeg indexelni kene az email property-t
            if(vC.Users.Count(c => c.email == user.email) != 0)
            {
                return false;
            }
            Byte[] passwordBytes = null;

            using (SHA512CryptoServiceProvider provider = new SHA512CryptoServiceProvider())
            {
                passwordBytes = provider.ComputeHash(Encoding.UTF8.GetBytes(user.password));
            }
            if (passwordBytes == null)
            {
                return false;
            }
            vC.Users.Add( new User 
            {
                email = user.email,
                password = passwordBytes,
                userChallenge = Guid.NewGuid().ToString()
            });
            try
            {
                vC.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
            //todo: ezután a controllereket is meg kell irni(4. előadás 8:20)
        }
    }
}
