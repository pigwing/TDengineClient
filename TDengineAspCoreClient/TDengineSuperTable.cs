namespace TDengineAspCoreClient
{
    public class TDengineSuperTable
    {
        public static string GetTableName(long unitId, string alias)
        {
            return string.Concat(unitId, "-", alias);
        }
    }
}
