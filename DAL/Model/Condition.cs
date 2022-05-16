using System;

namespace DAL.Model
{
    public class Condition
    {
        public string PropertyName { get; }

        public string DisplayValue { get; }
        public Predicate<object> Predicate { get; set; }

        public Condition(string propertyName, string conditionValue, Func<string, Predicate<object>> predicate)
        {
            PropertyName = propertyName;
            DisplayValue = $"{propertyName}={conditionValue}";
            Predicate = predicate(conditionValue);
        }
    }
}
