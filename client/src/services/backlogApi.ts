import { apiRequest } from '@/services/apiClient'
import type {
  AddBacklogEntryRequest,
  BacklogEntry,
  BacklogFilters,
  UpdateBacklogEntryRequest,
} from '@/types/backlog'

export function getBacklog(filters: BacklogFilters = {}) {
  const query = new URLSearchParams()

  if (filters.search?.trim()) {
    query.set('search', filters.search.trim())
  }

  if (filters.status) {
    query.set('status', filters.status)
  }

  const queryString = query.toString()

  return apiRequest<BacklogEntry[]>(`/api/backlog${queryString ? `?${queryString}` : ''}`)
}

export function getBacklogEntry(id: number) {
  return apiRequest<BacklogEntry>(`/api/backlog/${id}`)
}

export function addBacklogEntry(request: AddBacklogEntryRequest) {
  return apiRequest<BacklogEntry>('/api/backlog', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(request),
  })
}

export function updateBacklogEntry(id: number, request: UpdateBacklogEntryRequest) {
  return apiRequest<void>(`/api/backlog/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(request),
  })
}

export function deleteBacklogEntry(id: number) {
  return apiRequest<void>(`/api/backlog/${id}`, {
    method: 'DELETE',
  })
}
