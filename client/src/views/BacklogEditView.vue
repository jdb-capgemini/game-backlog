<script setup lang="ts">
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'
import BacklogForm from '@/components/backlog/BacklogForm.vue'
import { useBacklogStore } from '@/stores/backlogStore'
import type { UpdateBacklogEntryRequest } from '@/types/backlog'

const props = defineProps<{
  id: number
}>()

const router = useRouter()
const backlogStore = useBacklogStore()

onMounted(async () => {
  if (!Number.isInteger(props.id) || props.id <= 0) {
    await router.replace({ name: 'not-found' })
    return
  }

  try {
    await backlogStore.fetchEntry(props.id)
  } catch {
    if (!backlogStore.selectedEntry) {
      await router.replace({ name: 'not-found' })
    }
  }
})

async function save(request: UpdateBacklogEntryRequest) {
  await backlogStore.updateEntry(props.id, request)

  await router.push({
    name: 'backlog-details',
    params: { id: props.id },
  })
}

function cancel() {
  void router.push({
    name: 'backlog-details',
    params: { id: props.id },
  })
}
</script>

<template>
  <section class="mx-auto max-w-3xl space-y-6">
    <div v-if="backlogStore.loading">Loading...</div>

    <template v-else-if="backlogStore.selectedEntry">
      <header>
        <p class="text-sm text-mauve-400">Edit personal backlog information</p>

        <h1 class="mt-2 text-3xl font-bold text-white">
          {{ backlogStore.selectedEntry.name }}
        </h1>

        <p class="mt-2 text-slate-400">
          Catalogue details come from RAWG and are not editable here.
        </p>
      </header>

      <div class="border border-slate-800 bg-slate-900 p-6">
        <BacklogForm
          :entry="backlogStore.selectedEntry"
          :saving="backlogStore.saving"
          @submit="save"
          @cancel="cancel"
        />
      </div>
    </template>
  </section>
</template>
