using System;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is DateTime dateValue)
            {
                if (dateValue <= DateTime.Now)
                {
                    return new ValidationResult(ErrorMessage ?? "Tarih gelecekte bir zaman olmalıdır.");
                }

                return ValidationResult.Success;
            }

            return new ValidationResult("Geçersiz tarih formatı.");
        }
    }
}