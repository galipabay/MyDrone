using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDrone.Kernel.Services;
using MyDrone.Kernel.UnitOfWork;
using MyDrone.Types;
using MyDrone.Web.App.Models;
using System.Security.Claims;

namespace MyDrone.Web.App.Controllers
{
    public class MessagesController : BaseController
    {
        private readonly IMessageService _messageService;
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;

        public MessagesController(IMessageService messageService, INotificationService notificationService, IUnitOfWork unitOfWork, AppDbContext context)
            : base(unitOfWork, notificationService, context)
        {
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// Kullanıcıların mesajlaşma geçmişini almak için kullanılır.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserMessages()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID claim is missing.");

            var userId = int.Parse(userIdClaim);
            var messages = await _messageService.GetUserMessagesAsync(userId);
            return PartialView("_MessageDropdown", messages);
        }

        /// <summary>
        /// Kullanıcıların mesajlaşma geçmişini almak için kullanılır.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="receiverId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMessageHistory(int userId, int receiverId)
        {
            var messages = await _context.Messages
                .Where(m => (m.SenderId == userId && m.ReceiverId == receiverId) ||
                            (m.SenderId == receiverId && m.ReceiverId == userId))
                .OrderBy(m => m.SentDate)
                .Select(m => new MessageDto
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Content = m.Content,
                    SentDate = m.SentDate,
                    IsRead = m.IsRead,
                    SenderName = _context.Users.FirstOrDefault(u => u.Id == m.SenderId).Name,
                    ReceiverName = _context.Users.FirstOrDefault(u => u.Id == m.ReceiverId).Name
                })
                .ToListAsync();

            foreach (var message in messages)
            {
                if (!message.IsRead && message.ReceiverId == userId)
                {
                    await _messageService.MarkAsReadAsync(message.Id);
                }
            }
            return Json(messages);  // Burada PartialView yerine Json döndürüyorsun.
            //return PartialView("_MessageDropdown", messages);
        }

        /// <summary>
        /// Kullanıcıların mesajlaşma geçmişini almak için kullanılır.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetChatList(int userId)
        {
            var chatUserIds = await _context.Messages
                .Where(m => m.ReceiverId == userId || m.SenderId == userId)
                .Select(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
                .Distinct()
                .ToListAsync();

            var users = await _context.Users
                .Where(u => chatUserIds.Contains(u.Id))
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Surname,
                    u.MailAddress,
                    u.Image
                })
                .ToListAsync();

            return Json(users);
        }

        /// <summary>
        /// Mesaj gönderme işlemi
        /// </summary>
        /// <param name="receiverId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SendMessage(int receiverId, string content)
        {
            if (string.IsNullOrEmpty(content))
                return BadRequest("Mesaj içeriği boş olamaz.");
            try
            {
                var senderIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(senderIdClaim))
                    return Unauthorized("Kullanici giris yapmalidir.");

                int senderId = int.Parse(senderIdClaim);
                // Mesaj gönderme metodunu çağır
                await _messageService.SendMessageAsync(senderId, receiverId, content);

                return Ok(new
                {
                    Content = content,
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    SentDate = DateTime.UtcNow.ToUniversalTime(),
                    CreatedDate = DateTime.Now,
                    IsRead = false
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Mesaj gönderilemedi: {ex.Message}");
            }
        }

        /// <summary>
        /// Kullanıcıların mesajlaşma geçmişini almak için kullanılır.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IActionResult Chat(int userId)
        {
            var messages = _messageService.GetUserMessagesAsync(userId);
            return View(messages);
        }
    }
}
