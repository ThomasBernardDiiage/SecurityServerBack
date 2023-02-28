namespace API.DataAccess.Initializer
{
    public static class DataMocker
    {
        public static IEnumerable<string> ApplicationNames()
        {
            yield return "Name1";
            yield return "Name2";
            yield return "Name3";
        }

        public static IEnumerable<string> ClaimTypes()
        {
            yield return "type1";
            yield return "type2";
            yield return "type3";
        }

        public static IEnumerable<string> ClaimValues()
        {
            yield return "value1";
            yield return "value2";
            yield return "value3";
        }
    }
}
