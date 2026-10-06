<script setup lang="ts">
import { computed, onMounted } from 'vue'
import { RouterLink } from 'vue-router'
import BacklogGameCard from '@/components/backlog/BacklogGameCard.vue'
import { useBacklogStore } from '@/stores/backlogStore'

const backlogStore = useBacklogStore()

const wishlistCount = computed(
  () => backlogStore.entries.filter((entry) => entry.status === 'Wishlist').length,
)

const pausedCount = computed(
  () => backlogStore.entries.filter((entry) => entry.status === 'Paused').length,
)

const averageRating = computed(() => {
  const ratedEntries = backlogStore.entries.filter((entry) => entry.personalRating !== null)

  if (ratedEntries.length === 0) {
    return null
  }

  const totalRating = ratedEntries.reduce((total, entry) => total + (entry.personalRating ?? 0), 0)

  return (totalRating / ratedEntries.length).toFixed(1)
})

const recentlyUpdatedGames = computed(() =>
  [...backlogStore.entries]
    .sort(
      (first, second) => new Date(second.updatedAt).getTime() - new Date(first.updatedAt).getTime(),
    )
    .slice(0, 3),
)

const statusBreakdown = computed(() => [
  {
    label: 'Completed',
    count: backlogStore.completedGames.length,
    colour: 'bg-emerald-500',
  },
  {
    label: 'Playing',
    count: backlogStore.playingGames.length,
    colour: 'bg-blue-500',
  },
  {
    label: 'Backlog',
    count: backlogStore.backlogGames.length,
    colour: 'bg-slate-500',
  },
  {
    label: 'Paused',
    count: pausedCount.value,
    colour: 'bg-amber-500',
  },
  {
    label: 'Wishlist',
    count: wishlistCount.value,
    colour: 'bg-mauve-600',
  },
])

function statusPercentage(count: number) {
  if (backlogStore.totalGames === 0) {
    return 0
  }

  return Math.round((count / backlogStore.totalGames) * 100)
}

onMounted(async () => {
  await backlogStore.fetchBacklog()
})
</script>

