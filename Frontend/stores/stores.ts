export const useWorkoutStore = defineStore("workout", () => {
  const api = useApi();
  const exercises = ref<Exercise[]>([]);


  const loadExercises = async () => {
    exercises.value = await api.customFetch<Exercise[]>("Exercises");
  };

  const addExercise = async (exercise: Exercise) => {
    const res = await api.customFetch("Exercises", {
      method: "POST",
      body: exercise,
    });
  };

  return { exercises, loadExercises, addExercise };
});

