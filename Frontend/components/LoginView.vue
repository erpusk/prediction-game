<template>
  <div class="min-h-screen bg-white flex justify-center items-center dark:bg-gray-900">
    <form name="login-form" @submit.prevent="handleLogin"
      class="space-y-8 p-16 bg-white rounded-lg shadow-lg max-w-md w-full border rounded-md p-4 dark:bg-gray-800 dark:border-black">
      <h2 class="text-3xl font-semibold text-center mb-6 drop-shadow-lg text-black dark:text-white">Login</h2>

      <UFormGroup label="Email" class="!text-black">
        <UInput v-model="email" class="border rounded-md p-4 text-lg dark:border-black" type="email" required />
      </UFormGroup>

      <UFormGroup label="Password" class="!text-black">
        <UInput v-model="password" class="border rounded-md p-4 text-lg dark:border-black" type="password" required />
      </UFormGroup>

      <div class="flex justify-center space-x-4 mt-6">
        <UButton type="submit"
          class="btn-primary font-bold transition duration-300">
          Login
        </UButton>
      </div>
      <div>
        <p class="text-center mt-6 text-gray-600 dark:text-gray-400">
          Don´t have an account?
          <a @click="goToRegister" class="text-blue-500 hover:underline cursor-pointer">Register</a>
        </p>
      </div>
       
        <p v-if="errorMessage" class="text-red-500 text-center mt-4">{{ errorMessage }}</p>
    </form>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useUserStore } from '~/stores/userStore'; 
import { useRouter } from 'vue-router';

const userStore = useUserStore();
const router = useRouter(); 

const email = ref('');
const password = ref('');
const errorMessage = ref(null);

const handleLogin = async () => {
  errorMessage.value = null;
  try {
    await userStore.login({ email: email.value, password: password.value });

    if(userStore.isAuthenticated) {
      router.push('/');
    } else {
      throw new Error('Autentimine ebaõnnestus')
    }
  } catch(error) {
    errorMessage.value = error.message || 'Vale kasutajanimi või parool';
  }
};

const goToRegister = () => {
  router.push('/register');
};
</script>

<style scoped>
div :deep(label) {
  color: black !important;
}
@media (prefers-color-scheme: dark) {
  div :deep(label) {
    color: #ffffff !important;
  }
}

.btn-primary {
  background-color: #5bb17c;
  color: #fff;
  padding: 10px 28px;
  border: none;
  border-radius: 80px;
  cursor: pointer;
  font-family: 'Merriweather', sans-serif;
  transition: background-color 0.3s, transform 0.3s;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.btn-primary:hover {
  background-color: #26547C;
  transform: scale(1.05)
}

.btn-primary:active {
  background-color: #26547C;
  transform: scale(1);
}
</style>