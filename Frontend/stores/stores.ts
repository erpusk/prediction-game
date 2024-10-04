import type { GameEvent } from "~/types/gameEvent";
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
  };

  return { predictionGames, loadPredictionGames, addPredictionGame };
});

export const useGameEventsStore = defineStore("gameEvent", () => {
  const api = useApi();
  const gameEvents = ref<GameEvent[]>([]);
  
  const loadGameEvents = async () => {
    gameEvents.value = await api.customFetch<GameEvent[]>("Event");
  };

  const addGameEvent = async (gameEvent: GameEvent) => {
    const res = await api.customFetch("Event", {
      method: "POST",
      body: gameEvent,
    });
  };

  return { gameEvents, loadGameEvents, addGameEvent };
})

