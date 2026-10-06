import { ApiError, type ApiErrorResponse } from '@/types/api'

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL

if (!apiBaseUrl) {
  throw new Error('VITE_API_BASE_URL has not been configured.')
}

interface ApiRequestOptions extends RequestInit {
  signal?: AbortSignal
}

export async function apiRequest<T>(path: string, options: ApiRequestOptions = {}): Promise<T> {
  let response: Response

  try {
    response = await fetch(`${apiBaseUrl}${path}`, {
      ...options,
      credentials: 'include',
      headers: {
        Accept: 'application/json',
        ...options.headers,
      },
    })
  } catch {
    throw new ApiError('Could not connect to the Game Backlog API.', 0)
  }

  const isJson = response.headers.get('content-type')?.includes('application/json')

  const body = isJson ? await response.json() : undefined

  if (!response.ok) {
    const details = body as ApiErrorResponse | undefined

    if (response.status === 401) {
      throw new ApiError('You need to sign in to continue.', 401, details)
    }

    throw new ApiError(
      details?.message ?? details?.title ?? `The request failed with status ${response.status}.`,
      response.status,
      details,
    )
  }

  return body as T
}
