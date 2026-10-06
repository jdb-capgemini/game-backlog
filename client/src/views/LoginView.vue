<script setup lang="ts">
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import GoogleLoginButton from '@/components/auth/GoogleLoginButton.vue'

const route = useRoute()

const returnUrl = computed(() => {
  const value = route.query.returnUrl

  if (typeof value === 'string' && value.startsWith('/') && !value.startsWith('//')) {
    return value
  }

  return '/'
})
</script>

<template>
  <section class="flex min-h-[70vh] items-center justify-center py-12">
    <div class="w-full max-w-md border border-slate-800 bg-slate-900 p-8 shadow-xl">
      <div class="text-center">
        <div
          class="mx-auto flex h-14 w-14 items-center justify-center bg-mauve-600/10 text-2xl"
          aria-hidden="true"
        >
          🎮
        </div>

        <h1 class="mt-5 text-3xl font-bold text-white">Welcome to Game Backlog</h1>

        <p class="mt-3 leading-6 text-slate-400">
          Sign in to manage your video game backlog, ratings, progress, and notes.
        </p>
      </div>

      <div class="mt-8">
        <GoogleLoginButton :return-url="returnUrl" />
      </div>

      <p class="mt-6 text-center text-xs leading-5 text-slate-500">
        Google is used only to verify your identity. This application does not receive your Google
        password.
      </p>
    </div>
  </section>
</template>
