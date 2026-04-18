using System;
using System.Security.Cryptography;
using System.Text;

namespace Isolation_Protocol.Services;

public static class Hasher
{
    public static string HashPassword(string password)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(password);

        byte[] hashBytes = SHA256.HashData(inputBytes);

        return Convert.ToHexString(hashBytes);
    }

    public static bool Verify(string password, string savedHash)
    {
        return HashPassword(password) == savedHash;
    }
}