namespace ActinUranium.Web.Helpers
{
    /// <summary>
    /// Collection of SQL Server's data type names (Transact-SQL).
    /// </summary>
    /// <remarks>For class name in the plural, see <a href=""></a></remarks>
    /// <seealso cref="Microsoft.Net.Http.Headers.HeaderNames"/>
    /// <seealso href="https://stackoverflow.com/questions/7961282/naming-convention-for-class-of-constants-in-c-plural-or-singular">
    /// Class Naming in the Plural</seealso>
    public static class TransactSqlTypeNames
    {
        /// <summary>
        /// Date taking 3 bytes of storage.
        /// </summary>
        /// <seealso href="https://docs.microsoft.com/en-us/sql/t-sql/data-types/date-transact-sql">date (Transact-SQL)</seealso>
        public const string Date = "date";
    }
}
