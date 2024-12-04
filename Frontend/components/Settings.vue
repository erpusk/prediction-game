<template>
    <div class="min-h-screen flex justify-center items-center bg-white dark:bg-gray-900">
    <UForm
      :state="state"
      class="p-6 bg-white rounded-lg shadow-lg max-w-lg w-full dark:bg-gray-800"
      @submit="onSubmit"
    >
      <h2 class="text-2xl font-semibold text-center mb-4 text-black dark:text-white">Settings</h2>
      <UFormGroup label="Username" name="userName" class="!text-black">
        <UInput v-model="state.userName" class="border rounded-md p-2"/>
      </UFormGroup>

      <UFormGroup label="Profile picture" name="profilePicture" class="!text-black">
        <div>
          <div v-if="state.profilePicture" class="relative inline-block">
            <img
              :src="`data:image/png;base64,${state.profilePicture}`"
              alt="Current Profile Picture"
              class="w-20 h-20 object-cover rounded-full mb-2 border"
            />
            <button
              @click="removeProfilePicture"
              class="absolute top-0 right-0 bg-red-600 text-white rounded-full w-5 h-5 flex items-center justify-center text-xs shadow-md hover:bg-red-700 focus:outline-none"
            >
            X
            </button>
          </div>

        </div>
        <input v-if="!state.profilePicture" type="file" accept="image/*" class="border rounded-md p-2" @change="onFileChange" />
      </UFormGroup>
      
      <UButton type="submit" class="confirm-button text-white font-bold py-2 px-4 rounded-md transition duration-300 dark:hover:bg-blue-600 dark:bg-blue-500">
          Confirm
        </UButton>
    </UForm>
  </div>
  </template>

<script setup lang="ts">
const userStore = useUserStore();
const router = useRouter();


const state = reactive<AppUser>({
    id: 0,
    userName: '',
    email: '',
    profilePicture: ''
})

onMounted(async () => {
    await userStore.loadUser()
    if (userStore.user != null){
        state.id = userStore.user.id
        state.userName = userStore.user.userName    
        state.email = userStore.user.email
        state.profilePicture = userStore.user.profilePicture
    }
})


async function onSubmit() {
    const payload = {
        id: state.id,
        userName: state.userName,
        email: state.email,
        profilePicture: state.profilePicture,
    };

    console.log(payload)

    await userStore.updateUser(payload);

    window.location.reload();
}

function removeProfilePicture(){
  state.profilePicture = ''
}

function onFileChange(event: Event) {
  const file = (event.target as HTMLInputElement).files?.[0];
  if (file) {
    const reader = new FileReader();
    reader.onload = () => {
      state.profilePicture = (reader.result as string).split(',')[1]; // Base64 string
    };
    reader.readAsDataURL(file);
  }
}

</script>

<style scoped>
.user-settings {
  max-width: 600px;
  margin: 0 auto;
  padding: 2rem;
  background-color: #f9f9f9;
  border: 1px solid #ddd;
  border-radius: 8px;
}

h1 {
  font-size: 1.5rem;
  margin-bottom: 1rem;
}

.settings-form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.form-group {
  display: flex;
  flex-direction: column;
}

label {
  font-weight: bold;
  margin-bottom: 0.5rem;
}

input.form-control {
  padding: 0.5rem;
  border: 1px solid #ccc;
  border-radius: 4px;
}

input.readonly {
  background-color: #e9ecef;
  cursor: not-allowed;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
}

button.btn-save {
  padding: 0.5rem 1rem;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

button.btn-save:hover {
  background-color: #0056b3;
}
</style>