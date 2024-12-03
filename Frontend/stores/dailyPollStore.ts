export const useDailyPollStore = defineStore("dailyPoll", () => {
    const api = useApi();
    const homeTeamName = ref<string | null>(null);
    const awayTeamName = ref<string | null>(null);

    const getDailyPollTeams = async () => {
        try {
          const url = `DailyPoll/user-poll-match`;
          const dailyGameEvent = await api.customFetch<{
            date: string;
            status: string;
            competition: {
              name: string;
              code: string;
            };
            homeTeam: {
              name: string;
            };
            awayTeam: {
              name: string;
            };
          }>(url);
    
          homeTeamName.value = dailyGameEvent.homeTeam.name;
          awayTeamName.value = dailyGameEvent.awayTeam.name;
    
          return dailyGameEvent;
        } catch (error) {
          console.error("Failed to fetch daily poll teams:", error);
        }
      };

    return {
        getDailyPollTeams,
        homeTeamName,
        awayTeamName,
      };

});