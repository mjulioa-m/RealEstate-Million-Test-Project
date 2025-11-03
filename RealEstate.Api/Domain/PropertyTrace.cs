namespace RealEstate.Api.Domain
{
    public class PropertyTrace
    {
        public DateTime DateSale { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
    }
}