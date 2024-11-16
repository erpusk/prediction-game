import { ListFormat } from "typescript";
import type { GameEvent } from "~/types/gameEvent";
import type { Prediction } from "~/types/prediction";
import type { PredictionGame } from "~/types/predictionGame";
import { useUserStore } from '@/stores/userStore';

export const usePredictionGameStore = defineStore("predictionGame", () => {
  const api = useApi();
  const predictionGames = ref<PredictionGame[]>([]);
  const userStore = useUserStore();

  const leavePredictionGame = async (uniqueCode: string | null) => {
    if (!uniqueCode) {
        console.error("UniqueCode is null or undefined.");
        return;
    }
    try {
        await api.customFetch(`PredictionGames/${uniqueCode}/leave`, {
            method: "POST",
            headers: {
                "Authorization": `Bearer ${userStore.token}`,
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ UserId: userStore.user!.id }),
        });
    } catch (error) {
        console.error("Error leaving the game:", error);
    }
};



  

  const loadPredictionGames = async () => {
    predictionGames.value = await api.customFetch<PredictionGame[]>("PredictionGames");
  };

  const addPredictionGame = async (game: PredictionGame) => {
    const res = await api.customFetch("PredictionGames", {
      method: "POST",
      body: game,
    });
    await loadPredictionGames();
  };

  const deletePredictionGame = async (game: PredictionGame) => {
    await api.customFetch(`PredictionGames/${game.id}`, {
      method: "DELETE",
    });

    const index = predictionGames.value.findIndex(g => g.id === game.id);
    if (index !== -1) {
      predictionGames.value.splice(index, 1);
    }
    await loadPredictionGames();
  };

  const loadPredictionGame = async (predictionGameId: string | number) => {
    const gameData = await api.customFetch<PredictionGame>(`PredictionGames/${predictionGameId}`);
    return gameData;
  };

  const getPredictionGameById = async (id: number): Promise<PredictionGame | null> => {
    const predictionGame = await api.customFetch<PredictionGame>(`PredictionGames/${id}`);
    return predictionGame || null;
  };

  return {
    predictionGames,
    loadPredictionGames,
    addPredictionGame,
    deletePredictionGame,
    getPredictionGameById,
    loadPredictionGame,
    leavePredictionGame,
  };
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
    await loadGameEvents(gameEvent.predictionGameId);
  };

  const deletePredictionGameEvent = async (gameEvent: GameEvent) => {
    await api.customFetch(`PredictionGames/${gameEvent.predictionGameId}/Event/${gameEvent.id}`, {
      method: "DELETE",
    });
    const index = gameEvents.value.findIndex(g => g.id === gameEvent.id);
    
    if (index !== -1) {
      gameEvents.value.splice(index, 1);
    }
    await loadGameEvents(gameEvent.predictionGameId);
  }

  const editPredictionGameEvent = async (gameEvent: GameEvent) => {
    await api.customFetch(`PredictionGames/${gameEvent.predictionGameId}/Event/${gameEvent.id}`, {
      method: "PUT",
      body: gameEvent
    })
    await loadGameEvents(gameEvent.predictionGameId)
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
  const userPredictionHistory = ref<Prediction[]>([]);
  const userUpcomingPredictions = ref<Prediction[]>([]);
  const userPredictionsMap = ref<Record<number, Prediction | null>>({});
  
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

  const loadUserPrediction = async (eventId: number) => {
    console.log(eventId)
    const prediction = await api.customFetch<Prediction>(`Prediction/user/event/${eventId}`)
    userPrediction.value = prediction;
    if (prediction) {
      userPredictionsMap.value[eventId] = prediction;
    }
  }

  const loadUserPredictionHistory = async () => {
    userPredictionHistory.value = await api.customFetch<Prediction[]>(`Prediction/history`);
  };

  const loadUserUpcomingPredictions = async () => {
    userUpcomingPredictions.value = await api.customFetch<Prediction[]>(`Prediction/upcoming`);
  };

  return {
    predictions, 
    addPrediction, 
    loadPredictions, 
    getPredictions, 
    userPrediction, 
    userPredictionsMap, 
    loadUserPrediction, 
    loadUserPredictionHistory,
    userPredictionHistory,
    loadUserUpcomingPredictions,
    userUpcomingPredictions 
  }
})