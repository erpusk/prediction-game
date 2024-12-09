<template>
    <div class="flex justify-center items-start bg-gray-100 dark:bg-gray-900">
        
      <div class="w-full max-w-md p-4 relative bg-white dark:bg-gray-800 rounded-lg shadow-lg">

        <button @click="$emit('close')" class="absolute top-2 right-2 text-gray-500 dark:text-white text-xl">
        &times;
      </button>
        
        <h2 class="text-3xl font-semibold text-center text-gray-800 dark:text-white mb-6">{{ user.userName }}</h2>
  
        <div class="bg-gray-50 dark:bg-gray-700 p-4 mb-4 border border-gray-200 rounded-lg shadow-sm">
          <p class="text-lg font-medium text-gray-700 dark:text-white text-center">
            <strong>Profile picture:</strong>
            <img v-if="user.profilePicture" :src="decodeProfilePicture(user.profilePicture)" class="w-32 h-32 rounded-full mx-auto mb-4" />
            <div v-else>No picture added</div>
          </p>
        </div>
  
        <div class="bg-gray-50 dark:bg-gray-700 p-4 mb-4 border border-gray-200 rounded-lg shadow-sm">
          <p class="text-lg font-medium text-gray-700 dark:text-white text-center"><strong>Date of birth:</strong> {{ user.dob }}</p>
        </div>
  
        <div v-if="user.createdPredictionGames.trim()" class="bg-gray-50 dark:bg-gray-700 p-4 border border-gray-200 rounded-lg shadow-sm">
          <p class="text-lg font-medium text-gray-700 dark:text-white text-center" style="white-space: pre-line;"><strong>Created predictiongames:</strong> {{ user.createdPredictionGames }}</p>
        </div>
      </div>
    </div>
  </template>
    
<script setup lang="ts">
  import { ref, onMounted } from 'vue';
  const userStore = useUserStore();

  const props = defineProps<{
    id: number
  }>();

  const user = ref({
    id: 0,
    userName: '',
    email: '',
    profilePicture: '',
    dob: '',
    createdPredictionGames: ''
  });

  onMounted(async () => {
    console.log(props.id)
    await userStore.getUserById(props.id)
    if (userStore.user != null){
        user.value.id = userStore.user.id
        user.value.userName = userStore.user.userName    
        user.value.email = userStore.user.email
        user.value.profilePicture = userStore.user.profilePicture
        user.value.dob = userStore.user.dateOfBirth ? new Date(userStore.user.dateOfBirth).toLocaleDateString() : ''
        const createdPredictionGamesNames = [] as string[];
        userStore.user.createdPredictionGames.forEach(async game => {
        createdPredictionGamesNames.push(game.predictionGameTitle)
        })
        user.value.createdPredictionGames = "\n" + createdPredictionGamesNames.join('\n');
    }
  });

  function decodeProfilePicture(picString: any){
  return `data:image/jpeg;base64,${picString}`;
    }

   
</script>
    
<style scoped>
    .detail-page {
      padding: 20px;
      background-color: white;
      border-radius: 8px;
      max-width: 600px;
      margin: 20px auto;
    }
    
    .dark .detail-page {
      background-color: #1f2937;
    }

    .profile-picture {
    height: 200px;
    width: 200px;
    border-radius: 50%; 
    object-fit: cover; 
    border: 2px solid #ddd; 
    }
</style>