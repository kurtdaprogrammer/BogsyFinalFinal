using System.ComponentModel.DataAnnotations;

namespace BogsyFinalFinal.Models
{
    public class Customers
    {
        [Key]
        [Display(Name = "Customer Name")]
        public int CustomerId { get; set; }
       
        [Required]
        [StringLength(100)]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Required]
        [Display(Name = "Customer Contact Number")]
        public string CustomerNo { get; set; }

        [Required]
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}
