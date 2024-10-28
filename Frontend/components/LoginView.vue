<template>
    <form name="login-form" @submit.prevent="handleLogin">
        <UFormGroup label="Email">
            <UInput v-model="email" class="border rounded-md p-2" type="email" required/>
        </UFormGroup>

        <UFormGroup label="Password">
            <UInput v-model="password" class="border rounded-md p-2" type="password" required/>
        </UFormGroup>
        
        <UButton type="submit">
            Login
        </UButton>

        <p v-if="errorMessage" class="text-red-500">{{ errorMessage }}</p>
    </form>
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
</script>
