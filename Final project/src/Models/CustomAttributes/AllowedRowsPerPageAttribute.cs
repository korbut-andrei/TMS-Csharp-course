using System.ComponentModel.DataAnnotations;

namespace AndreiKorbut.CareerChoiceBackend.Models.CustomAttributes
{
    public class AllowedRowsPerPageAttribute : ValidationAttribute
    {
        private readonly HashSet<int> _allowedValues;

        public AllowedRowsPerPageAttribute(params int[] allowedValues) 
        {
            _allowedValues = new HashSet<int>(allowedValues);
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int intValue && !_allowedValues.Contains(intValue))
            {
                return new ValidationResult(ErrorMessage ?? "Invalid RowsPerPage.");
            }
            return ValidationResult.Success;
        }
    }
}
