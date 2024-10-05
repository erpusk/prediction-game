import type { PredictionGame } from "~/types/predictionGame";

export const usePredictionGameStore = defineStore("predictionGame", () => {
  const api = useApi();
  const predictionGames = ref<PredictionGame[]>([]);

  const loadPredictionGames = async () => {
    predictionGames.value = await api.customFetch<PredictionGame[]>("PredictionGames");
  };

  const addPredictionGame = async (game: PredictionGame) => {
    const res = await api.customFetch("PredictionGames", {
      method: "POST",
      body: game,
    });
    loadPredictionGames();
  };

  const deletePredictionGame = async (game: PredictionGame) => {
    await api.customFetch(`PredictionGames/${game.id}`, {
      method: "DELETE",
    });

    const index = predictionGames.value.findIndex(g => g.id === game.id);
    
    if (index !== -1) {
      predictionGames.value.splice(index, 1);
    }
  }

  return { predictionGames, loadPredictionGames, addPredictionGame, deletePredictionGame };
});

