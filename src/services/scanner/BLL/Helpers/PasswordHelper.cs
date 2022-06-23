using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace BLL.Models.Helpers;

public class PasswordHelper
{
    public string HashPassword(string password, byte[] salt = null, bool needsOnlyHash = false)
    {
        if (salt == null || salt.Length != 16)
        {
            salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
        }

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        if (needsOnlyHash) return hashed;
        
        return $"{hashed}:{Convert.ToBase64String(salt)}";
    }

    public bool VerifyPassword(string hashedPasswordWithSalt, string passwordToCheck)
    {
        var passwordAndHash = hashedPasswordWithSalt.Split(':');
        if (passwordAndHash == null || passwordAndHash.Length != 2)
            return false;
        var salt = Convert.FromBase64String(passwordAndHash[1]);
        if (salt == null)
            return false;

        var hashOfpasswordToCheck = HashPassword(passwordToCheck, salt, true);

        if (String.Compare(passwordAndHash[0], hashOfpasswordToCheck) == 0)
        {
            return true;
        }
        return false;
    }
}