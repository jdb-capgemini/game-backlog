<script setup lang="ts">
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'

const router = useRouter()
const authStore = useAuthStore()

const initials = computed(() => {
  const name = authStore.user?.displayName || authStore.user?.email || 'User'

  return name
    .split(/\s+/)
    .filter(Boolean)
    .slice(0, 2)
    .map((part) => part.charAt(0).toUpperCase())
    .join('')
})

async function handleLogout() {
  try {
    await authStore.logout()

    await router.replace({
      name: 'login',
    })
  } catch {
    // The store already exposes the error to the template.
  }
}
</script>

<template>
  <section class="mx-auto max-w-3xl space-y-8">
    <header>
      <p class="text-sm font-semibold uppercase tracking-wider text-mauve-400">Account</p>

      <h1 class="mt-2 text-3xl font-bold text-white">My profile</h1>

      <p class="mt-2 text-slate-400">View the Google account connected to your game backlog.</p>
    </header>

    <div v-if="authStore.loading && !authStore.user" class="space-y-6" aria-label="Loading profile">
      <div class="h-64 animate-pulse border border-slate-800 bg-slate-900" />
    </div>

    <div
      v-else-if="authStore.error && !authStore.user"
      role="alert"
      class="border border-muted-rust-500/30 bg-muted-rust-500/10 p-6"
    >
      <h2 class="font-semibold text-muted-rust-100">Could not load your profile</h2>

      <p class="mt-2 text-sm text-muted-rust-200">
        {{ authStore.error }}
      </p>

      <button
        type="button"
        class="mt-5 bg-muted-rust-500/20 px-4 py-2 font-semibold text-muted-rust-100 hover:bg-muted-rust-500/30"
        @click="authStore.initialize()"
      >
        Try again
      </button>
    </div>

    <template v-else-if="authStore.user">
      <article class="overflow-hidden border border-slate-800 bg-slate-900 shadow-xl">
        <div class="h-28 bg-gradient-to-r from-mauve-700 via-mauve-600 to-mauve-500" />

        <div class="px-6 pb-7 sm:px-8">
          <div class="-mt-12 flex flex-col gap-5 sm:flex-row sm:items-end">
            <div
              class="flex h-24 w-24 shrink-0 items-center justify-center overflow-hidden border-4 border-slate-900 bg-mauve-600 text-2xl font-bold text-white shadow-lg"
            >
              <img
                v-if="authStore.user.pictureUrl"
                :src="authStore.user.pictureUrl"
                :alt="`${authStore.user.displayName} profile picture`"
                class="h-full w-full object-cover"
                referrerpolicy="no-referrer"
              />

              <span v-else aria-hidden="true">
                {{ initials }}
              </span>
            </div>

            <div class="min-w-0 pb-1">
              <h2 class="truncate text-2xl font-bold text-white">
                {{ authStore.user.displayName }}
              </h2>

              <p class="mt-1 truncate text-sm text-slate-400">
                {{ authStore.user.email }}
              </p>
            </div>
          </div>

          <dl class="mt-8 divide-y divide-slate-800 border-y border-slate-800">
            <div class="grid gap-1 py-5 sm:grid-cols-[180px_1fr]">
              <dt class="text-sm font-medium text-slate-400">Display name</dt>

              <dd class="break-words text-slate-100">
                {{ authStore.user.displayName }}
              </dd>
            </div>

            <div class="grid gap-1 py-5 sm:grid-cols-[180px_1fr]">
              <dt class="text-sm font-medium text-slate-400">Email address</dt>

              <dd class="break-words text-slate-100">
                {{ authStore.user.email }}
              </dd>
            </div>

            <div class="grid gap-1 py-5 sm:grid-cols-[180px_1fr]">
              <dt class="text-sm font-medium text-slate-400">Sign-in provider</dt>

              <dd class="text-slate-100">Google</dd>
            </div>
          </dl>

          <p class="mt-5 text-sm leading-6 text-slate-500">
            Your name, email address, and profile picture come from your Google account. This
            application does not receive or store your Google password.
          </p>
        </div>
      </article>

      <section class="border border-muted-rust-500/20 bg-muted-rust-500/5 p-6">
        <h2 class="text-lg font-bold text-white">Sign out</h2>

        <p class="mt-2 text-sm leading-6 text-slate-400">
          This signs you out of the Game Backlog application. It does not sign you out of Google on
          other websites.
        </p>

        <div
          v-if="authStore.error"
          role="alert"
          class="mt-4 bg-muted-rust-500/10 p-3 text-sm text-muted-rust-200"
        >
          {{ authStore.error }}
        </div>

        <button
          type="button"
          :disabled="authStore.loading"
          class="mt-5 bg-muted-rust-600 px-5 py-2.5 font-semibold text-white hover:bg-muted-rust-500 disabled:cursor-not-allowed disabled:opacity-50"
          @click="handleLogout"
        >
          {{ authStore.loading ? 'Signing out...' : 'Log out' }}
        </button>
      </section>
    </template>
  </section>
</template>
