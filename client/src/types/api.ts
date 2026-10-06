export interface ApiErrorResponse {
  message?: string
  detail?: string
  title?: string
  status?: number
  errors?: Record<string, string[]>
}

export class ApiError extends Error {
  status: number
  details?: ApiErrorResponse

  constructor(message: string, status: number, details?: ApiErrorResponse) {
    super(message)
    this.name = 'ApiError'
    this.status = status
    this.details = details
  }
}
