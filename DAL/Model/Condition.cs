namespace DAL.Model
{
    public class Condition
    {
        public string PropertyName { get; }

        public string Value { get; }

        public string DisplayValue { get; }

        public Condition(string propertyName, string conditionValue)
        {
            PropertyName = propertyName;
            Value = conditionValue;
            DisplayValue = $"{propertyName}={conditionValue}";
        }
    }
}
