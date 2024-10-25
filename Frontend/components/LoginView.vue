<template>
    <form name="login-form" @submit.prevent="login">
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
import { useUserStore } from '~/stores/userStore'; // Import userStore
import { useRouter } from 'vue-router';

const userStore = useUserStore(); // Kasutame userStore
const router = useRouter(); // Routeri kasutamine

const email = ref('');
const password = ref('');
const errorMessage = ref(null);


const login = async () => {
  await new Promise((resolve) => setTimeout(resolve, 500)); // Simuleeri viivitust

  if (email.value === 'test@domain.ee' && password.value === '1234') {
    
    const fakeUser = { id: 1, name: 'Maksim', token: 'fake-jwt-token' };
    const fakeToken = 'fake-jwt-token';
    userStore.setUser(fakeUser); 
    userStore.setToken(fakeToken)
    router.push('/'); 

  } else {
    errorMessage.value = 'Vale kasutajanimi või parool.'; 
  }
};
</script>
