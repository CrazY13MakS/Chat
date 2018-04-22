using DbMain.EFDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServiceProvider.Model
{
    public class UserAccess:IDisposable
    {
        private readonly String smtpHost = "smtp.gmail.com";
        private readonly String smtpFrom = "mytempsmtp@gmail.com";
        private readonly String smtpPass = "TempMail2018";
        private readonly String smtpCaption = "MyChatBeta";

        ChatEntities chatContext;

        /// <summary>
        /// Получить токен для входа. 
        /// </summary>
        /// <param name="email">Адрес электронной почты</param>
        /// <param name="password">Пароль</param>
        /// <returns>Item1 - успех операции, Item2 - сообщение об ошибке или результат</returns>
        public Tuple<bool,String> LogIn(String email, String password)
        {
            var resultEmail = IsValidEmail(email);
            if (!resultEmail.Item1)
            {
                return resultEmail;
            }
            var resultPassword = IsValidPassword(password);
            if (!resultPassword.Item1)
            {
                return resultPassword;
            }
            try
            {
                using (chatContext = new ChatEntities())
                {
                    var user = chatContext.Users.FirstOrDefault(x => x.Email == email);
                    if (user == null || !PasswordCrypt.ComparePasswords(password, user.PasswordHash, user.PasswordSalt))
                    {
                        return new Tuple<bool, string>(false, "Error! Login and/or password do not match");
                    }
                    var token = GenerateToken(user);
                    chatContext.AccessTokens.Add(token);
                    int res = chatContext.SaveChanges();
                    if (res == 1)
                    {
                        return new Tuple<bool, string>(true, token.Token);
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "Error! Internal service Error");
                    }
                }
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, ex.Message);
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
        public Tuple<bool, String> IsValidPassword(String password)
        {
            if (String.IsNullOrWhiteSpace(password))
            {
                return new Tuple<bool, string>(false, $"Error! {nameof(password)} can't be null");
                //throw new ArgumentNullException($"Error! {nameof(email)} can't be null");
            }
            RegexUtilities regex = new RegexUtilities();
            if (!regex.IsValidPassword(password))
            {
                return new Tuple<bool, string>(false, "Error! Invalid password. This requires at least one digit, at least one alphabetic character, no special characters, and from 6-15 characters in length.");
                // throw new ArgumentException($"Error! Invalid Email adress");
            }
            return new Tuple<bool, string>(true, String.Empty);
        }

        public Tuple<bool, String> IsValidEmail(String email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                return new Tuple<bool, string>(false, $"Error! {nameof(email)} can't be null");
                //throw new ArgumentNullException($"Error! {nameof(email)} can't be null");
            }
            RegexUtilities regex = new RegexUtilities();
            if (!regex.IsValidEmail(email))
            {
                return new Tuple<bool, string>(false, $"Error! Invalid Email adress");
                // throw new ArgumentException($"Error! Invalid Email adress");
            }
            return new Tuple<bool, string>(true, String.Empty);
        }

        public Tuple<bool, String> IsValidLogin(String login)
        {
            if (String.IsNullOrWhiteSpace(login))
            {
                return new Tuple<bool, string>(false, $"Error! {nameof(login)} can't be null");
                //throw new ArgumentNullException($"Error! {nameof(email)} can't be null");
            }
            RegexUtilities regex = new RegexUtilities();
            if (!regex.IsValidLogin(login))
            {
                return new Tuple<bool, string>(false, $"Error! Invalid login. Length 5-50, only a-z, 0-9, '_'.Start only with a-z ");
                // throw new ArgumentException($"Error! Invalid Email adress");
            }
            return new Tuple<bool, string>(true, String.Empty);
        }
        /// <summary>
        /// Отправка сообщения на адрес электронной почты
        /// </summary>
        /// <param name="email"></param>
        /// <param name="message"></param>
        /// <returns></returns>
       public bool SendVerificationCode(String email, String message)
        {
            if (String.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException($"{nameof(message)} can't be null");
            }
            if (!IsValidEmail(email).Item1)
            {
                return false;
            }
            SmtpServer mail = new SmtpServer(smtpHost, smtpFrom, smtpPass);
            return mail.SendMail(email, smtpCaption, message);
        }

        public String GenerateVerificationCode(int length = 5)
        {
            return new String(Guid.NewGuid().ToString().Take(length).ToArray());
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="login">Login</param>
        /// <param name="password">Password</param>
        /// <returns>Item1 - успех операции, Item2 - сообщение об ошибке или результат</returns>
        public Tuple<bool,String> Registration(String email, String login, String password)
        {

            var resultEmail = IsValidEmail(email);
            if (!resultEmail.Item1)
            {
                return resultEmail;
            }
            var resultPassword = IsValidPassword(password);
            if (!resultPassword.Item1)
            {
                return resultPassword;
            }
            
            try
            {
                using (chatContext = new ChatEntities())
                {
                    if(chatContext.Users.Any(x => x.Email == email))
                    {
                        return new Tuple<bool, string>(false, "User with this email is already exist");
                    }
                    if (chatContext.Users.Any(x => x.Login == login))
                    {
                        return new Tuple<bool, string>(false, "User with this login is already exist");
                    }
                    var passKeyAndSalt =PasswordCrypt.GetHashFromPassword(password);
                    User user = new User()
                    {
                        PasswordHash = passKeyAndSalt.Item1,
                        PasswordSalt=passKeyAndSalt.Item2,
                        Name=login,
                        Email=email
                    };
                    chatContext.Users.Add(user);
                    if(chatContext.SaveChanges()!=1)
                    {
                        return new Tuple<bool, string>(false, "Error! Internal service Error");
                    }

                    var token = GenerateToken(user);
                    chatContext.AccessTokens.Add(token);
                    int res = chatContext.SaveChanges();
                    if (res == 1)
                    {
                        return new Tuple<bool, string>(true, token.Token);
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "Error! Internal service Error");
                    }
                }
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, ex.Message);
            }
        }

        public void Dispose()
        {
            chatContext?.Dispose();
        }
    }
}
