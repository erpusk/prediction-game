<template>
    <h2 class="text-lg font-bold font-merriweather-400 text-center">Who wins tonight?</h2>

    <!-- Poll Options -->
    <div class="poll-rectangle relative flex mt-2 w-full rounded-lg border border-gray-300">
        <!-- Team A Background -->
        <div
            class="team-bg team-a-bg absolute h-full"
            :style="{ 
            width: showResults ? `${teamAPercentage}%` : '50%',
            backgroundColor: selectedOption === true ? '#26547C' : '#26547C8b' }">
        </div>

        <!-- Team B Background -->
        <div
            class="team-bg team-b-bg absolute h-full"
            :style="{ 
            width: showResults ? `${teamBPercentage}%` : '50%',
            backgroundColor: selectedOption === false ? '#26547C' : '#26547C8b' }">
        </div>

        <!-- Divider -->
        <div
            class="divider absolute h-full"
            :style="{ left: showResults ? `${teamAPercentage}%` : '50%', transition: 'left 0.5s ease' }">
        </div>

        <!-- Team A Section -->
        <div
            class="team-section flex relative items-center text-center cursor-pointer w-1/2"
            :class="{ selected: selectedOption === true, 'static-result': showResults, disabled: hasVoted }"
            @click="vote(true)">
            <span class="team-name font-merriweather-400 absolute inset-0 flex items-center justify-center">
            {{ homeTeamName }}
            </span>
            <div
            v-if="showResults"
            class="percentage-overlay absolute inset-0 flex items-end justify-center">
            <span><strong>{{ `${teamAPercentage}%` }}</strong></span>
            </div>
        </div>

        <!-- Team B Section -->
        <div
            class="team-section flex-1 relative text-center cursor-pointer"
            :class="{ selected: selectedOption === false, 'static-result': showResults, disabled: hasVoted }"
            @click="vote(false)">
            <span class="team-name font-merriweather-400 absolute inset-0 flex items-center justify-center">
            {{ awayTeamName }}
            </span>
            <div
            v-if="showResults"
            class="percentage-overlay absolute inset-0 flex items-end justify-center">
            <span><strong>{{ `${teamBPercentage}%` }}</strong></span>
            </div>
        </div>
    </div>
    <div v-if="showResults" class="text-center mt-4">
        <p class="text-sm text-gray-600 font-merriweather-400">
            Based on {{ totalVotes }} votes
        </p>
    </div>
</template>


<script setup lang="ts">
    import { ref, computed } from 'vue';
    import { useDailyPollStore } from '~/stores/dailyPollStore';
    const dailyPollStore = useDailyPollStore();
    const selectedOption = ref<boolean | null>(null);
    const showResults = ref(false);
    const hasVoted = ref<boolean | null | undefined>(null);

    const pollResults = ref<{
        totalVotes: number;
        homeTeamVotes: number;
        homeTeamPercentage: number;
        awayTeamVotes: number;
        awayTeamPercentage: number;
    } | null | undefined>(null);

    const homeTeamName = computed(() => dailyPollStore.homeTeamName);
    const awayTeamName = computed(() => dailyPollStore.awayTeamName);

    const vote = async (votedForHomeTeam: boolean) => {
        if (hasVoted.value) return;

        const isForHomeTeam = votedForHomeTeam;
        const success = await dailyPollStore.submitVote(isForHomeTeam);

        if (success) {
            selectedOption.value = votedForHomeTeam;
            showResults.value = true;

            pollResults.value = await dailyPollStore.getPollResults();
            hasVoted.value = true;
        }
    };

    const teamAPercentage = computed(() =>
        pollResults.value ? pollResults.value.homeTeamPercentage : 50
    );
    const teamBPercentage = computed(() =>
        pollResults.value ? pollResults.value.awayTeamPercentage : 50
    );
    const totalVotes = computed(() =>
        pollResults.value ? pollResults.value.totalVotes : 0
    );

    onMounted(async () => {
        await dailyPollStore.getDailyPollTeams();
        pollResults.value = await dailyPollStore.getPollResults();
        hasVoted.value = await dailyPollStore.hasUserVoted();

        if (hasVoted.value !== null) {
            showResults.value = true;
            selectedOption.value = dailyPollStore.selectedTeamHomeOrAway;
        } else {
            showResults.value = false;
            selectedOption.value = null;
        }
    });
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Merriweather:wght@400;700;900&display=swap');

.font-merriweather-900 {
  font-family: 'Merriweather', sans-serif;
  font-weight: 900;
}
.font-merriweather-400 {
  font-family: 'Merriweather', sans-serif;
  font-weight: 400;
}

.poll-rectangle {
  position: relative;
  background-color: #f9fafb;
  height: 70px;
  overflow: hidden;
  transition: background-color 0.3s, width 0.3s;
  border-radius: 8px;
}

.team-bg {
  position: absolute;
  top: 0;
  bottom: 0;
  z-index: 1;
  transition: width 0.5s ease;
}

.team-a-bg {
  left: 0;
}

.team-b-bg {
  right: 0;
}

.divider {
  position: absolute;
  z-index: 1;
  width: 3.5px;
  background-color: #26547C;
}

.team-section {
  position: relative;
  padding: 16px 0;
  font-size: 16px;
  font-weight: bold;
  background-color: #26547C8b;
  transition: flex-grow 0.3s, background-color 0.3s;
  transition: width 0.5s ease;
}

.team-section.selected {
  background-color: #26547C8b;
  color: white;
}

.team-section:hover {
  background-color: #26547C;
}

.team-section.disabled {
  pointer-events: none;
  background-color: #26547C8b;
}

.team-section.disabled:hover {
  background-color: #26547C8b;
}

.team-name {
  position: absolute;
  z-index: 1;
  color: #f9fafb;
  font-size: 16px;
  font-weight: bold;
  padding: 8px;
}

.percentage-overlay {
  position: absolute;
  z-index: 2;
  color: #f9fafb;
  font-size: 14px;
  font-weight: bold;
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}
</style>