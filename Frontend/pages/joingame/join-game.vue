<template>
    <div class="join-game">
      <h1>Join Game</h1>
      <p>Enter the game code provided by the admin to join.</p>
      <input type="text" v-model="gameCode" placeholder="Enter game code" />
      <button @click="joinGame">Join</button>
      <p v-if="message">{{ message }}</p>
    </div>
  </template>
  
  <script>
  export default {
    data() {
      return {
        gameCode: '',
        message: ''
      };
    },
    methods: {
      async joinGame() {
        try {
          const response = await fetch(`http://localhost:3000/api/PredictionGames/${this.gameCode}/join`, {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json'
            },
            body: JSON.stringify(this.$store.state.userId) // Eeldan, et `userId` on salvestatud Vuex-i
          });
  
          if (response.ok) {
            this.message = 'Successfully joined the game.';
          } else {
            const errorData = await response.json();
            this.message = errorData.message || 'Failed to join the game.';
          }
        } catch (error) {
          this.message = 'An error occurred while joining the game.';
          console.error(error);
        }
      }
    }
  };
  </script>
  
  <style scoped>
  .join-game {
    max-width: 400px;
    margin: 0 auto;
    text-align: center;
  }
  
  input {
    margin-top: 1em;
    padding: 0.5em;
    width: 100%;
    box-sizing: border-box;
  }
  
  button {
    margin-top: 1em;
    padding: 0.5em 1em;
    cursor: pointer;
  }
  
  p {
    margin-top: 1em;
    color: #555;
  }
  </style>
  