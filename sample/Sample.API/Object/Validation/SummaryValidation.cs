using System;
using System.ComponentModel.DataAnnotations;
using Sample.Process.Object;

namespace Sample.API.Object.Validation
{
    internal class SummaryValidation : ValidationAttribute
    {
        private Summary Value;

        public override bool IsValid(object value)
        {
            try
            {
                Value = Enum.Parse<Summary>((string)value);
                return Value != Summary.Freezing;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return $"I will not allow {Value} for {name}!";
        }
    }
}
