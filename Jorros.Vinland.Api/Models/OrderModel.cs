namespace Jorros.Vinland.Api.Models
{
    public class OrderModel
    {
        public int Id { get; set; }

        public string User { get; set; }

        public int BottlesAmount { get; set; }

        public bool Confirmed { get; set; }
    }
}