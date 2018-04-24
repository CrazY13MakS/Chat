using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AuthServiceProvider.Model
{
    public class RegexUtilities
    {
        bool invalid = false;

        public bool IsValidEmail(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid email format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        /// <summary>
        /// This requires at least one digit, at least one alphabetic character, no special characters, and from 6-24 characters in length.
        /// </summary>
        /// <param name="input">Password</param>
        /// <returns></returns>
        public bool IsValidPassword(String input)
        {
            ;
            if (String.IsNullOrEmpty(input))
            {
                return false;
            }
            try
            {
                return Regex.IsMatch(input, @"(?=^.{6,24}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$");
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        /// <summary>
        /// Логин должен начинаться с латинской буквы в нижнем регистре или знака "_",далее может содержать латинские буквы в нижнем регистре, знак "_" и цифры. Длина от 5 до 50 символов
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsValidLogin(String input)
        {
            ;
            if (String.IsNullOrEmpty(input))
            {
                return false;
            }
            try
            {
                return Regex.IsMatch(input, "^[a-z_][a-z0-9_]{5,50}$");
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
    }
}
