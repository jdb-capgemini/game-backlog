<script setup lang="ts">
import { ref } from 'vue'
import { RouterLink, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'

const authStore = useAuthStore()
const router = useRouter()
const showProfileMenu = ref(false)

async function handleLogout() {
  try {
    await authStore.logout()
    showProfileMenu.value = false
    await router.push('/')
  } catch (error) {
    console.error('Logout failed:', error)
  }
}

function goToProfile() {
  showProfileMenu.value = false
  router.push('/profile')
}
</script>

<template>
  <header class="border-b border-slate-800 bg-gray-950">
    <div class="mx-auto flex max-w-7xl items-center justify-between px-4 py-4 sm:px-6 lg:px-8">
      <RouterLink to="/" class="text-xl font-bold text-white"> Game Backlog </RouterLink>

      <nav class="flex items-center gap-2">
        <RouterLink
          to="/"
          class="px-3 py-2 text-sm text-slate-300 hover:bg-mauve-800 hover:text-white"
        >
          Dashboard
        </RouterLink>

        <RouterLink
          to="/backlog"
          class="px-3 py-2 text-sm text-slate-300 hover:bg-mauve-800 hover:text-white"
        >
          Backlog
        </RouterLink>

        <RouterLink
          to="/backlog/add"
          class="bg-mauve-600 px-4 py-2 text-sm font-semibold text-white hover:bg-mauve-500"
        >
          Add game
        </RouterLink>

        <!-- Profile dropdown -->
        <div v-if="authStore.isAuthenticated" class="relative">
          <button
            @click="showProfileMenu = !showProfileMenu"
            class="overflow-hidden h-10 w-10 flex items-center justify-center hover:ring-2 hover:ring-mauve-500 transition-all"
          >
            <img
              v-if="authStore.user?.pictureUrl"
              :src="authStore.user.pictureUrl"
              :alt="authStore.user.displayName"
              class="h-full w-full object-cover"
            />
            <div
              v-else
              class="h-full w-full bg-gradient-to-br from-mauve-500 to-mauve-600 flex items-center justify-center"
            >
              <span class="text-white font-bold text-sm">
                {{ authStore.user?.displayName?.charAt(0)?.toUpperCase() || 'U' }}
              </span>
            </div>
          </button>

          <!-- Dropdown menu -->
          <div
            v-if="showProfileMenu"
            class="absolute right-0 mt-2 w-48 bg-slate-900 shadow-lg border border-slate-700 z-50"
          >
            <button
              @click="goToProfile"
              class="w-full text-left px-4 py-2 text-slate-300 hover:bg-slate-800 hover:text-white transition-colors"
            >
              My Profile
            </button>
            <button
              @click="handleLogout"
              class="w-full text-left px-4 py-2 text-slate-300 hover:bg-slate-800 hover:text-muted-rust-400 transition-colors"
            >
              Logout
            </button>
          </div>

          <!-- Close menu when clicking outside -->
          <div v-if="showProfileMenu" @click="showProfileMenu = false" class="fixed inset-0 z-40" />
        </div>
      </nav>
    </div>
  </header>
</template>
