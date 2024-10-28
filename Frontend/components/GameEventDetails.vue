<template>
    <div class=" min-h-screen flex items-center justify-center event-details p-6 bg-white rounded-lg shadow-md">
      <div class="bg-white p-6 rounded-lg  max-w-lg w-full">
      <h2 class="text-lg font-bold text-center mb-2 text-black">Event Details</h2>
      <p class="border rounded-md p-2 text-center text-black"><strong>Team A:</strong> {{ event.teamA }}</p>
      <p class="border rounded-md p-2 text-center text-black"><strong>Team B:</strong> {{ event.teamB }}</p>
      <p class="border rounded-md p-2 text-center text-black"><strong>Event Date:</strong> {{ event.eventDate }}</p>
      <p class="border rounded-md p-2 text-center text-black"><strong>Prediction Game ID:</strong> {{ event.predictionGameId }}</p>
      <p class="border rounded-md p-2 text-center text-black"><strong>Team A Score:</strong> {{ event.teamAScore }}</p>
      <p class="border rounded-md p-2 text-center text-black"><strong>Team B Score:</strong> {{ event.teamBScore }}</p>
      <p class="border rounded-md p-2 text-center text-black"><strong>Is Completed:</strong> {{ event.isCompleted ? 'Yes' : 'No' }}</p>
    </div>
  </div>
  </template>
  
  <script setup lang="ts">
  import { defineProps, ref, computed, onMounted } from 'vue';
  import { useGameEventsStore } from '~/stores/stores'; 
  const props = defineProps<{
    id: string | string[];
    predictionGameId: string | string[];
  }>();
  
  const gameEventStore = useGameEventsStore();

  const event = ref({
    id: parseInt(props.id.toString(), 10),
    teamA: '',
    teamB: '',
    eventDate: '',
    predictionGameId: parseInt(props.predictionGameId.toString(), 10),
    teamAScore: 0,
    teamBScore: 0,
    isCompleted: false
  });
  
  onMounted(async () => {
    const gameEvent = await gameEventStore.loadSingleEvent(Number(props.id));
    event.value.teamA = gameEvent.teamA;
    event.value.teamB = gameEvent.teamB;
    event.value.eventDate = gameEvent.eventDate ? new Date(gameEvent.eventDate).toString() : '';
    event.value.teamAScore = gameEvent.teamAScore;
    event.value.teamBScore = gameEvent.teamBScore;
    event.value.isCompleted = gameEvent.isCompleted;
  });
  
  
  </script>
  
  
  