<template>
  <div class="p-2">
    <NuxtLayout>
    <NuxtPage />
  </NuxtLayout>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { useUserStore } from '~/stores/userStore';

const userStore = useUserStore();

onMounted(async () => {
  await userStore.loadUser();
});
</script>