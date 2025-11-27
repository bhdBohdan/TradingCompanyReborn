using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



    namespace TradingCompany.WPF2.Services
    {
        public static class DataValidators
        {
            public static string ValidateRequired(string? value, string fieldName)
            {
                if (string.IsNullOrWhiteSpace(value)) return $"{fieldName} is required.";
                return string.Empty;
            }

            public static string ValidateProductName(string? value)
                => ValidateRequired(value, "Product name");

            public static string ValidateCategory(string? value)
                => string.Empty; // optional - extend if needed

            public static string ValidatePriceString(string? priceText)
            {
                if (string.IsNullOrWhiteSpace(priceText)) return "Price is required.";
                if (!decimal.TryParse(priceText, NumberStyles.Number, CultureInfo.InvariantCulture, out var v))
                    return "Price must be a valid number.";
                if (v <= 0) return "Price must be greater than zero.";
                return string.Empty;
            }

            public static string ValidatePriceDecimal(decimal price)
            {
                if (price <= 0) return "Price must be greater than zero.";
                return string.Empty;
            }

            public static string ValidateEmail(string? email)
            {
                if (string.IsNullOrWhiteSpace(email)) return "Email is required.";
                // simple email check
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) return "Email is invalid.";
                return string.Empty;
            }

            // Add other validators as needed (Username, Password, Profile fields, etc.)
        }
    }

