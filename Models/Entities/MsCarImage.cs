namespace RentCar.Web.Models.Entities
{
    public class MsCarImage
    {
        public Guid CarImageId { get; set; } = Guid.NewGuid();
        public Guid CarId { get; set; }
        public MsCar Car { get; set; } = null!;
        public string ImageLink { get; set; } = string.Empty;
    }
}
