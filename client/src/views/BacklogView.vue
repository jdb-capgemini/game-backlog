<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { RouterLink } from 'vue-router'
import BacklogGameCard from '@/components/backlog/BacklogGameCard.vue'
import { useBacklogStore } from '@/stores/backlogStore'
import { backlogStatuses, type BacklogStatus } from '@/types/backlog'

const backlogStore = useBacklogStore()

const search = ref('')
const status = ref<BacklogStatus | ''>('')

async function loadBacklog() {
  await backlogStore.fetchBacklog({
    search: search.value || undefined,
    status: status.value || undefined,
  })
}

function resetFilters() {
  search.value = ''
  status.value = ''
  void loadBacklog()
}

onMounted(loadBacklog)
</script>

<template>
  <section class="space-y-8">
    <header class="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
      <div>
        <h1 class="text-3xl font-bold text-white">My backlog</h1>

        <p class="mt-2 text-slate-400">Track the games you want to play and complete.</p>
      </div>

      <RouterLink
        to="/backlog/add"
        class="bg-mauve-600 px-5 py-3 text-center font-semibold text-white hover:bg-mauve-500"
      >
        Add game
      </RouterLink>
    </header>

    <form
      class="grid gap-4 border border-slate-800 bg-slate-900 p-5 sm:grid-cols-[1fr_220px_auto]"
      @submit.prevent="loadBacklog"
    >
      <div>
        <label for="backlog-search" class="mb-1 block text-sm text-slate-300"> Search </label>

        <input
          id="backlog-search"
          v-model="search"
          type="search"
          placeholder="Search saved games"
          class="w-full border border-slate-700 bg-slate-950 px-3 py-2 text-white"
        />
      </div>

      <div>
        <label for="status-filter" class="mb-1 block text-sm text-slate-300"> Status </label>

        <select
          id="status-filter"
          v-model="status"
          class="w-full border border-slate-700 bg-slate-950 px-3 py-2 text-white"
        >
          <option value="">All statuses</option>

          <option v-for="item in backlogStatuses" :key="item" :value="item">
            {{ item }}
          </option>
        </select>
      </div>

      <div class="flex items-end gap-2">
        <button
          type="submit"
          class="bg-mauve-600 px-4 py-2 font-semibold text-white hover:bg-mauve-500"
        >
          Apply
        </button>

        <button
          type="button"
          class="bg-slate-800 px-4 py-2 text-slate-200 hover:bg-slate-700"
          @click="resetFilters"
        >
          Reset
        </button>
      </div>
    </form>

    <div
      v-if="backlogStore.error"
      role="alert"
      class="border border-muted-rust-500/30 bg-muted-rust-500/10 p-4 text-muted-rust-200"
    >
      {{ backlogStore.error }}
    </div>

    <div v-if="backlogStore.loading" class="py-12 text-center text-slate-400">
      Loading your backlog...
    </div>

    <div
      v-else-if="backlogStore.entries.length === 0"
      class="border border-slate-800 bg-slate-900 p-12 text-center"
    >
      <h2 class="text-xl font-semibold text-white">Your backlog is empty</h2>

      <p class="mt-2 text-slate-400">Search the catalogue and add your first game.</p>

      <RouterLink
        to="/backlog/add"
        class="mt-6 inline-block bg-mauve-600 px-5 py-3 font-semibold text-white hover:bg-mauve-500"
      >
        Search for a game
      </RouterLink>
    </div>

    <div v-else class="grid gap-6 sm:grid-cols-2 xl:grid-cols-3">
      <BacklogGameCard v-for="entry in backlogStore.entries" :key="entry.id" :entry="entry" />
    </div>
  </section>
</template>
