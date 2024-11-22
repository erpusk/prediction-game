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
  import { useUserStore } from '@/stores/userStore';
  
  const router = useRouter();
  const userStore = useUserStore();
  
  const gameCode = ref('');
  const message = ref('');
  const messageClass = ref('');
  
  async function joinGame() {
    try {
      const userId = userStore.user?.id;
      if (!userId) {
        message.value = 'You must be logged in to join a game.';
        messageClass.value = 'error';
        return;
      }
  
      const response = await fetch(
        `http://localhost:5160/api/PredictionGames/${gameCode.value}/join`,
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${userStore.token}`,
          },
          body: JSON.stringify({ userId }),
        }
      );
  
      if (response.ok) {
        message.value = 'Successfully joined the game.';
        messageClass.value = 'success';
        setTimeout(() => {
          router.push('/predictiongames');
        }, 2000);
      } else {
        const errorData = await response.json();
        message.value = errorData.message || 'No games with the provided code';
        messageClass.value = 'error';
      }
    } catch (error) {
      message.value = 'An error occurred while joining the game.';
      messageClass.value = 'error';
      console.error(error);
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
  
  .join-game {
    max-width: 400px;
    width: 100%;
    padding: 2em;
    text-align: center;
    background-color: #f9f9f9;
    border-radius: 10px;
    box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
  }
  
  h1 {
    font-size: 1.5em;
    color: black;
    margin-bottom: 0.5em;
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
    border-radius: 5px;
    cursor: pointer;
  }
  
  .join-button:hover {
    background-color: #5bb17c;
  }
  
  .success {
    color: #4CAF50;
  }
  
  .error {
    color: #FF0000;
  }
  </style>
  