<template>
    <div class="min-h-screen bg-gray-100 flex justify-center items-start  dark:bg-gray-900">
      <div class="detail-page">
        <h2 class="text-4xl font-semibold text-center mb-8 text-gray-800 dark:text-white">Account</h2>

        <div class="grid md:grid-cols-2 gap-3">

          <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm dark:bg-gray-700 dark:border-gray-500">
            <p class="text-lg font-medium text-gray-700 text-center dark:text-white"><strong>Profile picture:</strong> 
                <img v-if="user.profilePicture != ''"
                  :src="decodeProfilePicture(user.profilePicture)"
                  class="profile-picture"
                />
                <div v-else>
                    No picture added
                </div>
            </p>
          </div>
          <p class="">
          <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm dark:bg-gray-700 dark:border-gray-500 mb-4">
            <p class="text-lg font-medium text-gray-700 text-center dark:text-white"><strong>Email:</strong> {{ user.email }}</p>
          </div>

          <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm dark:bg-gray-700 dark:border-gray-500 mb-4">
            <p class="text-lg font-medium text-gray-700 text-center dark:text-white"><strong>Username:</strong> {{ user.userName }}</p>
          </div>
        
          <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm dark:bg-gray-700 dark:border-gray-500 mb-4">
            <p class="text-lg font-medium text-gray-700 text-center dark:text-white"><strong>Date of birth:</strong> {{ user.dob }}</p>
          </div>
            </p>

          <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm dark:bg-gray-700 dark:border-gray-500 md:col-span-2">
            <p class="text-lg font-medium text-gray-700 text-center dark:text-white" style="white-space: pre-line;"><strong>Created predictiongames:</strong> {{ user.createdPredictionGames }}</p>
          </div>

        </div>
      </div>
    </div>
  </template>
    
    <script setup lang="ts">
  import { ref, onMounted } from 'vue';
  import { useRoute } from 'vue-router';
  import { usePredictionGameStore } from '@/stores/stores';
import PredictiongameId from '~/pages/predictiongame-details/[predictiongameId].vue';
  const route = useRoute();
  const predictionGameStore = usePredictionGameStore();
  const userStore = useUserStore();


  const user = ref({
    id: 0,
    userName: '',
    email: '',
    profilePicture: '',
    dob: '',
    createdPredictionGames: ''
  });

  onMounted(async () => {
    await userStore.loadUser()
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