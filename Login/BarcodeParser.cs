using System;
using System.Text.RegularExpressions;

namespace Login
{
    /// <summary>
    /// Result structure produced by the BarcodeParser.
    /// </summary>
    public class BarcodeParseResult
    {
        public bool IsSuccess { get; }
        public int Quantity { get; }
        public string Sku { get; }
        public string ErrorMessage { get; }
        public string RawInput { get; }

        private BarcodeParseResult(bool success, int quantity, string sku, string errorMessage, string rawInput)
        {
            IsSuccess = success;
            Quantity = quantity;
            Sku = sku;
            ErrorMessage = errorMessage;
            RawInput = rawInput;
        }

        public static BarcodeParseResult Success(int quantity, string sku, string rawInput)
        {
            return new BarcodeParseResult(true, quantity, sku, string.Empty, rawInput);
        }

        public static BarcodeParseResult Failure(string errorMessage, string rawInput)
        {
            return new BarcodeParseResult(false, 0, string.Empty, errorMessage, rawInput);
        }
    }

    /// <summary>
    /// Parses barcode and SKU strings entered at checkout.
    /// Supports multiplier syntax such as:
    /// - "3*UM-007" (3 units of UM-007)
    /// - "5 * UM-004" (5 units of UM-004)
    /// - "2xUM-001" (2 units of UM-001)
    /// - "UM-007" (1 unit of UM-007 default)
    /// </summary>
    public class BarcodeParser
    {
        private static readonly Regex MultiplierPattern = new Regex(@"^\s*(\d+)\s*[\*xX]\s*(.+)$", RegexOptions.Compiled);

        public static BarcodeParseResult Parse(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return BarcodeParseResult.Failure("Barcode or SKU cannot be empty.", input ?? string.Empty);
            }

            string cleanInput = input.Trim();

            // Check if input matches multiplier pattern (e.g. 3*UM-007 or 3xUM-007)
            Match match = MultiplierPattern.Match(cleanInput);
            if (match.Success)
            {
                string qtyStr = match.Groups[1].Value;
                string skuStr = match.Groups[2].Value.Trim().ToUpperInvariant();

                if (!int.TryParse(qtyStr, out int qty) || qty <= 0)
                {
                    return BarcodeParseResult.Failure("Multiplier quantity must be a positive number greater than 0.", cleanInput);
                }

                if (string.IsNullOrWhiteSpace(skuStr))
                {
                    return BarcodeParseResult.Failure("Product SKU cannot be blank after multiplier.", cleanInput);
                }

                return BarcodeParseResult.Success(qty, skuStr, cleanInput);
            }

            // Standard single-item barcode/SKU without multiplier
            string normalizedSku = cleanInput.ToUpperInvariant();
            return BarcodeParseResult.Success(1, normalizedSku, cleanInput);
        }

        public static bool TryParse(string? input, out int quantity, out string sku)
        {
            var res = Parse(input);
            if (res.IsSuccess)
            {
                quantity = res.Quantity;
                sku = res.Sku;
                return true;
            }

            quantity = 0;
            sku = string.Empty;
            return false;
        }
    }
}

