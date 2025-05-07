namespace MyDrone.Kernel.Models
{
    public class Message : BaseEntity
    {

        private int senderId;

        public int SenderId
        {
            get { return senderId; }
            set { senderId = value; }
        }

        private int receiverId;

        public int ReceiverId
        {
            get { return receiverId; }
            set { receiverId = value; }
        }

        private string content;

        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        private DateTime sentDate;

        public DateTime SentDate
        {
            get { return sentDate; }
            set { sentDate = value; }
        }

        private bool isRead;

        public bool IsRead
        {
            get { return isRead; }
            set { isRead = value; }
        }

    }
}
