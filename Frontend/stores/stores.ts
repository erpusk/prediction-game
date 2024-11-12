import { ListFormat } from "typescript";
import type { GameEvent } from "~/types/gameEvent";
import type { Prediction } from "~/types/prediction";
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
    loadPredictionGames()
  }
  const loadPredictionGame = async (predictionGameId: string | number) => {
    const gameData = await api.customFetch<PredictionGame>(`PredictionGames/${predictionGameId}`);
    return gameData;
  };

  const getPredictionGameById = async (id: number): Promise<PredictionGame | null> => {
    const predictionGame = await api.customFetch<PredictionGame>(`PredictionGames/${id}`);
    return predictionGame || null;
  };

  return { predictionGames, loadPredictionGames, addPredictionGame, deletePredictionGame, getPredictionGameById, loadPredictionGame};
});

export const useGameEventsStore = defineStore("gameEvent", () => {
  const api = useApi();
  const gameEvents = ref<GameEvent[]>([]);
  
  const loadGameEvents = async (predictionGameId: number) => {
    const url = `PredictionGames/${predictionGameId}/Event`;
      gameEvents.value = await api.customFetch<GameEvent[]>(url);
  };

  const addGameEvent = async (gameEvent: GameEvent) => {
    const res = await api.customFetch(`PredictionGames/${gameEvent.predictionGameId}/Event`, {
      method: "POST",
      body: gameEvent,
    });
    loadGameEvents(gameEvent.predictionGameId);
  };

  const deletePredictionGameEvent = async (gameEvent: GameEvent) => {
    await api.customFetch(`PredictionGames/${gameEvent.predictionGameId}/Event/${gameEvent.id}`, {
      method: "DELETE",
    });
    const index = gameEvents.value.findIndex(g => g.id === gameEvent.id);
    
    if (index !== -1) {
      gameEvents.value.splice(index, 1);
    }
    loadGameEvents(gameEvent.predictionGameId);
  }

  const editPredictionGameEvent = async (gameEvent: GameEvent) => {
    await api.customFetch(`PredictionGames/${gameEvent.predictionGameId}/Event/${gameEvent.id}`, {
      method: "PUT",
      body: gameEvent
    })
    loadGameEvents(gameEvent.predictionGameId)
  }

  const loadSingleEvent = async (id: Number) => {
    const eventModel = gameEvents.value.find(g => g.id == id)
    const event = await api.customFetch<GameEvent>(`PredictionGames/${eventModel?.predictionGameId}/Event/${id}`)
    return event;
  }

  return { gameEvents, loadGameEvents, addGameEvent, deletePredictionGameEvent, editPredictionGameEvent, loadSingleEvent };
})

export const usePredictionsStore = defineStore("prediction", () => {
  const api = useApi();
  const predictions = ref<Prediction[]>([]);
  const userPrediction = ref<Prediction>();
  
  const addPrediction = async (prediction: Prediction) => {
    const res: Response = await api.customFetch("Prediction", {
      method: "POST",
      body: prediction
    })
    return res;
  }

  const loadPredictions = async (gameEventId: number) => {
    const url = gameEventId ? `Prediction?eventId=${gameEventId}` : 'Prediction';
      predictions.value = await api.customFetch<Prediction[]>(url);
  };

  const getPredictions= async (gameEventId: number) => {
    const url = gameEventId ? `Prediction?eventId=${gameEventId}` : 'Prediction';
      const predictionsList = await api.customFetch<Prediction[]>(url)
    return predictionsList
  };

  const loadUserPrediction = async (eventid: Number) => {
    console.log(eventid)
    userPrediction.value = await api.customFetch<Prediction>(`Prediction/user/event/${eventid}`)
  }

  return {predictions, addPrediction, loadPredictions, getPredictions, userPrediction, loadUserPrediction}
})