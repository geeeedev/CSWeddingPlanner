using System;
using System.ComponentModel.DataAnnotations;


// IMPT: Don't forget to wrap it in our namespace
// so our model can reference this atop: using CSWeddingPlanner.Validations;
namespace CSWeddingPlanner.Validations          
{
    public class FutureDateOnlyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((DateTime)value < DateTime.Now)
                return new ValidationResult("...Date must be in the FUTURE.");
            return ValidationResult.Success;
        }
    }

}