<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import CatalogGameCard from '@/components/catalog/CatalogGameCard.vue'
import CatalogSearchForm from '@/components/catalog/CatalogSearchForm.vue'
import { useBacklogStore } from '@/stores/backlogStore'
import { useCatalogStore } from '@/stores/catalogStore'
import { ApiError } from '@/types/api'
import type { AddBacklogEntryRequest, BacklogStatus } from '@/types/backlog'

const router = useRouter()
const catalogStore = useCatalogStore()
const backlogStore = useBacklogStore()

const importingRawgId = ref<number | null>(null)
const pageMessage = ref<string | null>(null)

async function search(query: string) {
  pageMessage.value = null
  await catalogStore.search(query, 1)
}

async function addGame(payload: { rawgId: number; status: BacklogStatus }) {
  importingRawgId.value = payload.rawgId
  pageMessage.value = null

  const request: AddBacklogEntryRequest = payload

  try {
    const entry = await backlogStore.addEntry(request)

    await router.push({
      name: 'backlog-details',
      params: { id: entry.id },
    })
  } catch (error) {
    if (error instanceof ApiError && error.status === 409) {
      pageMessage.value = 'That game is already in your backlog.'
    } else {
      pageMessage.value = backlogStore.error ?? 'The game could not be added.'
    }
  } finally {
    importingRawgId.value = null
  }
}
</script>

<template>
  <section class="space-y-8">
    <header>
      <p class="text-sm font-semibold uppercase tracking-wider text-mauve-400">Catalogue</p>

      <h1 class="mt-2 text-3xl font-bold text-white">Add a game</h1>

      <p class="mt-2 max-w-2xl text-slate-400">
        Search the game catalogue, choose a result, and add it to your personal backlog.
      </p>
    </header>

    <CatalogSearchForm
      :initial-query="catalogStore.query"
      :loading="catalogStore.loading"
      @search="search"
    />

    <div
      v-if="pageMessage"
      role="alert"
      class="border border-muted-amber-500/30 bg-muted-amber-500/10 p-4 text-muted-amber-200"
    >
      {{ pageMessage }}
    </div>

    <div
      v-if="catalogStore.error"
      role="alert"
      class="border border-muted-rust-500/30 bg-muted-rust-500/10 p-4 text-muted-rust-200"
    >
      {{ catalogStore.error }}
    </div>

    <div v-if="catalogStore.loading" class="py-12 text-center text-slate-400">
      Searching the game catalogue...
    </div>

    <div
      v-else-if="catalogStore.query && catalogStore.games.length === 0 && !catalogStore.error"
      class="border border-slate-800 bg-slate-900 p-10 text-center"
    >
      <h2 class="text-lg font-semibold text-white">No games found</h2>

      <p class="mt-2 text-slate-400">Check the title or try a broader search.</p>
    </div>

    <div v-else-if="catalogStore.games.length > 0" class="grid gap-6 sm:grid-cols-2 xl:grid-cols-3">
      <CatalogGameCard
        v-for="game in catalogStore.games"
        :key="game.rawgId"
        :game="game"
        :importing="importingRawgId === game.rawgId"
        @add="addGame"
      />
    </div>
  </section>
</template>
