<template>
    <form name="register-form">

        <UFormGroup label="Username">
            <UInput class="border rounded-md p-2" type="text"/>
        </UFormGroup>
        
        <UFormGroup label="Email">
            <UInput class="border rounded-md p-2" type="email"/>
        </UFormGroup>
        
        <UFormGroup label="Password">
            <UInput class="border rounded-md p-2" type="password"/>
        </UFormGroup>
        
        <UFormGroup label="Confirm password">
            <UInput class="border rounded-md p-2" type="password"/>
        </UFormGroup>

        <UButton type="submit">
            Register
        </UButton>
        
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
    if(!username.value || !email.value || !password.value || !confirmpassword.value) {
        errorMessage.value = "All fields must be covered";
        return;
    }

    if(password.value !== confirmpassword.value) {
        errorMessage.value = "Passwords do not match."
        return;
    }

    try {
        await userStore.register({
            username: username.value,
            email: email.value,
            password: password.value,
    })

    userStore.setUser(response.user);
    router.push('/')
    } catch(error) {
        errorMessage.value = "Registration failed, try again."
    }
};
</script>