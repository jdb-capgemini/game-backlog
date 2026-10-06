import { computed, ref } from 'vue'
import { defineStore } from 'pinia'
import { getCurrentUser, logout as logoutRequest, signInWithGoogle } from '@/services/authApi'
import { ApiError } from '@/types/api'
import type { CurrentUser } from '@/types/auth'

export const useAuthStore = defineStore('auth', () => {
  const user = ref<CurrentUser | null>(null)
  const loading = ref(false)
  const initialized = ref(false)
  const error = ref<string | null>(null)

  const isAuthenticated = computed(() => user.value !== null)

  /**
   * Checks the current session cookie through /api/auth/me.
   *
   * Returns true when authenticated and false when anonymous.
   */
  async function initialize(): Promise<boolean> {
    // Avoid repeatedly calling /api/auth/me.
    if (initialized.value) {
      return isAuthenticated.value
    }

    loading.value = true
    error.value = null

    try {
      user.value = await getCurrentUser()
      return true
    } catch (caughtError) {
      user.value = null

      // A 401 is the expected signed-out state.
      if (caughtError instanceof ApiError && caughtError.status === 401) {
        return false
      }

      error.value =
        caughtError instanceof ApiError
          ? caughtError.message
          : 'Could not check your authentication status.'

      return false
    } finally {
      initialized.value = true
      loading.value = false
    }
  }

  function login(returnUrl = '/') {
    signInWithGoogle(returnUrl)
  }

  async function logout() {
    loading.value = true
    error.value = null

    try {
      await logoutRequest()

      user.value = null

      // We know the user is signed out, so this remains true.
      initialized.value = true
    } catch (caughtError) {
      error.value = caughtError instanceof ApiError ? caughtError.message : 'Could not sign out.'

      throw caughtError
    } finally {
      loading.value = false
    }
  }

  /**
   * Forces the next route navigation to check /api/auth/me again.
   */
  function reset() {
    user.value = null
    initialized.value = false
    error.value = null
  }

  return {
    user,
    loading,
    initialized,
    error,
    isAuthenticated,
    initialize,
    login,
    logout,
    reset,
  }
})
