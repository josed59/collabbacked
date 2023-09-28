using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace collabBackend.Filters
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredIfMultipleAttribute : ValidationAttribute
    {
        private string[] OtherProperties { get; }
        private object TargetValue { get; }

        public RequiredIfMultipleAttribute(string[] otherProperties, object targetValue)
        {
            OtherProperties = otherProperties;
            TargetValue = targetValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            foreach (var otherProperty in OtherProperties)
            {
                PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(otherProperty);
                if (otherPropertyInfo != null)
                {
                    object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);

                    if (otherPropertyValue != null && otherPropertyValue.Equals(TargetValue))
                    {
                        if (value == null)
                        {
                            return new ValidationResult(ErrorMessage);
                        }
                    }
                }
            }

            return ValidationResult.Success;
        }
    }

}
