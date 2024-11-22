using System.ComponentModel.DataAnnotations;

namespace BogsyFinalFinal.Models
{
    public class Videos
    {

        [Key]
        public int VideoID { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [Required]
        [StringLength(3)]
        public string Category { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
        public int Quantity { get; set; }

        [Required]
        [Range(1, 3, ErrorMessage = "MaxDays must be between 1 and 3")]
        public int MaxDays { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "RentalPrice cannot be negative")]
        public decimal RentalPrice { get; set; }
    }
}
