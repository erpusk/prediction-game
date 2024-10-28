<template>
    <form name="register-form" @submit.prevent="registerUser">

        <UFormGroup label="Username">
            <UInput v-model="username" class="border rounded-md p-2" type="text" />
        </UFormGroup>
        
        <UFormGroup label="Email">
            <UInput v-model="email" class="border rounded-md p-2" type="email" />
        </UFormGroup>
        
        <UFormGroup label="Password">
            <UInput v-model="password" class="border rounded-md p-2" type="password" />
        </UFormGroup>
        
        <UFormGroup label="Confirm password">
            <UInput v-model="confirmpassword" class="border rounded-md p-2" type="password" />
        </UFormGroup>

        <UButton type="submit">
            Register
        </UButton>
        
        <p v-if="errorMessage" class="text-red-500">{{ errorMessage }}</p>
    </form>
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
        console.log(response)
        console.log("before setting user")

        userStore.setUser(response.user);
        console.log("before setting user2")
        router.push('/');
        console.log("before setting user3")
    } catch (error) {
        errorMessage.value = "Registration failed, try again.";
    }
};
</script>