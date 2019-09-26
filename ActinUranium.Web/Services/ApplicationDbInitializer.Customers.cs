using ActinUranium.Web.Helpers;
using ActinUranium.Web.Models;

namespace ActinUranium.Web.Services
{
    public sealed partial class ApplicationDbInitializer
    {
        private static Customer CreateCustomer(string name, string logoFileName)
        {
            string logoSource = "/img/customers/" + logoFileName;
            return new Customer
            {
                Slug = name.Slugify(),
                Name = name,
                Logo = CreateImage(logoSource)
            };
        }

        private void SeedCustomers()
        {
            const char Nbsp = UnicodeLiterals.NoBreakSpace;

            var customers = new Customer[]
            {
                CreateCustomer($"Aliquid{Nbsp}AG", "aliquid.svg"),
                CreateCustomer($"Ande Animi{Nbsp}GmbH", "ande-animi.svg"),
                CreateCustomer("Culpa Porro Commodi", "culpa-porro-comodi.svg"),
                CreateCustomer($"Delectus Ratione{Nbsp}GmbH", "delectus-ratione.svg"),
                CreateCustomer($"Dolore Odio Corrupti{Nbsp}GmbH", "dolore-odio-corrupti.svg"),
                CreateCustomer($"Facilis{Nbsp}AG", "facilis.svg"),
                CreateCustomer($"Maiores{Nbsp}AG", "maiores.svg"),
                CreateCustomer("Nostrum Tenetur Voluptas", "nostrum-tenetur-voluptas.svg"),
                CreateCustomer($"Rem Vel{Nbsp}AG", "rem-vel.svg"),
                CreateCustomer($"Sapiente Numquam{Nbsp}GmbH", "sapiente-numquam.svg"),
                CreateCustomer($"Tempora{Nbsp}GmbH", "tempora.svg"),
                CreateCustomer("Vero Harum Veniam Nemo", "vero-harum-veniam-nemo.svg")
            };

            _dbContext.Customers.AddRange(customers);
            _dbContext.SaveChanges();
        }
    }
}
