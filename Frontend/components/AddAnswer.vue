<template>
    <div class="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
      <div class="relative">
        <button 
          @click="$emit('close')" 
          class="absolute top-2 right-2 text-black hover:text-red-600 transition duration-300">
          &times;
        </button>
  
        <UForm
          :validate="validate"
          :state="state"
          class="p-6 bg-white dark:bg-gray-800 rounded-lg shadow-lg max-w-lg w-full"
          @submit="onSubmit"
          @error="onError"
        >
          <h2 class="text-2xl font-semibold text-center mb-4 text-black dark:text-white question-text">
            {{ question?.question || 'Loading...' }}
          </h2>
  
          <div v-if="question?.questionType === 'MultipleChoiceQuestion'">
            <UFormGroup label="Choose an option" name="answer">
              <div v-for="(option, idx) in question.options" :key="idx" class="mb-2">
                <label class="flex items-center">
                  <input
                    type="radio"
                    :value="option"
                    v-model="state.answerText"
                    class="mr-2"
                  />
                  {{ option }}
                </label>
              </div>
            </UFormGroup>
          </div>

          <UFormGroup
            v-else-if="question?.questionType === 'NumberQuestion'"
            label="Enter a number:"
            name="answer"
          >
            <UInput
              v-model="state.answerText"
              type="number"
              class="border rounded-md p-2"
              placeholder="Enter a number"
            />
          </UFormGroup>

          <UFormGroup
            v-else
            label="Your answer"
            name="answer"
          >
            <UInput v-model="state.answerText" class="border rounded-md p-2" placeholder="Enter your answer here"/>
          </UFormGroup>
  
          <div class="flex justify-between mt-6 space-x-8">
            <UButton @click="$emit('close')" class="bg-gray-300 hover:bg-gray-400 text-black font-bold py-2 px-4 rounded-md transition duration-300">
              Close
            </UButton>
            <UButton type="submit" class="add-button text-white font-bold py-2 px-4 rounded-md transition duration-300">
              Add
            </UButton>
          </div>
        </UForm>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { reactive } from 'vue';
  import type { FormError, FormErrorEvent } from "#ui/types";
  import type { BonusQuestionAnswer } from '~/types/bonusQuestionAnswer';
  
  const { addAnswer } = useBonusQuestionsStore();
  const emit = defineEmits(['close', 'refresh']);
  
  const props = defineProps<{ question: BonusQuestion }>();
  const question = computed(() => props.question);
  
  const state = reactive<BonusQuestionAnswer>({
    id: 0,
    questionId: 0,
    answerText: "",
    answerMakerId: 0
  });
  
  const validate = (state: any): FormError[] => {
    const errors = [];
    const questionType = question.value?.questionType;
    const answerAsString = String(state.answerText);

    if (!answerAsString.trim()) {
      errors.push({ path: "answerText", message: "Answer cannot be empty" });
    } else if (questionType === "NumberQuestion" && isNaN(Number(state.answerText))) {
      errors.push({ path: "answerText", message: "Answer must be a valid number" });
    }
    return errors;
  };
  
  async function onSubmit() {
    const payload = {
      id: state.id,
      answerText: String(state.answerText),
      questionId: props.question.id,
      answerMakerId: state.answerMakerId
    };
  
    try {
      await addAnswer(payload);
      emit('refresh');
      emit('close');
    } catch (error) {
      console.error("Error adding answer:", error);
    }

    window.location.reload();
  }
  
  function onError(event: FormErrorEvent) {
    const element = document.getElementById(event.errors[0].id);
    element?.focus();
    element?.scrollIntoView({ behavior: "smooth", block: "center" });
  }
  </script>
  
  <style>
  .add-button {
    background-color: #5bb17c;
  }
  .add-button:hover {
    background-color: #26547C;
  }
  
  div :deep(label) {
    color: black !important;
  }
  @media (prefers-color-scheme: dark) {
    div :deep(label) {
      color: #ffffff !important;
    }
  }

  .question-text {
  word-break: break-word;
  white-space: normal;
  display: block;
  max-width: 100%;
}
  </style>