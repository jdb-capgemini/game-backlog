<script setup lang="ts">
import { reactive } from 'vue'
import { backlogStatuses, type BacklogEntry, type UpdateBacklogEntryRequest } from '@/types/backlog'

const props = defineProps<{
  entry: BacklogEntry
  saving?: boolean
}>()

const emit = defineEmits<{
  submit: [request: UpdateBacklogEntryRequest]
  cancel: []
}>()

const form = reactive<UpdateBacklogEntryRequest>({
  status: props.entry.status,
  personalRating: props.entry.personalRating,
  estimatedHours: props.entry.estimatedHours,
  notes: props.entry.notes,
  startedOn: props.entry.startedOn,
  completedOn: props.entry.completedOn,
})

const errors = reactive<Record<string, string>>({})

function validate() {
  Object.keys(errors).forEach((key) => delete errors[key])

  if (form.personalRating !== null && (form.personalRating < 1 || form.personalRating > 10)) {
    errors.personalRating = 'Rating must be between 1 and 10.'
  }

  if (form.estimatedHours !== null && form.estimatedHours < 0) {
    errors.estimatedHours = 'Estimated hours cannot be negative.'
  }

  if (form.startedOn && form.completedOn && form.completedOn < form.startedOn) {
    errors.completedOn = 'Completion date cannot be before start date.'
  }

  return Object.keys(errors).length === 0
}

function submit() {
  if (!validate()) {
    return
  }

  emit('submit', {
    ...form,
    notes: form.notes?.trim() || null,
  })
}
</script>

<template>
  <form class="space-y-6" @submit.prevent="submit">
    <div>
      <label for="status" class="mb-1 block text-sm font-medium text-slate-300"> Status </label>

      <select
        id="status"
        v-model="form.status"
        class="w-full border border-slate-700 bg-slate-950 px-3 py-2 text-white"
      >
        <option v-for="status in backlogStatuses" :key="status" :value="status">
          {{ status }}
        </option>
      </select>
    </div>

    <div class="grid gap-6 sm:grid-cols-2">
      <div>
        <label for="personal-rating" class="mb-1 block text-sm font-medium text-slate-300">
          Personal rating
        </label>

        <input
          id="personal-rating"
          v-model.number="form.personalRating"
          type="number"
          min="1"
          max="10"
          class="w-full border border-slate-700 bg-slate-950 px-3 py-2 text-white"
        />

        <p v-if="errors.personalRating" class="mt-1 text-sm text-muted-rust-400">
          {{ errors.personalRating }}
        </p>
      </div>

      <div>
        <label for="estimated-hours" class="mb-1 block text-sm font-medium text-slate-300">
          Estimated hours
        </label>

        <input
          id="estimated-hours"
          v-model.number="form.estimatedHours"
          type="number"
          min="0"
          step="0.5"
          class="w-full border border-slate-700 bg-slate-950 px-3 py-2 text-white"
        />

        <p v-if="errors.estimatedHours" class="mt-1 text-sm text-muted-rust-400">
          {{ errors.estimatedHours }}
        </p>
      </div>
    </div>

    <div class="grid gap-6 sm:grid-cols-2">
      <div>
        <label for="started-on" class="mb-1 block text-sm font-medium text-slate-300">
          Date started
        </label>

        <input
          id="started-on"
          v-model="form.startedOn"
          type="date"
          class="w-full border border-slate-700 bg-slate-950 px-3 py-2 text-white"
        />
      </div>

      <div>
        <label for="completed-on" class="mb-1 block text-sm font-medium text-slate-300">
          Date completed
        </label>

        <input
          id="completed-on"
          v-model="form.completedOn"
          type="date"
          class="w-full border border-slate-700 bg-slate-950 px-3 py-2 text-white"
        />

        <p v-if="errors.completedOn" class="mt-1 text-sm text-muted-rust-400">
          {{ errors.completedOn }}
        </p>
      </div>
    </div>

    <div>
      <label for="notes" class="mb-1 block text-sm font-medium text-slate-300"> Notes </label>

      <textarea
        id="notes"
        v-model="form.notes"
        rows="6"
        maxlength="4000"
        class="w-full border border-slate-700 bg-slate-950 px-3 py-2 text-white"
      />
    </div>

    <div class="flex justify-end gap-3">
      <button
        type="button"
        class="bg-slate-800 px-5 py-2.5 text-slate-200 hover:bg-slate-700"
        @click="emit('cancel')"
      >
        Cancel
      </button>

      <button
        type="submit"
        :disabled="saving"
        class="bg-mauve-600 px-5 py-2.5 font-semibold text-white hover:bg-mauve-500 disabled:opacity-50"
      >
        {{ saving ? 'Saving...' : 'Save changes' }}
      </button>
    </div>
  </form>
</template>
