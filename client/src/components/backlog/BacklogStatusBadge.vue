<script setup lang="ts">
import { computed } from 'vue'
import type { BacklogStatus } from '@/types/backlog'

const props = defineProps<{
  status: BacklogStatus
}>()

const statusClass = computed(() => {
  const classes: Record<BacklogStatus, string> = {
    Backlog: 'bg-slate-700 text-slate-200',
    Playing: 'bg-blue-500/20 text-blue-300',
    Completed: 'bg-emerald-500/20 text-emerald-300',
    Paused: 'bg-amber-500/20 text-amber-300',
    Dropped: 'bg-muted-rust-500/20 text-muted-rust-300',
    Wishlist: 'bg-mauve-600/20 text-mauve-300',
  }

  return classes[props.status]
})
</script>

<template>
  <span :class="statusClass" class="inline-flex px-2.5 py-1 text-xs font-semibold">
    {{ status }}
  </span>
</template>
