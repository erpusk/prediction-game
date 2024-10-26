<template>
  <TheMenu />
  <div class="p-2">
    <NuxtPage />
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