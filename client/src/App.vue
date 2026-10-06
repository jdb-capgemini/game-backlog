<script setup lang="ts">
import { onMounted } from 'vue'
import { RouterView } from 'vue-router'
import AppHeader from '@/components/layout/AppHeader.vue'
import AppFooter from '@/components/layout/AppFooter.vue'
import { useAuthStore } from '@/stores/authStore'

const authStore = useAuthStore()

onMounted(() => {
  authStore.initialize()
})
</script>

<template>
  <div class="min-h-screen bg-gray-950 text-slate-100">
    <AppHeader />

    <main class="mx-auto w-full max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
      <RouterView />
    </main>

    <AppFooter />
  </div>
</template>
