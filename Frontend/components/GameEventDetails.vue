<template>
  <div class="min-h-screen bg-gray-100 flex justify-center items-start py-10 dark:bg-gray-900">
    <div class="bg-white rounded-xl shadow-lg max-w-4xl w-full p-6 dark:bg-gray-800">
      <h1 class="text-4xl font-semibold text-center mb-8 text-gray-800 dark:text-white">Event Details</h1>
      
      <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm dark:bg-gray-700 dark:border-gray-500">
          <p class="text-lg font-medium text-gray-700 text-center dark:text-white"><strong>Team A:</strong> {{ event.teamA }}</p>
        </div>
        <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm dark:bg-gray-700 dark:border-gray-500">
          <p class="text-lg font-medium text-gray-700 text-center dark:text-white"><strong>Team B:</strong> {{ event.teamB }}</p>
        </div>

        <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm dark:bg-gray-700 dark:border-gray-500">
          <p class="text-lg font-medium text-gray-700 text-center dark:text-white"><strong>Event Date:</strong> {{ event.eventDate }}</p>
        </div>
        <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm dark:bg-gray-700 dark:border-gray-500">
          <p class="text-lg font-medium text-gray-700 text-center dark:text-white"><strong>Game ID:</strong> {{ event.predictionGameId }}</p>
        </div>

        <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm dark:bg-gray-700 dark:border-gray-500">
          <p class="text-lg font-medium text-gray-700 text-center dark:text-white"><strong>Team A Score:</strong> {{ event.teamAScore }}</p>
        </div>
        <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm dark:bg-gray-700 dark:border-gray-500">
          <p class="text-lg font-medium text-gray-700 text-center dark:text-white"><strong>Team B Score:</strong> {{ event.teamBScore }}</p>
        </div>

        <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm col-span-1 md:col-span-2 dark:bg-gray-700 dark:border-gray-500">
          <p class="text-lg font-medium text-gray-700 text-center dark:text-white"><strong>Is Completed:</strong> {{ event.isCompleted ? 'Yes' : 'No' }}</p>
        </div>

        <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm col-span-1 md:col-span-2 dark:bg-gray-700 dark:border-gray-500">
          <p class="text-lg font-medium text-gray-700 text-center dark:text-white"><strong>Your Prediction:</strong> {{ yourPrediction.yourprediction }}</p>
        </div>
      </div>
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
    event.value.eventDate = gameEvent.eventDate ? new Date(gameEvent.eventDate).toLocaleDateString() : '';
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

