<script setup lang="ts">
import { computed } from 'vue'
import { RouterLink, useRoute } from 'vue-router'

const route = useRoute()

const missingGame = computed(() => route.query.reason === 'game-not-found')

const heading = computed(() => (missingGame.value ? 'Game not found' : 'Page not found'))

const description = computed(() =>
  missingGame.value
    ? 'This backlog entry may have been removed, or the address may be incorrect.'
    : 'The page you are looking for does not exist or may have moved.',
)
</script>

<template>
  <section class="flex min-h-[65vh] items-center justify-center py-12">
    <div class="max-w-xl text-center">
      <p class="text-sm font-semibold uppercase tracking-[0.3em] text-mauve-400">404</p>

      <h1 class="mt-4 text-4xl font-bold tracking-tight text-white sm:text-5xl">
        {{ heading }}
      </h1>

      <p class="mx-auto mt-5 max-w-md leading-7 text-slate-400">
        {{ description }}
      </p>

      <div class="mt-8 flex flex-col justify-center gap-3 sm:flex-row">
        <RouterLink
          to="/"
          class="bg-mauve-600 px-5 py-3 font-semibold text-white hover:bg-mauve-500"
        >
          Go to dashboard
        </RouterLink>

        <RouterLink
          to="/backlog"
          class="bg-slate-800 px-5 py-3 font-semibold text-slate-200 hover:bg-slate-700"
        >
          View backlog
        </RouterLink>
      </div>

      <div class="mt-10 border border-slate-800 bg-slate-900 p-5 text-left">
        <p class="text-sm font-medium text-slate-300">You can also:</p>

        <ul class="mt-3 list-inside list-disc space-y-2 text-sm text-slate-400">
          <li>Search the catalogue for a game</li>
          <li>Review your playing list</li>
          <li>Update a game's status or rating</li>
        </ul>

        <RouterLink
          to="/backlog/add"
          class="mt-5 inline-block text-sm font-semibold text-mauve-400 hover:text-mauve-300"
        >
          Search for a game
        </RouterLink>
      </div>
    </div>
  </section>
</template>
