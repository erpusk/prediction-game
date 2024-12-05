<template>
    <div class="min-h-screen bg-white flex justify-center items-center dark:bg-gray-900">
    <form
      name="register-form"
      @submit.prevent="registerUser"
      class="space-y-8 p-16 bg-white rounded-lg shadow-lg max-w-md w-full border rounded-md p-2 dark:bg-gray-800 dark:border-black"
    >
    <h2 class="text-3xl font-semibold text-center mb-6 drop-shadow-lg text-black dark:text-white">Register</h2>

        <UFormGroup label="Username" class="!text-black">
            <UInput v-model="username" class="border rounded-md p-2 dark:border-black" type="text" required/>
        </UFormGroup>
        
        <UFormGroup label="Email">
            <UInput v-model="email" class="border rounded-md p-2 dark:border-black" type="email" required/>
        </UFormGroup>
        
        <UFormGroup label="Password">
            <UInput v-model="password" class="border rounded-md p-2 dark:border-black" type="password" required/>
        </UFormGroup>
        
        <UFormGroup label="Confirm password">
            <UInput v-model="confirmpassword" class="border rounded-md p-2 dark:border-black" type="password" required/>
        </UFormGroup>

        <div class="flex justify-center space-x-4 mt-6">
        <UButton type="submit"
        class="btn-primary font-bold transition duration-300">
            Register
        </UButton>
    </div>
    <div>
        <p class="text-center mt-6 text-gray-600 dark:text-gray-400">
          Already have an account?
          <a @click="goToLogin" class="text-blue-500 hover:underline cursor-pointer">Login</a>
        </p>
      </div>
        <p v-if="errorMessage" class="text-red-500">{{ errorMessage }}</p>
    </form>
    </div>
</template>

<script setup>

const userStore = useUserStore();
const router = useRouter();

const username = ref('');
const email = ref('');
const password = ref('');
const confirmpassword = ref('');
const errorMessage = ref(null);

const registerUser = async () => {
    if (!username.value || !email.value || !password.value || !confirmpassword.value) {
        errorMessage.value = "All fields must be covered";
        return;
    }

    if (password.value !== confirmpassword.value) {
        errorMessage.value = "Passwords do not match.";
        return;
    }

    try {
        const response = await userStore.register({
            username: username.value,
            email: email.value,
            password: password.value,
        });
        userStore.setUser(response.user);
        router.push('/');
    } catch (error) {
        errorMessage.value = "Registration failed, try again.";
    }
};

const goToLogin = () => {
    router.push('/login')
}
</script>

<style scoped>
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