import { ListFormat } from "typescript";
import type { GameEvent } from "~/types/gameEvent";
import type { Prediction } from "~/types/prediction";
import type { PredictionGame } from "~/types/predictionGame";
import { useUserStore } from '@/stores/userStore';
import PredictionGameId from "~/pages/add-gameevents/[predictionGameId].vue";

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

      // After leaving, reload the prediction games list to reflect changes
      await loadPredictionGames();
    } catch (error) {
      console.error("Error leaving the game:", error);
    }
  };

  const loadUserPoints = async (predictionGameId: number) => {
    try {
      const points = await api.customFetch<number>(`PredictionGames/${predictionGameId}/user-points`, {
        headers: {
        "Authorization": `Bearer ${userStore.token}`,
        },
      });
      return points;
    } catch(error) {
      console.error("Error with getting user points", error)
      return null;
    }	
  }

  const loadPredictionGames = async () => {
    try {
        const response = await api.customFetch<PredictionGame[]>("PredictionGames");
        console.log("Loaded Prediction Games:", response);
        predictionGames.value = response;
    } catch (error) {
        console.error("Error loading prediction games:", error);
    }
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
  
    await loadPredictionGames();
  };
  
  


  const loadPredictionGame = async (predictionGameId: number) => {
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
    loadUserPoints
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
  const userPredictionsMap = ref<Record<number, Prediction | null>>({});
  
  const addPrediction = async (prediction: Prediction) => {
    const res: Response = await api.customFetch("Prediction", {
      method: "POST",
      body: prediction
    })
    await loadPredictions(prediction.eventId)
    return res
  }

  const loadPredictions = async (gameEventId: number) => {
    const url = gameEventId ? `Prediction?eventId=${gameEventId}` : 'Prediction';
      predictions.value = await api.customFetch<Prediction[]>(url);
  };

  

  const loadUserPrediction = async (eventId: number) => {
    const prediction = await api.customFetch<Prediction>(`Prediction/user/event/${eventId}`)
    userPrediction.value = prediction;
    if (prediction) {
      userPredictionsMap.value[eventId] = prediction;
    }
  }

  return {predictions, addPrediction, loadPredictions, userPrediction, userPredictionsMap, loadUserPrediction}
})