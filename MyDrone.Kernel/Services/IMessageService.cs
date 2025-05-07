using MyDrone.Kernel.Models;

namespace MyDrone.Kernel.Services
{
    public interface IMessageService
    {
        Task SendMessageAsync(int senderId, int receiverId, string content);
        Task<List<Message>> GetUserMessagesAsync(int userId);
        Task MarkAsReadAsync(int messageId);
    }
}
