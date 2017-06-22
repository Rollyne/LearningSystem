using System;
using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Models.Attributes
{
    public class FutureDateOnlyAttribute : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return $"Only future dates are allowed at the {name} field.";
        }

        public override bool IsValid(object value)
        {
           
            DateTime d = Convert.ToDateTime(value);
            return d >= DateTime.Now;
        }

    }
}
