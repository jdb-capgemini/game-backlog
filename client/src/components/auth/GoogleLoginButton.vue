<script setup lang="ts">
import { signInWithGoogle } from '@/services/authApi'

const props = withDefaults(
  defineProps<{
    returnUrl?: string
    disabled?: boolean
  }>(),
  {
    returnUrl: '/',
    disabled: false,
  },
)

function signIn() {
  if (props.disabled) {
    return
  }

  signInWithGoogle(props.returnUrl)
}
</script>

<template>
  <button
    type="button"
    :disabled="disabled"
    class="inline-flex w-full items-center justify-center gap-3 border border-slate-700 bg-white px-5 py-3 font-semibold text-slate-900 transition hover:bg-slate-100 focus:outline-none focus:ring-2 focus:ring-mauve-600 focus:ring-offset-2 focus:ring-offset-slate-950 disabled:cursor-not-allowed disabled:opacity-50"
    @click="signIn"
  >
    <svg class="h-5 w-5" viewBox="0 0 24 24" aria-hidden="true">
      <path
        fill="#4285F4"
        d="M21.6 12.23c0-.71-.06-1.4-.18-2.07H12v3.92h5.38a4.6 4.6 0 01-2 3.02v2.54h3.24c1.9-1.75 2.98-4.33 2.98-7.41z"
      />
      <path
        fill="#34A853"
        d="M12 22c2.7 0 4.97-.9 6.62-2.36l-3.24-2.54c-.9.6-2.05.96-3.38.96-2.61 0-4.82-1.76-5.61-4.13H3.04v2.62A10 10 0 0012 22z"
      />
      <path
        fill="#FBBC05"
        d="M6.39 13.93A6 6 0 016.08 12c0-.67.11-1.32.31-1.93V7.45H3.04A10 10 0 002 12c0 1.61.38 3.14 1.04 4.55l3.35-2.62z"
      />
      <path
        fill="#EA4335"
        d="M12 5.94c1.47 0 2.79.5 3.83 1.5l2.87-2.87A9.65 9.65 0 0012 2a10 10 0 00-8.96 5.45l3.35 2.62C7.18 7.7 9.39 5.94 12 5.94z"
      />
    </svg>

    <span>Sign in with Google</span>
  </button>
</template>
