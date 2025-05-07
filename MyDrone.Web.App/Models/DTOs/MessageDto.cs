namespace MyDrone.Web.App.Models
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime SentDate { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; } // Bunu User tablosundan çekeceğiz
        public int ReceiverId { get; set; }
        public string ReceiverName { get; set; } // İstersen bu da olur
        public bool IsRead { get; set; } // Okundu mu okunmadı mı
        public byte[] Image { get; set; }
    }

}
