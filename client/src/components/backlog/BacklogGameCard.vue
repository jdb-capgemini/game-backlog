<script setup lang="ts">
import { RouterLink } from 'vue-router'
import BacklogStatusBadge from './BacklogStatusBadge.vue'
import type { BacklogEntry } from '@/types/backlog'

defineProps<{
  entry: BacklogEntry
}>()
</script>

<template>
  <article class="overflow-hidden border border-slate-800 bg-slate-900">
    <div class="aspect-video bg-slate-800">
      <img
        v-if="entry.backgroundImageUrl"
        :src="entry.backgroundImageUrl"
        :alt="`${entry.name} artwork`"
        class="h-full w-full object-cover"
        loading="lazy"
      />
    </div>

    <div class="space-y-4 p-5">
      <div class="flex items-start justify-between gap-3">
        <div>
          <h2 class="font-bold text-white">
            {{ entry.name }}
          </h2>

          <p class="mt-1 text-sm text-slate-400">
            {{ entry.platforms.join(', ') }}
          </p>
        </div>

        <BacklogStatusBadge :status="entry.status" />
      </div>

      <div class="flex items-center gap-4 text-sm">
        <span v-if="entry.personalRating !== null" class="text-amber-300">
          Your rating: {{ entry.personalRating }}/10
        </span>

        <span v-if="entry.metacriticScore !== null" class="text-emerald-300">
          Metacritic: {{ entry.metacriticScore }}
        </span>
      </div>

      <RouterLink
        :to="{
          name: 'backlog-details',
          params: { id: entry.id },
        }"
        class="block bg-slate-800 px-4 py-2.5 text-center font-medium text-white hover:bg-slate-700"
      >
        View details
      </RouterLink>

      <a
        v-if="entry.rawgUrl"
        :href="entry.rawgUrl"
        target="_blank"
        rel="noopener noreferrer"
        class="block text-center text-sm text-mauve-400 hover:underline"
      >
        View on RAWG
      </a>
    </div>
  </article>
</template>
