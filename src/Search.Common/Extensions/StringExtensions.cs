using System;

namespace Search.Common.Extensions
{
    public static class StringExtensions
    {
        public static int AfterIndexOf(this string input, string value, StringComparison stringComparison)
        {
            return input.AfterIndexOf(value, 0, stringComparison);
        }

        public static int AfterIndexOf(this string input, string value)
        {
            return input.AfterIndexOf(value, 0, StringComparison.InvariantCulture);
        }

        public static int AfterIndexOf(this string input, string value, int startIndex, StringComparison stringComparison)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var result = input.IndexOf(value, startIndex, stringComparison);
            if (result == -1)
            {
                return -1;
            }

            return result + value.Length;
        }

        public static int AfterIndexOf(this string input, string value, int startIndex)
        {
            return input.AfterIndexOf(value, startIndex, StringComparison.InvariantCulture);
        }

        public static string Copy(this string input, int startIndex, int endIndex)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (endIndex >= input.Length)
            {
                endIndex = input.Length;
            }
            return input[startIndex..endIndex];
        }

        public static string Overflow(this string input, int length, string? overflow = "...")
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (overflow.Length > length)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Should be not less than overflow string length.");
            }

            if (input.Length > length)
            {
                length = length - overflow.Length;
                return input.Substring(0, length) + overflow;
            }

            return input;
        }
    }
}