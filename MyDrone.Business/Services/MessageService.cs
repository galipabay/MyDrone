using Microsoft.EntityFrameworkCore;
using MyDrone.Kernel.Models;
using MyDrone.Kernel.Services;
using MyDrone.Types;

namespace MyDrone.Business.Services
{
    public class MessageService : IMessageService
    {
        private readonly AppDbContext _context;
        private readonly INotificationService _notificationService;

        public MessageService(AppDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        /// <summary>
        /// Sends a message from one user to another and creates a notification for the receiver.
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="receiverId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(int senderId, int receiverId, string content)
        {
            var message = new Message()
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                SentDate = DateTime.UtcNow.ToUniversalTime(),
                CreatedDate = DateTime.Now,
                IsRead = false
            };

            await _context.Messages.AddAsync(message);

            await _context.SaveChangesAsync();

            var senderUser = _context.Users.FirstOrDefault(u => u.Id == senderId);
            var senderName = senderUser != null ? $"{senderUser.Name} {senderUser.Surname}" : null;

            var notificationMessage = $"{senderName} size yeni bir mesaj gönderdi.";

            await _notificationService.CreateNotificationAsync(
                receiverId,
                notificationMessage,
                senderId,
                null,
                NotificationType.Message
            );
        }

        /// <summary>
        /// Retrieves a list of messages for a specific user, ordered by the sent date in descending order.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Message>> GetUserMessagesAsync(int userId)
        {
            return await _context.Messages.
                Where(m => m.ReceiverId == userId).
                OrderByDescending(m => m.SentDate).
                ToListAsync();
        }

        /// <summary>
        /// Marks a message as read by updating its IsRead property to true.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public Task MarkAsReadAsync(int messageId)
        {
            var message = _context.Messages.Find(messageId);
            if (message != null)
            {
                message.IsRead = true;
                _context.Messages.Update(message);
                return _context.SaveChangesAsync();
            }
            return Task.CompletedTask;
        }
    }
}
