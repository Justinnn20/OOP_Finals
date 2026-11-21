using System.Globalization;

namespace UMakCoop.Services
{
    /// <summary>Peso formatting and parsing in one place so every screen shows money the same way.</summary>
    public static class Money
    {
        public static string Peso(decimal value)
        {
            return "₱" + value.ToString("N2", CultureInfo.InvariantCulture);
        }

        public static bool TryParse(string text, out decimal value)
        {
            value = 0m;
            if (string.IsNullOrWhiteSpace(text)) return false;
            text = text.Replace("₱", "").Replace(",", "").Trim();
            return decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out value) && value >= 0m;
        }
    }
}
