﻿using BCrypt.Net;

namespace OfficeMonitor.MiddleWares
{
    public static class PasswordHasher
    {
        public static string Generate(string password) =>
            BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        public static bool Verify(string password, string hashedPassword) =>
            BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}
