using System.Diagnostics.CodeAnalysis;

namespace ActinUranium.Web
{
    [SuppressMessage(
        "Design",
        "CA1052:Static holder types should be Static or NotInheritable",
        Justification = "Not supported by the framework")]
    public class SharedResources
    {
        public const string Creations = nameof(Creations);
        public const string CreationsIcon = "Creations icon";
        public const string Customers = nameof(Customers);
        public const string CustomersIcon = "Customers icon";
        public const string GitHubIcon = "GitHub icon";
        public const string Headlines = nameof(Headlines);
        public const string HeadlinesIcon = "Headlines icon";
        public const string Home = nameof(Home);
        public const string HomeIcon = "Home icon";
        public const string Imprint = nameof(Imprint);
        public const string LoyalCustomers = "Loyal customers";
        public const string Privacy = nameof(Privacy);
        public const string RecentHeadlines = "Recent headlines";
        public const string RecentlyReleasedCreations = "Recently released creations";
        public const string SourceCode = "Source code";
        public const string Terms = nameof(Terms);
        public const string TermsAndConditions = "Terms and conditions";
        public const string ViewAllCreations = "View all creations";
        public const string ViewAllHeadlines = "View all headlines";
    }
}
