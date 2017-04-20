namespace AzureSearchDemo.Models
{
    public class RealEstate
    {
        public string ListingId { get; set; }
        public int Beds { get; set; }
        public int Bads { get; set; }
        public string Description { get; set; }
        public string Description_nl { get; set; }
        public string City { get; set; }
        public string Sqft { get; set; }


        public override string ToString()
        {
            return $"ListingId:{ListingId}\r\n" +
                   $"Description:{Description}\r\n" +
                   $"Description NL:{Description_nl}\r\n" +
                   $"No of beds:{Beds}\r\n" +
                   $"City: {City}\r\n" +
                   $"Sqft: {Sqft}";
        }
    }
}