<template>
  <section class="space-y-10">
    <header class="flex flex-col gap-5 sm:flex-row sm:items-end sm:justify-between">
      <div>
        <p class="text-sm font-semibold uppercase tracking-wider text-mauve-400">Overview</p>

        <h1 class="mt-2 text-3xl font-bold tracking-tight text-white sm:text-4xl">
          Your gaming dashboard
        </h1>

        <p class="mt-3 max-w-2xl text-slate-400">
          Track what you are playing, what you have completed, and what is still waiting in your
          backlog.
        </p>
      </div>

      <RouterLink
        to="/backlog/add"
        class="inline-flex items-center justify-center bg-mauve-600 px-5 py-3 font-semibold text-white transition hover:bg-mauve-500 focus:outline-none focus:ring-2 focus:ring-mauve-400 focus:ring-offset-2 focus:ring-offset-slate-950"
      >
        Add a game
      </RouterLink>
    </header>

    <div
      v-if="backlogStore.error"
      role="alert"
      class="border border-muted-rust-500/30 bg-muted-rust-500/10 p-4 text-muted-rust-200"
    >
      <p class="font-semibold">Could not load your dashboard</p>

      <p class="mt-1 text-sm">
        {{ backlogStore.error }}
      </p>

      <button
        type="button"
        class="mt-4 bg-muted-rust-500/20 px-4 py-2 text-sm font-semibold text-muted-rust-100 hover:bg-muted-rust-500/30"
        @click="backlogStore.fetchBacklog()"
      >
        Try again
      </button>
    </div>

    <div
      v-if="backlogStore.loading"
      class="grid gap-4 sm:grid-cols-2 xl:grid-cols-4"
      aria-label="Loading dashboard"
    >
      <div
        v-for="index in 4"
        :key="index"
        class="h-32 animate-pulse border border-slate-800 bg-slate-900"
      />
    </div>

    <template v-else>
      <div
        v-if="backlogStore.totalGames === 0"
        class="border border-slate-800 bg-mauve-900 px-6 py-14 text-center"
      >
        <div
          class="mx-auto flex h-14 w-14 items-center justify-center bg-mauve-600/10 text-2xl"
          aria-hidden="true"
        >
          🎮
        </div>

        <h2 class="mt-5 text-xl font-semibold text-white">Your backlog is empty</h2>

        <p class="mx-auto mt-2 max-w-md text-slate-400">
          Search the RAWG catalogue and add your first game to begin tracking your progress.
        </p>

        <RouterLink
          to="/backlog/add"
          class="mt-6 inline-flex bg-mauve-600 px-5 py-3 font-semibold text-white hover:bg-mauve-500"
        >
          Search for a game
        </RouterLink>
      </div>

      <template v-else>
        <div class="grid gap-4 sm:grid-cols-2 xl:grid-cols-4">
          <article class="border border-slate-800 bg-slate-900 p-5">
            <p class="text-sm font-medium text-slate-400">Total games</p>

            <p class="mt-3 text-3xl font-bold text-white">
              {{ backlogStore.totalGames }}
            </p>

            <RouterLink
              to="/backlog"
              class="mt-4 inline-block text-sm font-medium text-mauve-400 hover:text-mauve-300"
            >
              View backlog
            </RouterLink>
          </article>

          <article class="border border-blue-500/20 bg-blue-500/10 p-5">
            <p class="text-sm font-medium text-blue-200">Currently playing</p>

            <p class="mt-3 text-3xl font-bold text-white">
              {{ backlogStore.playingGames.length }}
            </p>

            <p class="mt-4 text-sm text-blue-200/70">Games in progress</p>
          </article>

          <article class="border border-emerald-500/20 bg-emerald-500/10 p-5">
            <p class="text-sm font-medium text-emerald-200">Completed</p>

            <p class="mt-3 text-3xl font-bold text-white">
              {{ backlogStore.completedGames.length }}
            </p>

            <p class="mt-4 text-sm text-emerald-200/70">
              {{ backlogStore.completionPercentage }}% completion rate
            </p>
          </article>

          <article class="border border-amber-500/20 bg-amber-500/10 p-5">
            <p class="text-sm font-medium text-amber-200">Average rating</p>

            <p class="mt-3 text-3xl font-bold text-white">
              {{ averageRating === null ? 'Not rated' : `${averageRating}/10` }}
            </p>

            <p class="mt-4 text-sm text-amber-200/70">Based on your ratings</p>
          </article>
        </div>

        <div class="grid gap-6 lg:grid-cols-2">
          <article class="border border-slate-800 bg-slate-900 p-6">
            <div>
              <h2 class="text-xl font-bold text-white">Completion progress</h2>

              <p class="mt-1 text-sm text-slate-400">
                Wishlist games are excluded from this calculation.
              </p>
            </div>

            <div class="mt-6">
              <div class="mb-2 flex items-center justify-between text-sm">
                <span class="text-slate-300"> Overall completion </span>

                <span class="font-semibold text-white">
                  {{ backlogStore.completionPercentage }}%
                </span>
              </div>

              <div
                class="h-3 overflow-hidden bg-slate-800"
                role="progressbar"
                :aria-valuenow="backlogStore.completionPercentage"
                aria-valuemin="0"
                aria-valuemax="100"
                aria-label="Backlog completion percentage"
              >
                <div
                  class="h-full bg-emerald-500 transition-all"
                  :style="{
                    width: `${backlogStore.completionPercentage}%`,
                  }"
                />
              </div>
            </div>
          </article>

          <article class="border border-slate-800 bg-slate-900 p-6">
            <h2 class="text-xl font-bold text-white">Status breakdown</h2>

            <div class="mt-6 space-y-4">
              <div v-for="item in statusBreakdown" :key="item.label">
                <div class="mb-1.5 flex items-center justify-between text-sm">
                  <span class="text-slate-300">
                    {{ item.label }}
                  </span>

                  <span class="font-medium text-white">
                    {{ item.count }}
                  </span>
                </div>

                <div class="h-2 overflow-hidden bg-slate-800">
                  <div
                    :class="item.colour"
                    class="h-full"
                    :style="{
                      width: `${statusPercentage(item.count)}%`,
                    }"
                  />
                </div>
              </div>
            </div>
          </article>
        </div>

        <section class="space-y-5">
          <div class="flex items-center justify-between gap-4">
            <div>
              <h2 class="text-2xl font-bold text-white">Recently updated</h2>

              <p class="mt-1 text-sm text-slate-400">Your latest backlog activity.</p>
            </div>

            <RouterLink
              to="/backlog"
              class="text-sm font-semibold text-mauve-400 hover:text-mauve-300"
            >
              View all
            </RouterLink>
          </div>

          <div class="grid gap-6 sm:grid-cols-2 xl:grid-cols-3">
            <BacklogGameCard v-for="entry in recentlyUpdatedGames" :key="entry.id" :entry="entry" />
          </div>
        </section>
      </template>
    </template>
  </section>
</template>
