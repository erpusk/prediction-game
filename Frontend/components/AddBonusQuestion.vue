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
          @submit.prevent="onSubmit"
          @error="onError"
        >
        <div>
          <h2 class="text-2xl font-semibold text-center mb-4 text-black dark:text-white">Add a Bonus Question</h2>
          
          <label class="title-text mb-2 block">Choose the question type you want:</label>
          <USelect
            v-model="state.questionType"
            :options="questionTypeOptions"
            class="border rounded-md p-2 mb-4 w-full">
          </USelect>

          <label class="title-text mb-2 block">Question:</label>
          <!-- <UInput
            v-model="state.question"
            class="border rounded-md p-2 mb-4 w-full"
            placeholder="Enter your question here"
            :error="showErrors && !state.question.trim() ? 'Question cannot be empty' : ''"
          /> -->
          <UFormGroup
            :error="showErrors && !state.question.trim() ? 'Question cannot be empty' : ''"
          >
            <UInput
              v-model="state.question"
              placeholder="Enter your question here"
              class="border rounded-md p-2 mb-4 w-full"
            />
          </UFormGroup>

          <div v-if="state.questionType === 'MultipleChoiceQuestion'" class="mb-4">
            <label class="title-text mb-2 block">Options:</label>
            <div class="options-scroll">
              <div v-for="(option, idx) in state.options" :key="idx" class="flex items-center mb-2">
                <UInput v-model="state.options[idx]" class="flex-1 mr-2" placeholder="Enter your option" />
                <div v-if="state.options.length > 2" class="flex items-center">
                  <button type="button" @click="removeOption(idx)" class="text-red-500 hover:underline">Remove</button>
                </div>
              </div>          
            </div>

            <div v-if="showErrors && state.options.filter(opt => opt.trim() !== '').length < 2" class="text-red-500 text-sm mt-1">
              Please enter at least 2 valid options.
            </div>

            <button type="button" @click="addOption" class="text-green-700 hover:underline mt-0">Add another option</button>
          </div>

          <div class="flex justify-between mt-6">
            <UButton @click="$emit('close')" class="bg-gray-300 hover:bg-gray-400 text-black font-bold py-2 px-4 rounded-md transition duration-300">
              Close
            </UButton>
            <UButton type="submit" class="add-button text-white font-bold py-2 px-4 rounded-md transition duration-300">
              Add
            </UButton>
          </div>
        </div>
      </UForm>
    </div>
  </div>
</template>
  
  <script setup lang="ts">
  import { reactive, ref, watch } from 'vue';
  import type { FormError, FormErrorEvent } from "#ui/types";
  import type { BonusQuestion } from "~/types/bonusQuestion";
  import { usePredictionGameStore } from '@/stores/stores';
  
  const { addPredictionGameBonusQuestion } = useBonusQuestionsStore();
  const emit = defineEmits(['close', 'refresh']);
  
  const props = defineProps<{ predictionGameId: number }>();
  const options = ref<string[]>([]);
  const questionTypeOptions = [
  { label: 'Text', value: 'StringQuestion' },
  { label: 'Number', value: 'NumberQuestion' },
  { label: 'Multiple Choice', value: 'MultipleChoiceQuestion' }
];
  
  const state = reactive<BonusQuestion>({
    id: 0,
    predictionGameId: 0,
    question: "",
    questionType: "StringQuestion",
    options: [],
  });
  
  const showErrors = ref(false);

  const validate = (state: any): FormError[] => {
    const errors = [];
    if (showErrors.value && !state.question.trim()) {
      errors.push({ path: "question", message: "Question cannot be empty" });
    }

    if (state.questionType === 'MultipleChoiceQuestion') {
      const nonEmptyOptions = state.options.filter(opt => opt.trim() !== '');
      if (nonEmptyOptions.length < 2) {
        errors.push({
          path: "options",
          message: "Multiple choice questions must have at least 2 options"
        });
      }
    }
    return errors;
  };
  
  async function onSubmit() {
    showErrors.value = true;
    if (!state.question.trim()) return;

    if (
      state.questionType === 'MultipleChoiceQuestion' &&
      state.options.filter(opt => opt.trim() !== '').length < 2
    ) return;

    const payload = {
      id: state.id,
      question: state.question,
      predictionGameId: props.predictionGameId,
      questionType: state.questionType,
      options: (state.questionType === "MultipleChoiceQuestion")
        ? state.options?.filter(opt => opt.trim() !== '')
        : []
    };
  
    try {
      await addPredictionGameBonusQuestion(payload);
      // emit('refresh');
      emit('close');
      // window.location.reload();
    } catch (error) {
      console.error("Error adding bonus question:", error);
    }

    window.location.reload();
  }
  
  function onError(event: FormErrorEvent) {
    const element = document.getElementById(event.errors[0].id);
    element?.focus();
    element?.scrollIntoView({ behavior: "smooth", block: "center" });
  }

  watch(() => state.questionType, (newType) => {
  if (newType === 'MultipleChoiceQuestion') {
    if (state.options.length === 0) state.options = ['', ''];
  } else {
    state.options = [];
  }
});

function addOption() {
  state.options.push('');
}
function removeOption(idx: number) {
  state.options.splice(idx, 1);
}

  </script>
  
  <style>
  .title-text {
    font-size: 14px;
    font-weight: bold;
    color: #505050;
    padding-top: 10px;
  }

  .options-scroll {
  max-height: 200px;
  overflow-y: auto;
  padding-right: 4px;
}

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
  </style>
  