<template>
    <div>
      <div class="chat-box">
        <!-- Kuvab sõnumid -->
        <div v-for="message in messages" :key="message.id" class="message">
  <strong>{{ getSenderName(message.senderId) }}</strong>: {{ message.message }}
  <small>{{ new Date(message.timestamp).toLocaleString() }}</small>
</div>


      </div>
  
      <!-- Sõnumi sisestus ja nupp -->
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
  onMounted(() => {
  loadMessages();
});
  const messages = ref([]);
  const newMessage = ref("");
  const users = ref({});
  const backendBaseUrl = "http://localhost:5160";
  const currentUserName = ref(localStorage.getItem("userName") || "Current User");

  const loadMessages = async () => {
  try {
    const response = await fetch(`${backendBaseUrl}/api/PredictionGames/${props.gameId}/Chat`, {
      headers: {
        "Authorization": `Bearer ${localStorage.getItem("token")}`,
        "Content-Type": "application/json",
      },
    });

    if (!response.ok) throw new Error(await response.text());

    const gameData = await response.json();

    // Lae osalejate andmed
    const participants = gameData.participants || [];
    users.value = participants.reduce((acc, user) => {
      acc[user.userId] = user.userName;
      return acc;
    }, {});

    console.log("Users loaded:", users.value); // Kontrolli osalejate andmeid
    // Lisa igale sõnumile senderName
    messages.value = gameData.chatMessages.map((msg) => ({
      ...msg,
      senderName: users.value[msg.senderId] || "Unknown User",
    }));

    console.log("Messages loaded:", messages.value); // Kontrolli sõnumite andmeid

    scrollToBottom(); // Kerib lõpuni
  } catch (error) {
    console.error("Error loading messages:", error);
  }
};


  
const getSenderName = (userId) => {
  if (userId === props.currentUserId) {
    return props.currentUserName || "Me";
  }
  return users.value[userId] || "Unknown User";
};
  
const sendMessage = async () => {
  if (!newMessage.value.trim()) return;

  // Lisa ajutine sõnum
  const tempMessage = {
    id: Date.now(),
    senderId: props.currentUserId,
    message: newMessage.value,
    timestamp: new Date().toISOString(),
  };

  messages.value.push(tempMessage);

  console.log("Temporary message added:", tempMessage);

  try {
    const response = await fetch(`${backendBaseUrl}/api/PredictionGames/${props.gameId}/Chat`, {
      method: "POST",
      headers: {
        "Authorization": `Bearer ${localStorage.getItem("token")}`,
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        senderId: props.currentUserId,
        message: newMessage.value,
      }),
    });

    if (response.ok) {
      const savedMessage = await response.json();

      // Uuenda sõnumi andmed
      messages.value = messages.value.map((msg) =>
        msg.id === tempMessage.id ? { ...savedMessage, senderName: users.value[savedMessage.senderId] || "Unknown User" } : msg
      );

      console.log("Saved message received:", savedMessage);
    } else {
      console.error("Error sending message:", await response.text());
      messages.value.pop(); // Eemalda ajutine sõnum, kui saatmine ebaõnnestub
    }
  } catch (error) {
    console.error("Error sending message:", error);
    messages.value.pop(); // Eemalda ajutine sõnum, kui saatmine ebaõnnestub
  }

  newMessage.value = ""; // Tühjenda sisendväli
  scrollToBottom(); // Kerib lõpuni
};


const scrollToBottom = () => {
  nextTick(() => {
    const chatBox = document.querySelector(".chat-box");
    if (chatBox) {
      chatBox.scrollTop = chatBox.scrollHeight;
    }
  });
  console.log(messages.value);
};

  </script>
  
  <style>
  .chat-box {
    height: 250px;
    overflow-y: auto;
    border: 1px solid #ccc;
    border-radius: 8px;
    padding: 15px;
    background-color: #ffffff;
    margin-bottom: 20px;
  }
  
  .message {
    margin-bottom: 10px;
    color: black;
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
  