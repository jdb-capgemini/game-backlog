import { ref } from 'vue'
import { defineStore } from 'pinia'
import { searchCatalog } from '@/services/catalogApi'
import { ApiError } from '@/types/api'
import type { CatalogGame } from '@/types/catalog'

export const useCatalogStore = defineStore('catalog', () => {
  const games = ref<CatalogGame[]>([])
  const query = ref('')
  const page = ref(1)
  const pageSize = ref(12)
  const totalCount = ref(0)
  const loading = ref(false)
  const error = ref<string | null>(null)

  let activeController: AbortController | null = null

  async function search(searchQuery: string, requestedPage = 1) {
    const normalisedQuery = searchQuery.trim()

    if (normalisedQuery.length < 2) {
      games.value = []
      totalCount.value = 0
      error.value = 'Enter at least two characters to search.'
      return
    }

    activeController?.abort()
    activeController = new AbortController()

    query.value = normalisedQuery
    page.value = requestedPage
    loading.value = true
    error.value = null

    try {
      const result = await searchCatalog(
        {
          query: normalisedQuery,
          page: requestedPage,
          pageSize: pageSize.value,
        },
        activeController.signal,
      )

      games.value = result.games
      totalCount.value = result.count
    } catch (caughtError) {
      if (caughtError instanceof DOMException && caughtError.name === 'AbortError') {
        return
      }

      error.value =
        caughtError instanceof ApiError ? caughtError.message : 'The catalogue search failed.'
    } finally {
      if (!activeController.signal.aborted) {
        loading.value = false
      }
    }
  }

  function clear() {
    activeController?.abort()
    games.value = []
    query.value = ''
    page.value = 1
    totalCount.value = 0
    loading.value = false
    error.value = null
  }

  return {
    games,
    query,
    page,
    pageSize,
    totalCount,
    loading,
    error,
    search,
    clear,
  }
})
