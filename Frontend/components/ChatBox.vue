<template>
    <div>
      <div class="chat-box">
        <div v-for="message in messages" :key="message.id" class="message">
  <strong>{{ getSenderName(message.senderId) }}</strong>: {{ message.message }}
  <small>{{ new Date(message.timestamp).toLocaleString() }}</small>
</div>


      </div>
  
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
        Authorization: `Bearer ${localStorage.getItem("token")}`,
        "Content-Type": "application/json",
      },
    });

    if (!response.ok) throw new Error(await response.text());

    const messagesFromServer = await response.json();

    const userMap = {};
    messagesFromServer.forEach((msg) => {
      if (msg.senderId && msg.senderName) {
        userMap[msg.senderId] = msg.senderName;
      }
    });
    users.value = userMap;

    messages.value = messagesFromServer.map((msg) => ({
      ...msg,
      senderName: users.value[msg.senderId] || "Unknown User",
    }));

    console.log("Messages after processing:", messages.value);
    scrollToBottom();
  } catch (error) {
    console.error("Error loading messages:", error);
  }
};
  
const getSenderName = (userId) => {

  if (userId === props.currentUserId) {
    return props.currentUserName || "Me";
  }

  const senderName = users.value[userId] || "Unknown User";
  return senderName;
};
  
const sendMessage = async () => {
  if (!newMessage.value.trim()) return;

  const tempMessage = {
    id: Date.now(),
    senderId: props.currentUserId,
    message: newMessage.value,
    timestamp: new Date().toISOString(),
  };

  messages.value.push(tempMessage);

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

      messages.value = messages.value.map((msg) =>
        msg.id === tempMessage.id ? { ...savedMessage, senderName: users.value[savedMessage.senderId] || "Unknown User" } : msg
      );

      console.log("Saved message received:", savedMessage);
    } else {
      console.error("Error sending message:", await response.text());
      messages.value.pop();
    }
  } catch (error) {
    console.error("Error sending message:", error);
    messages.value.pop();
  }

  newMessage.value = "";
  scrollToBottom();
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
  