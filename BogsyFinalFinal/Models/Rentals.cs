using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BogsyFinalFinal.Models
{
    public class Rentals
    {
        [Key]
        public int RentalID { get; set; }

        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public virtual Customers Customer { get; set; }

        [ForeignKey("Video")]
        public int VideoID { get; set; }
        public virtual Videos Video { get; set; }

        [Required]
        public DateTime RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; } = false;

        public int DaysRented
        {
            get
            {
                if (ReturnDate.HasValue)
                {
                    return (ReturnDate.Value - RentalDate).Days;
                }
                else
                {
                    return (DateTime.Now - RentalDate).Days;
                }
            }
        }

        public decimal? TotalRentalPrice { get; set; }

    }
}
