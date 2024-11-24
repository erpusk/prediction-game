<template>
    <div>
      <!-- Sõnumite kuvamise ala -->
      <div class="chat-box">
        <div v-for="message in messages" :key="message.id" class="message">
          <strong>{{ getSenderName(message.senderId) }}</strong>: {{ message.message }}
          <small>{{ new Date(message.timestamp).toLocaleString() }}</small>
        </div>
      </div>
  
      <!-- Sisestus ja saatmise nupp -->
      <form @submit.prevent="sendMessage" class="chat-input-container">
        <input
          v-model="newMessage"
          type="text"
          class="chat-input"
          placeholder="message..."
          required
        />
        <button type="submit" class="send-button">
          <i class="fas fa-arrow-right"></i>
        </button>
      </form>
    </div>
  </template>
  
  <script setup>
import { ref, onMounted, nextTick } from "vue";

const props = defineProps({
  gameId: {
    type: Number,
    required: true,
  },
  currentUserId: {
    type: Number,
    required: true,
  },
});

// Andmed
const messages = ref([]); // Sõnumite loend
const newMessage = ref(""); // Sisendväärtus uue sõnumi jaoks
const users = ref({}); // Osalejate nimed (ID -> Nimi)
const backendBaseUrl = "http://localhost:5160";
// Sõnumite laadimine
const loadMessages = async () => {
  try {
    const response = await fetch(`${backendBaseUrl}/api/PredictionGames/${props.gameId}`);
    if (!response.ok) throw new Error(await response.text());
    const gameData = await response.json();

    messages.value = gameData.chatMessages || [];
    const participants = gameData.participants || [];

    users.value = participants.reduce((acc, user) => {
      acc[user.userId] = user.userName;
      return acc;
    }, {});

    scrollToBottom(); // Automaatne kerimine
  } catch (error) {
    console.error("Error loading messages:", error);
  }
};

// Saatja nime kuvamine
const getSenderName = (userId) => {
  return users.value[userId] || "Unknown User";
};

// Sõnumi saatmine
const sendMessage = async () => {
  if (!newMessage.value.trim()) return; // Kontrolli tühja sisendit

  const newChatMessage = {
    id: Date.now(),
    senderId: props.currentUserId,
    message: newMessage.value,
    timestamp: new Date().toISOString(),
  };

  // Lisa uus sõnum kohe massiivi
  messages.value.push(newChatMessage);

  try {
    const response = await fetch(`${backendBaseUrl}/api/Chat/${props.gameId}`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        senderId: props.currentUserId,
        message: newMessage.value,
      }),
    });

    if (!response.ok) {
      console.error("Error sending message:", await response.text());
      messages.value.pop(); // Eemalda sõnum, kui saatmine ebaõnnestub
    }
  } catch (error) {
    console.error("Error sending message:", error);
    messages.value.pop(); // Eemalda sõnum, kui on viga
  }

  newMessage.value = ""; // Tühjenda sisendväli
  scrollToBottom(); // Kerib lõpuni
};


// Automaatne kerimine viimase sõnumi juurde
const scrollToBottom = () => {
  nextTick(() => {
    const chatBox = document.querySelector(".chat-box");
    if (chatBox) {
      chatBox.scrollTop = chatBox.scrollHeight;
    }
  });
};

// Lae sõnumid, kui komponent mountitakse
onMounted(() => {
  loadMessages();
});
</script>

  
  <style>
.chat-box {
  height: 300px; /* Fikseeritud kõrgus */
  overflow-y: auto; /* Kerimine */
  border: 1px solid #ccc;
  border-radius: 8px;
  padding: 10px;
  background-color: #f8f8f8;
  margin-bottom: 20px;
}

.message {
  margin-bottom: 10px;
}

.chat-input-container {
  display: flex;
  align-items: center;
}

.chat-input {
  flex: 1;
  border: 1px solid #ccc;
  border-radius: 6px;
  padding: 10px;
  font-size: 14px;
}

.send-button {
  border: none;
  background-color: #5bb17c;
  color: white;
  padding: 10px;
  border-radius: 50%;
  cursor: pointer;
  margin-left: 8px;
  transition: background-color 0.3s ease;
}

.send-button:hover {
  background-color: #4d6bac;
}

.send-button:active {
  background-color: #3f6b96;
}

  </style>
  