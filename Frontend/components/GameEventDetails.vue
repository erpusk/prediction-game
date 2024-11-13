<template>
    <div class=" min-h-screen bg-white flex justify-center items-start mt-10">
      <div class="bg-white rounded-lg shadow-lg max-w-6xl w-full relative">
      <h1 class="text-3xl font-bold text-center mb-6 text-black">Event Details</h1>
      <p class="border rounded-md p-2 text-center"><strong>Team A:</strong> {{ event.teamA }}</p>
      <p class="border rounded-md p-2 text-center"><strong>Team B:</strong> {{ event.teamB }}</p>
      <p class="border rounded-md p-2 text-center"><strong>Event Date:</strong> {{ event.eventDate }}</p>
      <p class="border rounded-md p-2 text-center"><strong>Prediction Game ID:</strong> {{ event.predictionGameId }}</p>
      <p class="border rounded-md p-2 text-center"><strong>Team A Score:</strong> {{ event.teamAScore }}</p>
      <p class="border rounded-md p-2 text-center"><strong>Team B Score:</strong> {{ event.teamBScore }}</p>
      <p class="border rounded-md p-2 text-center"><strong>Is Completed:</strong> {{ event.isCompleted ? 'Yes' : 'No' }}</p>
      <p class="border rounded-md p-2 text-center"><strong>Your prediction:</strong> {{ yourPrediction.yourprediction }}</p>
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
  const predictionStore = usePredictionsStore();

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

  const yourPrediction = ref({
    yourprediction: ''
  })
  
  onMounted(async () => {
    const gameEvent = await gameEventStore.loadSingleEvent(Number(props.id));
    event.value.teamA = gameEvent.teamA;
    event.value.teamB = gameEvent.teamB;
    event.value.eventDate = gameEvent.eventDate ? new Date(gameEvent.eventDate).toString() : '';
    event.value.teamAScore = gameEvent.teamAScore;
    event.value.teamBScore = gameEvent.teamBScore;
    event.value.isCompleted = gameEvent.isCompleted;

    await predictionStore.loadUserPrediction(parseInt(props.id.toString(), 10));
    const prediction = predictionStore.userPrediction;
    if (!prediction){
      yourPrediction.value.yourprediction = 'You have not made a prediction' 
    }
    else {
      yourPrediction.value.yourprediction = `${prediction.endScoreTeamA} - ${prediction.endScoreTeamB}`
    }
  });
  
  
  </script>
<style scoped>

h1 {
  color: #2c3e50;
}
 
</style>
 
  
  