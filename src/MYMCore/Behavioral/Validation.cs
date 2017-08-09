using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MYMCore.Behavioral {
    public class Validation {

        public static bool IsValidate<T>(T t, out List<ValidationResult> result) {
            var content = new ValidationContext(t);
            result = new List<ValidationResult>();
            return Validator.TryValidateObject(t, content, result, true);
        }

        public static bool IsValidateProperty<T>(T t, string field, object validateValue, out List<ValidationResult> result) {
            var content = new ValidationContext(t) {
                MemberName = field
            };
            result = new List<ValidationResult>();
            return Validator.TryValidateProperty(validateValue, content, result);
        }
    }
}
