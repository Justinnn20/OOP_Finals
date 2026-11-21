using System;

namespace Login.Models
{
    public class BarcodeParseResult
    {
        public bool IsSuccess { get; }
        public string Barcode { get; }
        public int Quantity { get; }
        public string? ErrorMessage { get; }

        private BarcodeParseResult(bool success, string barcode, int quantity, string? errorMessage)
        {
            IsSuccess = success;
            Barcode = barcode;
            Quantity = quantity;
            ErrorMessage = errorMessage;
        }

        public static BarcodeParseResult Success(string barcode, int quantity) =>
            new(true, barcode, quantity, null);

        public static BarcodeParseResult Failure(string errorMessage) =>
            new(false, string.Empty, 0, errorMessage);
    }

    public class BarcodeParser
    {
        public BarcodeParseResult Parse(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return BarcodeParseResult.Failure("Please enter a barcode or SKU.");
            }

            string trimmed = input.Trim();

            // Multiplier pattern e.g. "3*UM-007" or "UM-007*3"
            if (trimmed.Contains('*'))
            {
                string[] parts = trimmed.Split('*');
                if (parts.Length != 2)
                {
                    return BarcodeParseResult.Failure("Invalid multiplier syntax. Use {qty}*{barcode} (e.g. 3*UM-007).");
                }

                string part1 = parts[0].Trim();
                string part2 = parts[1].Trim();

                // Format: 3*UM-007
                if (int.TryParse(part1, out int qty1))
                {
                    if (qty1 <= 0)
                        return BarcodeParseResult.Failure("Quantity must be at least 1.");
                    if (string.IsNullOrWhiteSpace(part2))
                        return BarcodeParseResult.Failure("Barcode / SKU cannot be blank.");

                    return BarcodeParseResult.Success(part2, qty1);
                }
                // Format: UM-007*3
                else if (int.TryParse(part2, out int qty2))
                {
                    if (qty2 <= 0)
                        return BarcodeParseResult.Failure("Quantity must be at least 1.");
                    if (string.IsNullOrWhiteSpace(part1))
                        return BarcodeParseResult.Failure("Barcode / SKU cannot be blank.");

                    return BarcodeParseResult.Success(part1, qty2);
                }
                else
                {
                    return BarcodeParseResult.Failure("Multiplier format error. Expected a numeric quantity (e.g. 3*UM-007).");
                }
            }

            // Standard barcode/SKU without multiplier (default qty = 1)
            return BarcodeParseResult.Success(trimmed, 1);
        }
    }
}

