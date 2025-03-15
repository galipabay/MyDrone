namespace MyDrone.Web.App.Models
{
    public class DeviceViewModel
    {
        public int Id { get; set; }  // Cihazın ID'si
        public string Name { get; set; }  // Cihazın adı
        public string Category { get; set; }  // Cihazın kategorisi
        public decimal Price { get; set; }  // Cihazın fiyatı
        public int Stock { get; set; }  // Cihazın stok miktarı
    }

}
