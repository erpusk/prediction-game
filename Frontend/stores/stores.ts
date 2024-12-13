import { ListFormat } from "typescript";
import type { GameEvent } from "~/types/gameEvent";
import type { Prediction } from "~/types/prediction";
import type { PredictionGame } from "~/types/predictionGame";
import { useUserStore } from '@/stores/userStore';
import type { BonusQuestion } from "~/types/bonusQuestion";

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

  const joinPredictionGame = async (gameCode: string) => {
    try {
      const response = await api.customFetch(`PredictionGames/${gameCode}/join`, {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${userStore.token}`,
        },
      });
  
      return { success: true, message: "Successfully joined the game." };
    } catch (error) {
      const errorMessage = "An error occurred while joining the game.";
      console.error(errorMessage);
      return { success: false, message: errorMessage };
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
  };

  const getLeaderboard = async (predictionGameId: number) => {
    try {
      const leaderboard = await api.customFetch<{ username: string; points: number }[]>(`PredictionGames/${predictionGameId}/leaderboard`, {
        headers: {
        "Authorization": `Bearer ${userStore.token}`,
        },
      });
      return leaderboard;
    } catch(error) {
      console.error("Error with getting leaderboard", error)
      return null;
    }	
  };

  const loadPredictionGames = async () => {
    try {
        const response = await api.customFetch<PredictionGame[]>("PredictionGames");
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

  const getPredictionGameBonusQuestions = async (predictionGameId: number) => {
    const questions = await api.customFetch<BonusQuestion[]>(`BonusQuestion/predictionGame/${predictionGameId}`);
    return questions
  }

  const addPredictionGameBonusQuestion = async (bonusQuestion: BonusQuestion) => {
    const res = await api.customFetch("BonusQuestion", {
      method: "POST",
      body: bonusQuestion,
    });
  };

  return {
    predictionGames,
    loadPredictionGames,
    addPredictionGame,
    deletePredictionGame,
    getPredictionGameById,
    loadPredictionGame,
    leavePredictionGame,
    loadUserPoints,
    getLeaderboard,
    joinPredictionGame,
    getPredictionGameBonusQuestions,
    addPredictionGameBonusQuestion
  };
});


export const useGameEventsStore = defineStore("gameEvent", () => {
  const api = useApi();
  const userStore = useUserStore();
  const gameEvents = ref<GameEvent[]>([]);
  const userUpcomingPredictions = ref<GameEvent[]>([]);
  
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

  const loadUserUpcomingPredictions = async (predictionGameId: number) => {
    const response : GameEvent[] = await api.customFetch<GameEvent[]>(`PredictionGames/${predictionGameId}/Event/upcoming`);
    for (const event of response) {
        userUpcomingPredictions.value.push(event);
    }
  };

  const loadUserPointsForEvent = async (predictionGameId: number, gameEventId: number) => {
    const points = await api.customFetch<number>(`PredictionGames/${predictionGameId}/Event/${gameEventId}/event-user-points`, {
      headers: {
        "Authorization": `Bearer ${userStore.token}`,
      },
    });
    return points;
  }

  return { 
    gameEvents, 
    loadGameEvents, 
    addGameEvent, 
    deletePredictionGameEvent, 
    editPredictionGameEvent, 
    loadSingleEvent, 
    loadUserUpcomingPredictions,
    userUpcomingPredictions,
    loadUserPointsForEvent
  };
})

export const usePredictionsStore = defineStore("prediction", () => {
  const api = useApi();
  const predictions = ref<Prediction[]>([]);
  const userPrediction = ref<Prediction>();
  const userPredictionHistory = ref<Prediction[]>([]);
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

  const getPredictions= async (gameEventId: number) => {
    const url = gameEventId ? `Prediction?eventId=${gameEventId}` : 'Prediction';
      const predictionsList = await api.customFetch<Prediction[]>(url)
    return predictionsList
  };  

  const loadUserPrediction = async (eventId: number) => {
    const prediction = await api.customFetch<Prediction>(`Prediction/user/event/${eventId}`)
    userPrediction.value = prediction;
    if (prediction) {
      userPredictionsMap.value[eventId] = prediction;
    }
  }

  const loadUserPredictionHistory = async () => {
    userPredictionHistory.value = await api.customFetch<Prediction[]>(`Prediction/history`);
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
    userPredictionHistory
  }
})