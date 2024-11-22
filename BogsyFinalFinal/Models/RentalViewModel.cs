namespace BogsyFinalFinal.Models
{
    public class RentalViewModel
    {
        public int RentalID { get; set; }
        public string CustomerName { get; set; }
        public string VideoTitle { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string IsReturned { get; set; }
        public decimal? TotalRentalPrice { get; set; }
    }
    

}
