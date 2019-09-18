using System;

namespace ActinUranium.Web.Helpers
{
    public static class ActinUraniumInfo
    {
        public const short YearOfDiscovery = 1935;

        private static readonly Random Random = new Random();

        public static DateTime NextDate()
        {
            int year = NextYear();
            int month = Random.Next(1, 12 + 1);
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int day = Random.Next(1, daysInMonth + 1);
            return new DateTime(year, month, day);
        }

        private static int NextYear()
        {
            int minValue = YearOfDiscovery;
            int exclusiveMaxValue = DateTime.Today.Year + 1;
            return Random.Next(minValue, exclusiveMaxValue);
        }
    }
}
