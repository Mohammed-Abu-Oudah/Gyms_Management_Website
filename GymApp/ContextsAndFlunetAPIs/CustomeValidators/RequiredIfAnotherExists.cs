using GymApp.Data;
using System.ComponentModel.DataAnnotations;

namespace GymApp.ContextsAndFlunetAPIs.CustomeValidators
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredIfAnotherExistsAttribute : ValidationAttribute
    {
        private string OtherPropertyName { get; }

        public RequiredIfAnotherExistsAttribute(string otherPropertyName)
        {
            OtherPropertyName = otherPropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var otherPropertyObject = validationContext.ObjectType.GetProperty(OtherPropertyName);

            if (otherPropertyObject == null)
            {
                throw new ArgumentException($"Property with the name {OtherPropertyName} wasn't found");
            }

            var otherPropertyValue = otherPropertyObject.GetValue(validationContext.ObjectInstance);

            if (!(otherPropertyValue?.Equals(UserTypes.SuperAdmin) == true))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
