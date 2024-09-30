<template>
    <UHorizontalNavigation
      :links="links"
      class="border-b border-gray-200 dark:border-gray-800"
    />
  </template>
  
  <script setup lang="ts">
  const route = useRoute();
  
  const links = [
    {
      label: "Avaleht",
      to: "/",
    },
    {
      label: "Harjutused",
      to: "/exercises",
    },
    {
      label: "Lisa harjutus",
      to: "/add-exercise",
    },
  ];
  </script>  