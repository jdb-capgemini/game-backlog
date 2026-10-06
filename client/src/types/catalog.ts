export interface CatalogGame {
  rawgId: number
  name: string
  slug: string
  releasedOn: string | null
  backgroundImageUrl: string | null
  metacriticScore: number | null
  platforms: string[]
  genres: string[]
  rawgUrl: string
}

export interface CatalogSearchResponse {
  count: number
  page: number
  pageSize: number
  games: CatalogGame[]
}

export interface CatalogSearchParameters {
  query: string
  page?: number
  pageSize?: number
}
