<script setup lang="ts">
import { ref } from 'vue'
import type { BacklogStatus } from '@/types/backlog'
import { backlogStatuses } from '@/types/backlog'
import type { CatalogGame } from '@/types/catalog'

defineProps<{
  game: CatalogGame
  importing?: boolean
}>()

const emit = defineEmits<{
  add: [
    payload: {
      rawgId: number
      status: BacklogStatus
    },
  ]
}>()

const selectedStatus = ref<BacklogStatus>('Backlog')
</script>

<template>
  <article class="overflow-hidden border border-slate-800 bg-slate-900 shadow-lg">
    <div class="aspect-video bg-slate-800">
      <img
        v-if="game.backgroundImageUrl"
        :src="game.backgroundImageUrl"
        :alt="`${game.name} cover artwork`"
        class="h-full w-full object-cover"
        loading="lazy"
      />

      <div v-else class="flex h-full items-center justify-center text-sm text-slate-500">
        No image available
      </div>
    </div>

    <div class="space-y-4 p-5">
      <div>
        <h2 class="text-lg font-bold text-white">
          {{ game.name }}
        </h2>

        <p class="mt-1 text-sm text-slate-400">
          {{ game.releasedOn ?? 'Release date unknown' }}
        </p>
      </div>

      <div class="flex flex-wrap gap-2">
        <span
          v-for="platform in game.platforms.slice(0, 3)"
          :key="platform"
          class="bg-slate-800 px-2.5 py-1 text-xs text-slate-300"
        >
          {{ platform }}
        </span>
      </div>

      <p class="text-sm text-slate-300">
        {{ game.genres.join(', ') || 'Genres unavailable' }}
      </p>

      <p v-if="game.metacriticScore !== null" class="text-sm text-emerald-400">
        Metacritic: {{ game.metacriticScore }}
      </p>

      <div class="space-y-2">
        <label :for="`status-${game.rawgId}`" class="block text-sm font-medium text-slate-300">
          Initial status
        </label>

        <select
          :id="`status-${game.rawgId}`"
          v-model="selectedStatus"
          class="w-full border border-slate-700 bg-slate-950 px-3 py-2 text-white"
        >
          <option v-for="status in backlogStatuses" :key="status" :value="status">
            {{ status }}
          </option>
        </select>
      </div>

      <button
        type="button"
        :disabled="importing"
        class="w-full bg-mauve-600 px-4 py-2.5 font-semibold text-white hover:bg-mauve-500 disabled:cursor-not-allowed disabled:opacity-50"
        @click="
          emit('add', {
            rawgId: game.rawgId,
            status: selectedStatus,
          })
        "
      >
        {{ importing ? 'Adding to backlog...' : 'Add to backlog' }}
      </button>

      <a
        :href="game.rawgUrl"
        target="_blank"
        rel="noopener noreferrer"
        class="block text-center text-sm text-mauve-400 hover:text-mauve-300 hover:underline"
      >
        View on RAWG
      </a>
    </div>
  </article>
</template>
