export const useWorkoutStore = defineStore('workout', () => {
    const exercises = ref<Exercise[]>([
        { id: 1, name: 'Harjutus 1', description: 'Kirjeldus 1' },
        { id: 2, name: 'Harjutus 2', description: 'Kirjeldus 2' },
        { id: 3, name: 'Harjutus 3', description: 'Kirjeldus 3' },
        { id: 4, name: 'Harjutus 4', description: 'Kirjeldus 4' },
      ]);

      let currentId = 4;
    
      const addExercise = (exercise: Omit<Exercise, 'id'>) => {
        currentId++;
        exercises.value.push({...exercise, id: currentId});
      };

    const deleteExercise = (exercise: Exercise) => {
        const index = exercises.value.findIndex(e => e.id === exercise.id);

        if (index !== -1) {
            exercises.value.splice(index, 1);
        }
    };
    
    return { exercises, addExercise, deleteExercise }
  })
