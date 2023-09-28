using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

[AttributeUsage(AttributeTargets.Property)]
public class RequiredIfAttribute : ValidationAttribute
{
    private string OtherProperty { get; }
    private object TargetValue { get; }

    public RequiredIfAttribute(string otherProperty, object targetValue)
    {
        OtherProperty = otherProperty;
        TargetValue = targetValue;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
        if (otherPropertyInfo != null)
        {
            object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);
              if (otherPropertyValue != null)
              {
                if (TargetValue == null)
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
