<template>
  <div class="p-4">
    <h1 class="text-2xl font-bold">Bonus Question Detail</h1>
    <p v-if="question">Question: {{ question.question }}</p>
    <p v-if="question?.correctAnswer">Correct Answer: {{ question.correctAnswer }}</p>
    <p v-else-if="isGameCreator">No correct answer yet. You can add one here.</p>
    <p v-else>No correct answer available.</p>

    <div v-if="answers.length">
      <h2 class="text-xl font-semibold mt-4">User Answers:</h2>
      <ul>
        <li v-for="a in answers" :key="a.id">{{ a.answerText }}</li>
      </ul>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useRoute } from 'vue-router'
import { onMounted, ref } from 'vue'
import { useBonusQuestionsStore } from '@/stores/stores'
import type { BonusQuestion } from '~/types/bonusQuestion'
import type { BonusQuestionAnswer } from '~/types/bonusQuestionAnswer'

const route = useRoute()
const id = Number(route.params.id)
const bonusQuestionStore = useBonusQuestionsStore()
const userStore = useUserStore();
const predictionGameStore = usePredictionGameStore();
const question = ref<BonusQuestion | null>(null)
const answers = ref<BonusQuestionAnswer[]>([])
const isGameCreator = ref(false)

onMounted(async () => {
  question.value = await bonusQuestionStore.getBonusQuestionById(id)
  await bonusQuestionStore.loadAnswers(id)
  answers.value = bonusQuestionStore.answers

  const currentUserId = userStore.user?.id
  const predictionGame = predictionGameStore.getPredictionGameById(question.value?.predictionGameId)
  isGameCreator.value = predictionGame.gameCreatorId === currentUserId
})
</script>