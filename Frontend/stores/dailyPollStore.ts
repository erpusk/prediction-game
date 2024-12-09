export const useDailyPollStore = defineStore("dailyPoll", () => {
    const api = useApi();
    const userStore = useUserStore();
    const homeTeamName = ref<string | null>(null);
    const awayTeamName = ref<string | null>(null);
    const matchId = ref<number | null>(null);
    const selectedTeamHomeOrAway = ref<boolean | null>(null)

    const getDailyPollTeams = async () => {
        try {
          const url = `DailyPoll/daily-poll-match`;
          const dailyGameEvent = await api.customFetch<{
            id: number;
            homeTeamName: string;
            awayTeamName: string;
            utcDate: string;
            status: string;
            isActiveMatch: boolean;
            pollVotes: any[];
          }>(url);
    
          homeTeamName.value = dailyGameEvent.homeTeamName;
          awayTeamName.value = dailyGameEvent.awayTeamName;
          matchId.value = dailyGameEvent.id;
    
          return dailyGameEvent;
        } catch (error) {
          console.error("Failed to fetch daily poll teams:", error);
        }
    };

    const submitVote = async (isForHomeTeam: boolean) => {
      try {
        if (!matchId.value) {
          console.log("No active match ID available for voting.");
        }
        const url = `DailyPoll/vote`;
        await api.customFetch(url, {
          method: "POST",
          headers: {
            "Authorization": `Bearer ${userStore.token}`,
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ DailyPollMatchId: matchId.value, IsForHomeTeam: isForHomeTeam }),
        });
        return true;
      } catch (error) {
        console.error("Failed to submit vote:", error);
        return false;
      }
    };

    const hasUserVoted = async () => {
      try{
        if (!matchId.value) {
          console.log("No active match ID available for fetching results.");
          return;
        }
        const url = `DailyPoll/${matchId.value}/has-voted`
        const response = await api.customFetch<{votedForHomeTeam: boolean | null}>(url)

        if (response.votedForHomeTeam !== null) {
          selectedTeamHomeOrAway.value = response.votedForHomeTeam;
          return true;
        } else {
          selectedTeamHomeOrAway.value = null;
          return null;
        }
      } catch (error) {
        console.error("Failed to fetch user's vote", error);
        return;
      }
    };

    const getPollResults = async () => {
      try {
        if (!matchId.value) {
          console.log("No active match ID available for fetching results.");
          return null;
        }
        const url = `DailyPoll/${matchId.value}/results`;
        return await api.customFetch<{
          totalVotes: number;
          homeTeamVotes: number;
          homeTeamPercentage: number;
          awayTeamVotes: number;
          awayTeamPercentage: number;
        }>(url);
      } catch (error) {
        console.error("Failed to fetch poll results:", error);
      }
    };

    return {
      getDailyPollTeams,
      submitVote,
      getPollResults,
      hasUserVoted,
      homeTeamName,
      awayTeamName,
      matchId,
      selectedTeamHomeOrAway
      };

});