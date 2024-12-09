<template>
    <div class="join-game-container">
      <div class="join-game">
        <h1>Join Game</h1>
        <p>Enter the game code provided by the admin to join.</p>
        <input
          type="text"
          v-model="gameCode"
          placeholder="Enter game code"
          class="input-field"
        />
        <button @click="joinGame" class="join-button">Join</button>
        <p v-if="message" :class="messageClass">{{ message }}</p>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref } from 'vue';
  import { useRouter } from 'vue-router';
  
  const router = useRouter();
  const predictionGameStore = usePredictionGameStore();
  
  const gameCode = ref('');
  const message = ref('');
  const messageClass = ref('');
  
  async function joinGame() {
    const { success, message: responseMessage } = await predictionGameStore.joinPredictionGame(gameCode.value);

    message.value = responseMessage;
    messageClass.value = success ? 'success' : 'error';

    if (success) {
      message.value = responseMessage;
      messageClass.value = 'success';

      setTimeout(() => {
        router.push('/predictiongames');
      }, 1500);
    } else {
      message.value = responseMessage || 'Failed to join the game. Please try again.';
      messageClass.value = 'error';
    }
  }
  </script>
  
  <style scoped>
  .join-game-container {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 100vh;
    background-color: #ffffff;
  }

  .dark .join-game-container {
    background-color: #111827;
  }
  
  .join-game {
    max-width: 400px;
    width: 100%;
    padding: 2em;
    text-align: center;
    background-color: #f9f9f9;
    border-radius: 10px;
    box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
  }
  .dark .join-game {
    background-color: #1f2937;
    box-shadow: 0px 4px 8px rgba(255, 255, 255, 0.1);
  }
  
  h1 {
    font-size: 1.5em;
    color: black;
    margin-bottom: 0.5em;
  }

  .dark h1 {
    color: #ffffff;
  }
  
  .input-field {
    margin-top: 1em;
    padding: 0.5em;
    width: 100%;
    border: 1px solid #ccc;
    border-radius: 5px;
  }
  
  .join-button {
    margin-top: 1em;
    padding: 0.5em 1em;
    background-color: #5bb17c;
    color: white;
    font-weight: bold;
    border: none;
    border-radius: 8px;
    cursor: pointer;
  }
  
  .join-button:hover {
    background-color: #26547C;
    transform: scale(1.05);
    animation: slideIn 0.3s ease-in-out;
  }
  
  .success {
    color: #4CAF50;
  }

  .dark .success {
    color: #81c784;
  }
  
  .error {
    color: #FF0000;
  }

  .dark .error {
    color: #e57373;
  }
  </style>
  