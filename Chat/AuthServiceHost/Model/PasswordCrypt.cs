using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthServiceProvider.Model
{
    class PasswordCrypt
    {
        public static Tuple<byte[], byte[]> GetHashFromPassword(String password, int keyLength=32, int saltLength = 20)
        {
            // specify that we want to randomly generate a 20-byte salt
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltLength))
            {
                byte[] salt = deriveBytes.Salt;
                byte[] key = deriveBytes.GetBytes(keyLength);
                return new Tuple<byte[], byte[]>(key, salt);
            }
        }
        public static bool ComparePasswords(String userPass, byte[] passInDb, byte[] salt)
        {
            // load salt and key from database
            using (var deriveBytes = new Rfc2898DeriveBytes(userPass, salt))
            {
                byte[] newKey = deriveBytes.GetBytes(passInDb.Length);  // derive a key
                return newKey.SequenceEqual(passInDb);
            }
        }

    }
}
