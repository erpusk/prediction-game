<template>
    <div class="main-page h-screen bg-gray-100 flex flex-col justify-center items-center">
        <div class="logo text-center mb-20 logo container">
            <h1 class="logo-text">Prediction Games</h1>
        </div>
      <div class="button-group flex flex-col space-y-10 items-center">
        <button class="btn-primary" @click="ToCreateGame">Create Game</button>
        <button class="btn-primary" @click="ToJoinGame">Join Game</button>
        <button class="btn-primary" @click="ToMyGames">Games</button>
      </div>
    </div>
  </template>
  
<script setup lang="ts">
import { useRouter } from 'vue-router';
import { useUserStore } from '#imports';


const userStore = useUserStore();
const router = useRouter();

const userName = computed(() => userStore.user?.userName);

const ToCreateGame = () => {
  router.push('/add-predictiongame');
};
const ToMyGames = () => {
  router.push('/predictiongames');
};
const ToLogin = () => {
  router.push('/login')
};
const ToRegister = () => {
  router.push('/register')
}
const ToJoinGame = () => {
  router.push('/join-game');
};
</script>

  
<style scoped>
  .logo-text {
  font-family: 'Poppins', sans-serif;
  font-size: 3rem;
  font-weight: 700;
  color: #2e3747;
  text-shadow: 2px 2px 5px rgba(67, 107, 194, 0.2);
  

}
  .main-page {
    background-color: #f6f3f3;
    padding-top: 20px;
  }
  .header {
    position: absolute;
    top: 16px;
    left: 16px;
  }
  .btn-primary {
    padding: 20px 60px;
    font-size: 18px;
    font-weight: bold;
    color: #fff;
    background-color: #5bb17c;
    border-radius: 80px;
    border: none;
    cursor: pointer;
    transition: background-color 0.3s;
  }
  .btn-primary:hover {
    background-color: #4d6bac;
  }
  .btn-secondary {
    padding: 5px 10px;
    font-size: 18px;
    font-weight: bold;
    color: #fff;
    background-color: #4074d3;
    border-radius: 8px;
    border: none;
    cursor: pointer;
    transition: background-color 0.3s;
  }
  .btn-secondary:hover {
    background-color: #4d4a4a;
  }

  .user-greeting {
  font-size: 1.2rem;
  color: #333;
  margin-left: 10px;
  }
  </style>

  
  
