<template>
    <div class="game-details-container">
      <!-- <h1 class="text-2xl font-bold text-center my-4">{{ predictionGame.title }}</h1> -->
      <nav class="tabs">
        <button 
          :class="{ active: activeTab === 'events' }"
          @click="activeTab = 'events'"
          class="tab-button"
        >
          Events
        </button>
        <button 
          :class="{ active: activeTab === 'details' }"
          @click="activeTab = 'details'"
          class="tab-button"
        >
          Details
        </button>
        <button 
          :class="{ active: activeTab === 'bonusQuestions' }"
          @click="activeTab = 'bonusQuestions'"
          class="tab-button"
        >
          Bonus Questions
        </button>
      </nav>
  
      <div class="tab-content">
        <GameEventsList 
          v-if="activeTab === 'events'" 
          :predictionGameId="predictionGameId" 
        />
        <PredictionGameDetail 
          v-if="activeTab === 'details'" 
          :predictionGameId="predictionGameId"
        />
        <BonusQuestionList 
          v-if="activeTab === 'bonusQuestions'" 
          :predictionGameId="predictionGameId" 
        />
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref } from 'vue';
  import GameEventsList from '@/components/GameEventsList.vue';
  import PredictionGameDetail from '@/components/PredictionGameDetail.vue';
  import BonusQuestionList from '@/components/BonusQuestionList.vue';
  
  defineProps<{ predictionGameId: number }>();
  
  const activeTab = ref('events');
  </script>
  
  <style scoped>
  .tabs {
    display: flex;
    justify-content: center;
    gap: 1rem;
    margin-bottom: 1rem;
  }
  
  .tab-button {
    padding: 0.5rem 1rem;
    border: none;
    background-color: #f3f4f6;
    cursor: pointer;
    border-radius: 5px;
    font-weight: bold;
  }
  
  .tab-button.active {
    background-color: #2563eb;
    color: white;
  }
  
  .tab-content {
    padding: 1rem;
  }
  </style>  