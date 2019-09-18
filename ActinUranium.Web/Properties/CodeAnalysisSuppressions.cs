using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Globalization",
    "CA1308:Normalize strings to uppercase",
    Justification = "Not making security decisions based on the conversion result")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Data Transfer Objects (DTO)s",
    Scope = "namespaceanddescendants",
    Target = "ActinUranium.Web.Models")]
