<template>
    <div>
      <div class="chat-box">
        <div v-for="message in messages" :key="message.id" class="message">
          <strong>{{ message.senderName }}</strong>: {{ message.message }}
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
  const predictionGameStore = usePredictionGameStore();
  const userStore = useUserStore();
  
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
  const messages = computed(() => predictionGameStore.messages);
  const newMessage = ref("");
  const currentUserName = ref("");

  onMounted(async() => {
    await userStore.getUserById(props.currentUserId);
    currentUserName.value = userStore.user.userName;
    await predictionGameStore.loadChatMessages(props.gameId);
    scrollToBottom();
  });
  
const sendMessage = async () => {
  if (!newMessage.value.trim()) return;

  const tempMessage = {
    id: Date.now(),
    senderId: props.currentUserId,
    senderName: currentUserName.value,
    message: newMessage.value,
    timestamp: new Date().toISOString(),
  };
  messages.value.push(tempMessage);

  try {
    await predictionGameStore.sendChatMessage(props.gameId, {
      senderId: props.currentUserId,
      message: newMessage.value,
    });
  } catch (error) {
  console.error("Failed to send message:", error);
  } finally {
    newMessage.value = "";
    scrollToBottom();
  }
};


const scrollToBottom = () => {
  nextTick(() => {
    const chatBox = document.querySelector(".chat-box");
    if (chatBox) {
      chatBox.scrollTop = chatBox.scrollHeight;
    }
  });
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
  