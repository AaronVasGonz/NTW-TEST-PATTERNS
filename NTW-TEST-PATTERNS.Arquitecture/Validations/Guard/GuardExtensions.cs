using Ardalis.GuardClauses;
using System.Text.RegularExpressions;

namespace Arquitecture.Validations.Guard
{
    public static class GuardExtensions
    {
        public static void InvalidEmail(this IGuardClause guardClause, string email, string parameterName)
        {
            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new ArgumentException($"Invalid email address: {email}", parameterName);
            }
        }

        public static void InvalidPassword(this IGuardClause guardClause, string password, string parameterName)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
            {
                throw new ArgumentException($"Password must be at least 8 characters long.", parameterName);
            }
        }

        public static void InvalidUsername(this IGuardClause guardClause, string username, string parameterName)
        {
            if (string.IsNullOrEmpty(username) || username.Length < 3)
            {
                throw new ArgumentException($"Username must be at least 3 characters long.", parameterName);
            }
        }
    }
}