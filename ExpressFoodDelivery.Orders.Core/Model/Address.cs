namespace ExpressFoodDelivery.Orders.Core.Model
{
    public class Address
    {
        public string AddressLine1 { get; }

        public string AddressLine2 { get; }

        public string City { get; }

        public string Postcode { get; }

        public string Country { get; }

        public Address(string addressLine1, string addressLine2, string city, string postcode, string country)
        {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            Postcode = postcode;
            Country = country;
        }
    }
}
