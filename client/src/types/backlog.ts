export const backlogStatuses = [
  'Backlog',
  'Playing',
  'Completed',
  'Paused',
  'Dropped',
  'Wishlist',
] as const

export type BacklogStatus = (typeof backlogStatuses)[number]

export interface BacklogEntry {
  id: number
  rawgId: number
  name: string
  slug: string
  releasedOn: string | null
  backgroundImageUrl: string | null
  rawgUrl: string | null
  metacriticScore: number | null
  platforms: string[]
  genres: string[]
  status: BacklogStatus
  personalRating: number | null
  estimatedHours: number | null
  notes: string | null
  startedOn: string | null
  completedOn: string | null
  createdAt: string
  updatedAt: string
}

export interface AddBacklogEntryRequest {
  rawgId: number
  status: BacklogStatus
}

export interface UpdateBacklogEntryRequest {
  status: BacklogStatus
  personalRating: number | null
  estimatedHours: number | null
  notes: string | null
  startedOn: string | null
  completedOn: string | null
}

export interface BacklogFilters {
  search?: string
  status?: BacklogStatus
}
