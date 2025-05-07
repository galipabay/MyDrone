namespace MyDrone.Kernel.Models
{
    public class Notification : BaseEntity
    {
        private int receiverUserId;

        public int ReceiverUserId
        {
            get { return receiverUserId; }
            set { receiverUserId = value; }
        }

        private int? senderUserId;

        public int? SenderUserId
        {
            get { return senderUserId; }
            set { senderUserId = value; }
        }

        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        private NotificationType type = NotificationType.General;

        public NotificationType Type
        {
            get { return type; }
            set { type = value; }
        }

        private bool isRead;

        public bool IsRead
        {
            get { return isRead; }
            set { isRead = value; }
        }

        private string? relatedUrl;

        public string? RelatedUrl
        {
            get { return relatedUrl; }
            set { relatedUrl = value; }
        }

    }

    public enum NotificationType
    {
        General,
        Message,
        Alert,
        Warning,
        Info,
        OrderUpdate
    }
}
