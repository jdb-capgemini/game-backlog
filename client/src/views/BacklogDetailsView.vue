<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { RouterLink, useRouter } from 'vue-router'
import BacklogStatusBadge from '@/components/backlog/BacklogStatusBadge.vue'
import { useBacklogStore } from '@/stores/backlogStore'
import { ApiError } from '@/types/api'

const props = defineProps<{
  id: number
}>()

const router = useRouter()
const backlogStore = useBacklogStore()

const deleting = ref(false)
const showDeleteConfirmation = ref(false)

const entry = computed(() => backlogStore.selectedEntry)

const formattedReleaseDate = computed(() => formatDate(entry.value?.releasedOn))

const formattedStartedDate = computed(() => formatDate(entry.value?.startedOn))

const formattedCompletedDate = computed(() => formatDate(entry.value?.completedOn))

const formattedCreatedDate = computed(() => formatDateTime(entry.value?.createdAt))

const formattedUpdatedDate = computed(() => formatDateTime(entry.value?.updatedAt))

onMounted(async () => {
  if (!Number.isInteger(props.id) || props.id <= 0) {
    await router.replace({ name: 'not-found' })
    return
  }

  try {
    await backlogStore.fetchEntry(props.id)
  } catch (error) {
    if (error instanceof ApiError && error.status === 404) {
      await router.replace({
        name: 'not-found',
        query: { reason: 'game-not-found' },
      })
    }
  }
})

async function deleteEntry() {
  deleting.value = true

  try {
    await backlogStore.removeEntry(props.id)

    await router.push({
      name: 'backlog',
      query: { deleted: 'true' },
    })
  } catch {
    showDeleteConfirmation.value = false
  } finally {
    deleting.value = false
  }
}

function formatDate(value: string | null | undefined) {
  if (!value) {
    return 'Not recorded'
  }

  const date = new Date(`${value}T00:00:00`)

  if (Number.isNaN(date.getTime())) {
    return 'Not recorded'
  }

  return new Intl.DateTimeFormat(undefined, {
    dateStyle: 'medium',
  }).format(date)
}

function formatDateTime(value: string | null | undefined) {
  if (!value) {
    return 'Not recorded'
  }

  const date = new Date(value)

  if (Number.isNaN(date.getTime())) {
    return 'Not recorded'
  }

  return new Intl.DateTimeFormat(undefined, {
    dateStyle: 'medium',
    timeStyle: 'short',
  }).format(date)
}
</script>

