<template>
    <BonusQuestionDetails :bonus-question-id="Id"/>
</template>

<script setup lang="ts"> 
  import { useRoute } from 'vue-router';
  
  const route = useRoute();
  const Id = parseInt(route.params.bonusQuestionId.toString());
</script>