<script setup lang="ts">
import { ref } from 'vue'

const props = defineProps<{
  initialQuery?: string
  loading?: boolean
}>()

const emit = defineEmits<{
  search: [query: string]
}>()

const query = ref(props.initialQuery ?? '')

function submit() {
  const value = query.value.trim()

  if (value.length < 2) {
    return
  }

  emit('search', value)
}
</script>

<template>
  <form class="flex flex-col gap-3 sm:flex-row" @submit.prevent="submit">
    <div class="flex-1">
      <label for="game-search" class="sr-only"> Search for a video game </label>

      <input
        id="game-search"
        v-model="query"
        type="search"
        minlength="2"
        placeholder="Search for a game, for example Persona 5"
        class="w-full border border-slate-700 bg-slate-900 px-4 py-3 text-white placeholder:text-slate-500 focus:border-mauve-600 focus:ring-2 focus:ring-mauve-600/30"
      />
    </div>

    <button
      type="submit"
      :disabled="loading || query.trim().length < 2"
      class="bg-mauve-600 px-6 py-3 font-semibold text-white hover:bg-mauve-500 disabled:cursor-not-allowed disabled:opacity-50"
    >
      {{ loading ? 'Searching...' : 'Search' }}
    </button>
  </form>
</template>
