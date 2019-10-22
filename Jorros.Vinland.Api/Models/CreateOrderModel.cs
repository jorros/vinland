using System.ComponentModel.DataAnnotations;

namespace Jorros.Vinland.Api.Models
{
    public class CreateOrderModel
    {
        [Required]
        public string User { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Only positive amount allowed")]
        [Required]
        public int BottlesAmount { get; set; }
    }
}