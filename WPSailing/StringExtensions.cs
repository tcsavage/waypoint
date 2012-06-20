using System.Text.RegularExpressions;

namespace WPSailing
{
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a string using PascalCase to Title Case.
        /// </summary>
        /// <param name="phrase">The PascalCase string to convert.</param>
        /// <returns>A Title Case string.</returns>
        public static string FromPascalCase(this string phrase)
        {
            Regex r = new Regex("([A-Z]+[a-z]+)");
            return r.Replace(phrase, m => (m.Value.Length > 3 ? m.Value : m.Value.ToLower()) + " ");
        }
    }
}
