﻿using System.Diagnostics.CodeAnalysis;

namespace ActinUranium.Web
{
    [SuppressMessage(
        "Design",
        "CA1052:Static holder types should be Static or NotInheritable",
        Justification = "Not supported by the framework")]
    public class SharedResources
    {
        public const string About = nameof(About);
        public const string Awards = nameof(Awards);
        public const string AwardsMetaDescription = nameof(AwardsMetaDescription);
        public const string AwardsMetaKeywords = nameof(AwardsMetaKeywords);
        public const string Customers = nameof(Customers);
        public const string Headlines = nameof(Headlines);
        public const string Home = nameof(Home);
        public const string Projects = nameof(Projects);
        public const string SourceCode = nameof(SourceCode);
        public const string TermsAndConditions = nameof(TermsAndConditions);
        public const string Privacy = nameof(Privacy);
        public const string Imprint = nameof(Imprint);
    }
}