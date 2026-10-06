import { apiRequest } from '@/services/apiClient'
import type { CurrentUser } from '@/types/auth'

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL

export function getCurrentUser() {
  return apiRequest<CurrentUser>('/api/auth/me')
}

export function signInWithGoogle(returnUrl = '/') {
  const apiBaseUrl = import.meta.env.VITE_API_BASE_URL

  const safeReturnUrl = returnUrl.startsWith('/') && !returnUrl.startsWith('//') ? returnUrl : '/'

  const query = new URLSearchParams({
    returnUrl: safeReturnUrl,
  })

  window.location.assign(`${apiBaseUrl}/api/auth/login?${query.toString()}`)
}

export function logout() {
  return apiRequest<void>('/api/auth/logout', {
    method: 'POST',
  })
}
