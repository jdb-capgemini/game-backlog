import { apiRequest } from '@/services/apiClient'
import type { CatalogSearchParameters, CatalogSearchResponse } from '@/types/catalog'

export function searchCatalog(parameters: CatalogSearchParameters, signal?: AbortSignal) {
  const query = new URLSearchParams({
    query: parameters.query.trim(),
    page: String(parameters.page ?? 1),
    pageSize: String(parameters.pageSize ?? 12),
  })

  return apiRequest<CatalogSearchResponse>(`/api/catalog/search?${query.toString()}`, { signal })
}