<template>
  <section class="space-y-6">
    <div v-if="backlogStore.loading" class="space-y-6" aria-label="Loading game details">
      <div class="h-72 animate-pulse bg-slate-900" />

      <div class="h-64 animate-pulse bg-slate-900" />
    </div>

    <div
      v-else-if="backlogStore.error && !entry"
      role="alert"
      class="border border-muted-rust-500/30 bg-muted-rust-500/10 p-8 text-center"
    >
      <h1 class="text-xl font-bold text-muted-rust-100">Could not load this game</h1>

      <p class="mt-2 text-muted-rust-200">
        {{ backlogStore.error }}
      </p>

      <div class="mt-6 flex flex-wrap justify-center gap-3">
        <button
          type="button"
          class="bg-muted-rust-500/20 px-4 py-2 font-semibold text-muted-rust-100 hover:bg-muted-rust-500/30"
          @click="backlogStore.fetchEntry(id)"
        >
          Try again
        </button>

        <RouterLink
          to="/backlog"
          class="bg-slate-800 px-4 py-2 font-semibold text-white hover:bg-slate-700"
        >
          Return to backlog
        </RouterLink>
      </div>
    </div>

    <template v-else-if="entry">
      <RouterLink
        to="/backlog"
        class="inline-flex text-sm font-medium text-mauve-400 hover:text-mauve-300"
      >
        ← Back to backlog
      </RouterLink>

      <article class="overflow-hidden border border-slate-800 bg-slate-900">
        <div class="relative min-h-72 bg-slate-800">
          <img
            v-if="entry.backgroundImageUrl"
            :src="entry.backgroundImageUrl"
            :alt="`${entry.name} artwork`"
            class="absolute inset-0 h-full w-full object-cover"
          />

          <div v-else class="absolute inset-0 flex items-center justify-center text-slate-500">
            No image available
          </div>

          <div
            class="absolute inset-0 bg-gradient-to-t from-slate-950 via-slate-950/60 to-transparent"
          />

          <div class="relative flex min-h-72 flex-col justify-end p-6 sm:p-8">
            <BacklogStatusBadge :status="entry.status" class="self-start" />

            <h1 class="mt-4 text-3xl font-bold text-white sm:text-4xl">
              {{ entry.name }}
            </h1>

            <p class="mt-2 text-slate-300">
              {{ entry.genres.join(', ') || 'Genres unavailable' }}
            </p>
          </div>
        </div>

        <div
          class="flex flex-col gap-3 border-t border-slate-800 p-5 sm:flex-row sm:items-center sm:justify-between"
        >
          <div class="flex flex-wrap gap-3">
            <RouterLink
              :to="{
                name: 'backlog-edit',
                params: { id: entry.id },
              }"
              class="bg-mauve-600 px-5 py-2.5 font-semibold text-white hover:bg-mauve-500"
            >
              Edit backlog entry
            </RouterLink>

            <a
              v-if="entry.rawgUrl"
              :href="entry.rawgUrl"
              target="_blank"
              rel="noopener noreferrer"
              class="bg-slate-800 px-5 py-2.5 font-semibold text-white hover:bg-slate-700"
            >
              View on RAWG
            </a>
          </div>

          <button
            type="button"
            class="px-4 py-2.5 font-semibold text-muted-rust-400 hover:bg-muted-rust-500/10 hover:text-muted-rust-300"
            @click="showDeleteConfirmation = true"
          >
            Remove from backlog
          </button>
        </div>
      </article>

      <div class="grid gap-6 lg:grid-cols-3">
        <section class="border border-slate-800 bg-slate-900 p-6 lg:col-span-2">
          <h2 class="text-xl font-bold text-white">Personal progress</h2>

          <dl class="mt-6 grid gap-6 sm:grid-cols-2">
            <div>
              <dt class="text-sm text-slate-400">Status</dt>

              <dd class="mt-2">
                <BacklogStatusBadge :status="entry.status" />
              </dd>
            </div>

            <div>
              <dt class="text-sm text-slate-400">Personal rating</dt>

              <dd class="mt-2 text-lg font-semibold text-white">
                {{ entry.personalRating === null ? 'Not rated' : `${entry.personalRating}/10` }}
              </dd>
            </div>

            <div>
              <dt class="text-sm text-slate-400">Estimated playtime</dt>

              <dd class="mt-2 text-lg font-semibold text-white">
                {{
                  entry.estimatedHours === null ? 'Not recorded' : `${entry.estimatedHours} hours`
                }}
              </dd>
            </div>

            <div>
              <dt class="text-sm text-slate-400">Started</dt>

              <dd class="mt-2 text-lg font-semibold text-white">
                {{ formattedStartedDate }}
              </dd>
            </div>

            <div>
              <dt class="text-sm text-slate-400">Completed</dt>

              <dd class="mt-2 text-lg font-semibold text-white">
                {{ formattedCompletedDate }}
              </dd>
            </div>
          </dl>

          <div class="mt-8 border-t border-slate-800 pt-6">
            <h3 class="font-semibold text-white">Personal notes</h3>

            <p v-if="entry.notes" class="mt-3 whitespace-pre-wrap leading-7 text-slate-300">
              {{ entry.notes }}
            </p>

            <p v-else class="mt-3 text-slate-500">You have not added any notes for this game.</p>
          </div>
        </section>

        <aside class="border border-slate-800 bg-slate-900 p-6">
          <h2 class="text-xl font-bold text-white">Game information</h2>

          <dl class="mt-6 space-y-5">
            <div>
              <dt class="text-sm text-slate-400">Release date</dt>

              <dd class="mt-1 font-medium text-white">
                {{ formattedReleaseDate }}
              </dd>
            </div>

            <div>
              <dt class="text-sm text-slate-400">Platforms</dt>

              <dd class="mt-2 flex flex-wrap gap-2">
                <span
                  v-for="platform in entry.platforms"
                  :key="platform"
                  class="bg-slate-800 px-2.5 py-1 text-xs text-slate-300"
                >
                  {{ platform }}
                </span>

                <span v-if="entry.platforms.length === 0" class="text-sm text-slate-500">
                  Not available
                </span>
              </dd>
            </div>

            <div>
              <dt class="text-sm text-slate-400">Genres</dt>

              <dd class="mt-2 flex flex-wrap gap-2">
                <span
                  v-for="genre in entry.genres"
                  :key="genre"
                  class="bg-slate-800 px-2.5 py-1 text-xs text-slate-300"
                >
                  {{ genre }}
                </span>

                <span v-if="entry.genres.length === 0" class="text-sm text-slate-500">
                  Not available
                </span>
              </dd>
            </div>

            <div>
              <dt class="text-sm text-slate-400">Metacritic score</dt>

              <dd class="mt-1 text-lg font-semibold text-emerald-300">
                {{ entry.metacriticScore ?? 'Not available' }}
              </dd>
            </div>

            <div>
              <dt class="text-sm text-slate-400">RAWG ID</dt>

              <dd class="mt-1 font-medium text-white">
                {{ entry.rawgId }}
              </dd>
            </div>
          </dl>

          <div class="mt-6 border-t border-slate-800 pt-6 text-xs leading-5 text-slate-500">
            <p>
              Game metadata and imagery are provided by
              <a
                href="https://rawg.io"
                target="_blank"
                rel="noopener noreferrer"
                class="inline-flex text-sm font-medium text-mauve-400 hover:text-mauve-300"
              >
                https://rawg.io RAWG </a
              >.
            </p>
          </div>
        </aside>
      </div>

      <section class="border border-slate-800 bg-slate-900 p-6">
        <h2 class="text-lg font-bold text-white">Backlog history</h2>

        <dl class="mt-4 grid gap-5 sm:grid-cols-2">
          <div>
            <dt class="text-sm text-slate-400">Added to backlog</dt>

            <dd class="mt-1 text-slate-200">
              {{ formattedCreatedDate }}
            </dd>
          </div>

          <div>
            <dt class="text-sm text-slate-400">Last updated</dt>

            <dd class="mt-1 text-slate-200">
              {{ formattedUpdatedDate }}
            </dd>
          </div>
        </dl>
      </section>

      <div
        v-if="backlogStore.error"
        role="alert"
        class="border border-muted-rust-500/30 bg-muted-rust-500/10 p-4 text-muted-rust-200"
      >
        {{ backlogStore.error }}
      </div>

      <div
        v-if="showDeleteConfirmation"
        class="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/80 p-4"
        role="presentation"
        @click.self="showDeleteConfirmation = false"
      >
        <section
          role="dialog"
          aria-modal="true"
          aria-labelledby="delete-dialog-title"
          aria-describedby="delete-dialog-description"
          class="w-full max-w-md border border-slate-700 bg-slate-900 p-6 shadow-2xl"
          @keydown.esc="showDeleteConfirmation = false"
        >
          <h2 id="delete-dialog-title" class="text-xl font-bold text-white">
            Remove {{ entry.name }}?
          </h2>

          <p id="delete-dialog-description" class="mt-3 leading-6 text-slate-400">
            This removes the game and your personal progress from the backlog. This action cannot be
            undone.
          </p>

          <div class="mt-6 flex justify-end gap-3">
            <button
              type="button"
              :disabled="deleting"
              class="bg-slate-800 px-4 py-2.5 font-semibold text-slate-200 hover:bg-slate-700 disabled:opacity-50"
              @click="showDeleteConfirmation = false"
            >
              Cancel
            </button>

            <button
              type="button"
              :disabled="deleting"
              class="bg-muted-rust-600 px-4 py-2.5 font-semibold text-white hover:bg-muted-rust-500 disabled:cursor-not-allowed disabled:opacity-50"
              @click="deleteEntry"
            >
              {{ deleting ? 'Removing...' : 'Remove game' }}
            </button>
          </div>
        </section>
      </div>
    </template>
  </section>
</template>
