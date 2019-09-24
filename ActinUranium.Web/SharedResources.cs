using System.Diagnostics.CodeAnalysis;

namespace ActinUranium.Web
{
    [SuppressMessage(
        "Design",
        "CA1052:Static holder types should be Static or NotInheritable",
        Justification = "Not supported by the framework")]
    public class SharedResources
    {
        internal const string Execution = nameof(Execution);
        internal const string Strategy = nameof(Strategy);

        public static readonly string Creations = "Creations";
        public static readonly string CreationsIcon = "Creations icon";
        public static readonly string Customers = "Customers";
        public static readonly string CustomersIcon = "Customers icon";        
        public static readonly string GitHubIcon = "GitHub icon";
        public static readonly string Headlines = "Headlines";
        public static readonly string HeadlinesIcon = "Headlines icon";
        public static readonly string Home = "Home";
        public static readonly string HomeIcon = "Home icon";
        public static readonly string Imprint = "Imprint";
        public static readonly string LoyalCustomers = "Loyal customers";
        public static readonly string Privacy = "Privacy";
        public static readonly string RecentHeadlines = "Recent headlines";
        public static readonly string RecentlyReleasedCreations = "Recently released creations";
        public static readonly string ContemporaryCreations = "Contemporary creations";
        public static readonly string RelatedHeadlines = "Related headlines";
        public static readonly string SourceCode = "Source code";        
        public static readonly string Terms = "Terms";
        public static readonly string TermsAndConditions = "Terms and conditions";
        public static readonly string ViewAllCreations = "View all creations";
        public static readonly string ViewAllHeadlines = "View all headlines";
    }
}
