// chatService.js

import ChatUI from './chatUI.js'; // Doğru yolu kontrol edin

class ChatService {
    static existingUserIds = new Set(); // Kullanıcı ID'lerini saklamak için bir Set oluşturuyoruz

    static async loadChatList() {
        const currentUserId = document.body.dataset.userId;
        const chatListContainer = document.getElementById('chatList');
        chatListContainer.innerHTML = '<div class="p-4 text-center text-gray-500">Yükleniyor...</div>';

        try {
            const response = await fetch(`/Messages/GetChatList?userId=${currentUserId}`);
            if (!response.ok) throw new Error("Sunucu hatası");
            const users = await response.json();

            chatListContainer.innerHTML = '';
            this.existingUserIds.clear(); // Her yüklemede temizle

            if (!users.length) {
                chatListContainer.innerHTML = '<div class="p-4 text-center text-gray-500">Henüz sohbet yok</div>';
                return;
            }

            users.forEach(user => {
                if (this.existingUserIds.has(user.id)) return;

                const userElement = document.createElement('div');
                //userElement.className = 'p-3 hover:bg-gray-100 cursor-pointer flex items-center border-b';
                userElement.className = 'group relative p-3 hover:bg-gray-100 cursor-pointer flex justify-center border-b';

                userElement.innerHTML = `
                <div class="flex flex-col items-center">
                <img src="${user.image}" class="w-10 h-10 rounded-full">
                    
                    <div class="absolute bottom-0 left-1/2 transform -translate-x-1/2 translate-y-full
                        opacity-0 group-hover:opacity-100 transition-opacity duration-300
                        bg-gradient-to-r from-blue-500 to-cyan-400 text-white text-xs
                        px-3 py-1 rounded-lg mt-1 shadow-lg shadow-blue-500/50 backdrop-blur-sm
                        whitespace-nowrap z-10 pulse-glow">
                    ${user.name} ${user.surname}
                    </div>
                </div>
                `;
                userElement.addEventListener('click', () => ChatUI.startChat(user));
                chatListContainer.appendChild(userElement);
                this.existingUserIds.add(user.id);
            });
        } catch (error) {
            console.error("Hata:", error);
            chatListContainer.innerHTML = '<div class="p-4 text-center text-red-500">Yükleme hatası</div>';
        }
    }

    static async sendMessage(receiverId, content) {
        const messageInput = document.getElementById('messageInput');
        const message = messageInput.value.trim();
        const currenUserId = document.body.dataset.userId;


        if (!message) return;

        const params = new URLSearchParams();
        params.append("receiverId", receiverId);
        params.append("content", message);

        try {
            const response = await fetch('/Messages/SendMessage', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'RequestVerificationToken': ChatService.getRequestToken()
                },
                body: params.toString()
            });

            if (!response.ok) throw new Error("Sunucu hatası");

            await ChatUI.loadMessageHistory(receiverId);

            messageInput.value = '';
        } catch (error) {
            console.error("Mesaj gönderimi sırasında hata oluştu:", error);
        }
    }

    static getRequestToken() {
        const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
        if (!tokenInput) {
            alert("Güvenlik doğrulama hatası. Lütfen sayfayı yenileyin.");
            return '';
        }
        return tokenInput.value;
    }
}

export default ChatService;
