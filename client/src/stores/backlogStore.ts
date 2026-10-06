import { computed, ref } from 'vue'
import { defineStore } from 'pinia'
import {
  addBacklogEntry,
  deleteBacklogEntry,
  getBacklog,
  getBacklogEntry,
  updateBacklogEntry,
} from '@/services/backlogApi'
import { ApiError } from '@/types/api'
import type {
  AddBacklogEntryRequest,
  BacklogEntry,
  BacklogFilters,
  UpdateBacklogEntryRequest,
} from '@/types/backlog'

export const useBacklogStore = defineStore('backlog', () => {
  const entries = ref<BacklogEntry[]>([])
  const selectedEntry = ref<BacklogEntry | null>(null)
  const loading = ref(false)
  const saving = ref(false)
  const error = ref<string | null>(null)

  const totalGames = computed(() => entries.value.length)

  const playingGames = computed(() => entries.value.filter((entry) => entry.status === 'Playing'))

  const completedGames = computed(() =>
    entries.value.filter((entry) => entry.status === 'Completed'),
  )

  const backlogGames = computed(() => entries.value.filter((entry) => entry.status === 'Backlog'))

  const completionPercentage = computed(() => {
    const ownedGames = entries.value.filter((entry) => entry.status !== 'Wishlist')

    if (ownedGames.length === 0) {
      return 0
    }

    const completed = ownedGames.filter((entry) => entry.status === 'Completed').length

    return Math.round((completed / ownedGames.length) * 100)
  })

  async function fetchBacklog(filters: BacklogFilters = {}) {
    loading.value = true
    error.value = null

    try {
      entries.value = await getBacklog(filters)
    } catch (caughtError) {
      setError(caughtError, 'Could not load your backlog.')
    } finally {
      loading.value = false
    }
  }

  async function fetchEntry(id: number) {
    loading.value = true
    error.value = null
    selectedEntry.value = null

    try {
      selectedEntry.value = await getBacklogEntry(id)

      return selectedEntry.value
    } catch (caughtError) {
      selectedEntry.value = null
      setError(caughtError, 'Could not load the game.')
      throw caughtError
    } finally {
      loading.value = false
    }
  }

  async function addEntry(request: AddBacklogEntryRequest) {
    saving.value = true
    error.value = null

    try {
      const entry = await addBacklogEntry(request)
      entries.value.unshift(entry)
      selectedEntry.value = entry
      return entry
    } catch (caughtError) {
      setError(caughtError, 'Could not add the game to your backlog.')
      throw caughtError
    } finally {
      saving.value = false
    }
  }

  async function updateEntry(id: number, request: UpdateBacklogEntryRequest) {
    saving.value = true
    error.value = null

    try {
      await updateBacklogEntry(id, request)

      const refreshedEntry = await getBacklogEntry(id)

      const index = entries.value.findIndex((entry) => entry.id === id)

      if (index >= 0) {
        entries.value[index] = refreshedEntry
      }

      selectedEntry.value = refreshedEntry
      return refreshedEntry
    } catch (caughtError) {
      setError(caughtError, 'Could not update the backlog entry.')
      throw caughtError
    } finally {
      saving.value = false
    }
  }

  async function removeEntry(id: number) {
    saving.value = true
    error.value = null

    try {
      await deleteBacklogEntry(id)

      entries.value = entries.value.filter((entry) => entry.id !== id)

      if (selectedEntry.value?.id === id) {
        selectedEntry.value = null
      }
    } catch (caughtError) {
      setError(caughtError, 'Could not remove the backlog entry.')
      throw caughtError
    } finally {
      saving.value = false
    }
  }

  function clearError() {
    error.value = null
  }

  function setError(caughtError: unknown, fallbackMessage: string) {
    error.value = caughtError instanceof ApiError ? caughtError.message : fallbackMessage
  }

  return {
    entries,
    selectedEntry,
    loading,
    saving,
    error,
    totalGames,
    playingGames,
    completedGames,
    backlogGames,
    completionPercentage,
    fetchBacklog,
    fetchEntry,
    addEntry,
    updateEntry,
    removeEntry,
    clearError,
  }
})
