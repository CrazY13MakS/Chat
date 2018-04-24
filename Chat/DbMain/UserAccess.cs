using DbMain.EFDbContext;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DbMain
{
    public class UserAccess
    {
        ChatEntities chatContext;
        /// <summary>
        /// Получить токен для входа. Если строка начинается с "Error!" - ошибка
        /// </summary>
        /// <param name="email">Адрес электронной почты</param>
        /// <param name="password">Пароль</param>
        /// <returns>Токен</returns>
        public String LogIn(String email, String password)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                return $"Error! {nameof(email)} can't be null";
                //throw new ArgumentNullException($"Error! {nameof(email)} can't be null");
            }
            if (String.IsNullOrWhiteSpace(password))
            {
                return $"Error! {nameof(password)} can't be null";
                // throw new ArgumentNullException($"Error! {nameof(password)} can't be null");
            }
            RegexUtilities regex = new RegexUtilities();
            if (!regex.IsValidEmail(email))
            {
                return $"Error! Invalid Email adress";
                // throw new ArgumentException($"Error! Invalid Email adress");
            }
            if (!regex.IsValidPassword(password))
            {
                return "Error! Invalid password. This requires at least one digit, at least one alphabetic character, no special characters, and from 6-15 characters in length.";
                //throw new ArgumentException("Error! Invalid password. This requires at least one digit, at least one alphabetic character, no special characters, and from 6-15 characters in length.");
            }
            try
            {
                using (chatContext = new ChatEntities())
                {
                    var user = chatContext.Users.FirstOrDefault(x => x.Email == email);
                    if (user == null)
                    {
                        return "Error! User not found!";
                    }
                    if (user == null || !PasswordCrypt.ComparePasswords(password, user.PasswordHash, user.PasswordSalt))
                    {
                        return "Error! Login and/or password do not match";
                    }
                    var token = GenerateToken(user);
                    chatContext.AccessTokens.Add(token);
                    int res = chatContext.SaveChanges();
                    if (res == 1)
                    {
                        return token.Token;
                    }
                    else
                    {
                        return "Error! Internal service Error";
                    }
                }
            }
            catch (Exception)
            {
                return "Error! Internal service Error";
            }


        }
        private AccessToken GenerateToken(User user)
        {
            var token = new AccessToken()
            {
                Token = CreateToken(),
                UserId = user.Id
            };
            return token;
        }
        private String CreateToken()
        {
            var guid = Guid.NewGuid().ToString();
            return guid + DateTime.UtcNow.ToLongTimeString();
        }


        bool SendVerificationCode(String email)
        {

        }

    }

}
