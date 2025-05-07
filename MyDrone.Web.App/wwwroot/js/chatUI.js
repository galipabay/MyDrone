// chatUI.js

import ChatService from './chatService.js'; // veya doğru path neyse
class ChatUI {
    static init() {
        console.log("ChatUI initialized"); // Bu satırı ekleyin
        document.getElementById("chatToggle").addEventListener("click", ChatUI.toggleChatPanel);
        document.getElementById("closeChat").addEventListener("click", ChatUI.closeChatPanel);
        document.getElementById("sendMessageButton").addEventListener("click", ChatUI.sendMessage);
        document.getElementById("userSearch").addEventListener("input", ChatUI.handleUserSearch);
        document.getElementById("messageInput").addEventListener("keypress", ChatUI.handleKeyPress);
    }

    //Chat panelini aç/kapa
    static toggleChatPanel() {
        const chatPanel = document.getElementById("chatPanel");

        if (!chatPanel) {
            console.warn("Chat panel bulunamadı. Dom'da kontrol et id kontrol et #chatPanel");
            return;
        }
        try {
            chatPanel.classList.toggle("hidden");
            const ishidden = chatPanel.classList.contains("hidden");
            console.log("Chat panelinin hidden sınıfı:", ishidden);

            if (!ishidden) {

                //Acilis animasyonu.
                setTimeout(() => {
                    chatPanel.classList.remove("translate-y-full");
                    chatPanel.classList.remove("translate-y-0");
                }, 10);//Repaint bekletmesi.
                console.log("Chat paneli açık. Sohbet listesi yukleniyor.");
                ChatService.loadChatList()
                    .catch(error => {
                        console.log("Sohbet listesi yüklenirken hata oluştu:", error);
                    });
            } else {
                // Kapanış animasyonu
                chatPanel.classList.remove("translate-y-0");
                chatPanel.classList.add("translate-y-full");
                // Animasyon bitince tamamen gizle
                setTimeout(() => {
                    chatPanel.classList.add("hidden");
                }, 300);
                console.log("Chat paneli kapalı. Sohbet listesi yüklenmeyecek.");
            }

        } catch (error) {
            console.error("Chat panelini açarken hata oluştu:", error);
        }
    }

    static closeChatPanel() {
        const chatPanel = document.getElementById("chatPanel");
        chatPanel.classList.add("hidden");
    }

    static sendMessage() {
        const activeChat = document.getElementById('activeChat');
        const receiverId = activeChat.getAttribute('data-user-id');
        const messageInput = document.getElementById('messageInput');

        if (receiverId && messageInput.value.trim()) {
            ChatService.sendMessage(receiverId, messageInput.value);
        }
    }

    static handleUserSearch(event) {
        const query = event.target.value;

        if (query.length < 2) {
            document.getElementById('searchResults').classList.add('hidden');
            return;
        }

        fetch(`/Search/SearchUser?query=${encodeURIComponent(query)}`)
            .then(response => response.json())
            .then(users => {
                const resultsContainer = document.getElementById('searchResults');
                resultsContainer.innerHTML = '';

                if (users.length > 0) {
                    users.forEach(user => {
                        const li = document.createElement('li');
                        li.setAttribute('data-user-id', user.id);
                        li.classList.add('cursor-pointer', 'hover:bg-indigo-100', 'px-3', 'py-2', 'flex', 'items-center');
                        li.classList.add('user-item');

                        const profileImg = document.createElement('img');
                        profileImg.src = user.image;
                        profileImg.alt = `${user.name} ${user.surname}`;
                        profileImg.classList.add('w-10', 'h-10', 'rounded-full', 'mr-3');

                        const userName = document.createElement('div');
                        userName.textContent = `${user.name} ${user.surname}`;
                        userName.classList.add('font-medium', 'text-gray-800');

                        li.appendChild(profileImg);
                        li.appendChild(userName);

                        li.addEventListener('click', () => ChatUI.startChat(user));

                        resultsContainer.appendChild(li);
                    });
                    resultsContainer.classList.remove('hidden');
                } else {
                    resultsContainer.classList.add('hidden');
                }
            })
            .catch(error => console.error("Arama sırasında hata oluştu:", error));
    }

    static handleKeyPress(event) {
        if (event.key === 'Enter') {
            event.preventDefault();
            ChatUI.sendMessage();
        }
    }

    static startChat(user) {

        console.log("Sohbet başlatılıyor:", user);
        document.getElementById('searchResults').classList.add('hidden');
        document.getElementById('userSearch').value = '';

        document.getElementById('activeChatUserName').textContent = `${user.name} ${user.surname}`;
        document.getElementById('activeChatUserImage').src = user.image;

        document.getElementById('activeChatContainer').classList.remove('hidden');

        ChatUI.loadMessageHistory(user.id);
    }

    static async loadMessageHistory(userId) {
        const currentUserId = document.body.dataset.userId;
        const activeChat = document.getElementById('activeChat');
        activeChat.innerHTML = '<div class="p-4 text-center text-gray-500">Yükleniyor...</div>';

        try {
            const response = await fetch(`/Messages/GetMessageHistory?userId=${currentUserId}&receiverId=${userId}`);
            if (!response.ok) throw new Error("Sunucu hatası");
            const messages = await response.json();

            activeChat.innerHTML = '';

            messages.forEach(msg => {
                ChatUI.displayMessage(msg, msg.senderId == currentUserId);
            });

            activeChat.scrollTop = activeChat.scrollHeight;
            activeChat.setAttribute('data-user-id', userId);
        } catch (error) {
            console.error("Hata:", error);
            activeChat.innerHTML = '<div class="p-4 text-center text-red-500">Yükleme hatası</div>';
        }
    }

    static displayMessage(message, isOwnMessage) {
        const activeChat = document.getElementById('activeChat');

        const wrapper = document.createElement('div');
        wrapper.className = `flex ${isOwnMessage ? 'justify-end' : 'justify-start'}`;

        const msgBox = document.createElement('div');
        msgBox.className = `${isOwnMessage ? 'bg-blue-500 text-white' : 'bg-white'} rounded-lg p-3 max-w-xs shadow relative`;

        msgBox.textContent = message.content;

        // 🕒 Zaman etiketi
        const timestamp = document.createElement('div');
        timestamp.className = 'text-[10px] text-gray-400 mt-1 text-right';

        const utcDate = new Date(message.sentDate + "Z");  // Veritabanından gelen UTC tarih
        const userTimeZone = Intl.DateTimeFormat().resolvedOptions().timeZone; // Kullanıcının saat dilimi
        const localTime = utcDate.toLocaleString('tr-TR', { timeZone: userTimeZone });
        timestamp.textContent = localTime;

        console.log("UTC tarihi:", utcDate);

        console.log("Yerel saat:", localTime);


        msgBox.appendChild(timestamp);
        wrapper.appendChild(msgBox);
        activeChat.appendChild(wrapper);
    }
}
export default ChatUI;
