<script setup lang="ts">
import { ref, watch } from 'vue'
import { api, errorMessage } from '@/api'
import { formatDate, statusLabel } from '@/format'
import { priorities, type HistoryEntry, type TicketDetails, type TicketPriority, type TicketStatus } from '@/types'
import StatusBadge from '@/components/StatusBadge.vue'
import PriorityBadge from '@/components/PriorityBadge.vue'

// Comes from the URL /tickets/:id (see router/index.ts).
const props = defineProps<{ id: string }>()

const ticket = ref<TicketDetails | null>(null)
const history = ref<HistoryEntry[]>([])
const loading = ref(true)
const error = ref('')
const busy = ref(false)
const newComment = ref('')

async function load() {
  error.value = ''
  try {
    // Two independent requests, so run them in parallel.
    const [loadedTicket, loadedHistory] = await Promise.all([
      api.getTicket(Number(props.id)),
      api.getHistory(Number(props.id)),
    ])
    ticket.value = loadedTicket
    history.value = loadedHistory
  } catch (e) {
    error.value = errorMessage(e)
  } finally {
    loading.value = false
  }
}

// Runs an action (status change, comment, ...) and reloads the page data afterwards.
async function run(action: () => Promise<unknown>) {
  busy.value = true
  error.value = ''
  try {
    await action()
    await load()
  } catch (e) {
    error.value = errorMessage(e)
  } finally {
    busy.value = false
  }
}

function changeStatus(status: TicketStatus) {
  return run(() => api.changeStatus(Number(props.id), status))
}

function changePriority(event: Event) {
  const priority = (event.target as HTMLSelectElement).value as TicketPriority
  return run(() => api.changePriority(Number(props.id), priority))
}

function addComment() {
  return run(async () => {
    await api.addComment(Number(props.id), newComment.value)
    newComment.value = ''
  })
}

// Reload if the user navigates from one ticket to another.
watch(() => props.id, load, { immediate: true })
</script>

<template>
  <RouterLink to="/tickets" class="d-inline-block mb-3">← All tickets</RouterLink>

  <div v-if="error" class="alert alert-danger" role="alert">{{ error }}</div>
  <p v-if="loading">Loading…</p>

  <template v-if="ticket">
    <div class="d-flex flex-wrap align-items-center gap-2 mb-2">
      <h1 class="h3 mb-0 me-2">#{{ ticket.id }} {{ ticket.title }}</h1>
      <StatusBadge :status="ticket.status" />
      <PriorityBadge :priority="ticket.priority" />
    </div>
    <p class="text-muted small">
      {{ ticket.category }} · created by {{ ticket.createdBy }} {{ formatDate(ticket.createdAt) }}
      · updated {{ formatDate(ticket.updatedAt) }}
    </p>

    <div class="row g-4">
      <div class="col-lg-8">
        <div class="card mb-4">
          <div class="card-body description">{{ ticket.description || 'No description.' }}</div>
        </div>

        <h2 class="h5">Comments</h2>
        <p v-if="ticket.comments.length === 0" class="text-muted">No comments yet.</p>
        <div v-for="comment in ticket.comments" :key="comment.id" class="card mb-2">
          <div class="card-body py-2">
            <div class="small text-muted">{{ comment.author }} · {{ formatDate(comment.createdAt) }}</div>
            <div class="description">{{ comment.text }}</div>
          </div>
        </div>

        <form v-if="ticket.status !== 'Closed'" class="mt-3" @submit.prevent="addComment">
          <label for="new-comment" class="form-label">Add a comment</label>
          <textarea id="new-comment" v-model="newComment" class="form-control mb-2" rows="3" required maxlength="2000" />
          <button type="submit" class="btn btn-primary btn-sm" :disabled="busy">Add comment</button>
        </form>
      </div>

      <div class="col-lg-4">
        <h2 class="h5">Actions</h2>

        <div class="mb-3">
          <div class="form-label">Change status</div>
          <p v-if="ticket.allowedStatuses.length === 0" class="text-muted small">This ticket is closed.</p>
          <div class="d-flex flex-wrap gap-2">
            <button
              v-for="status in ticket.allowedStatuses"
              :key="status"
              type="button"
              class="btn btn-outline-primary btn-sm"
              :disabled="busy"
              @click="changeStatus(status)"
            >
              {{ statusLabel(status) }}
            </button>
          </div>
        </div>

        <div class="mb-4">
          <label for="priority" class="form-label">Priority</label>
          <select
            id="priority"
            class="form-select form-select-sm"
            :value="ticket.priority"
            :disabled="busy || ticket.status === 'Closed'"
            @change="changePriority"
          >
            <option v-for="priority in priorities" :key="priority" :value="priority">{{ priority }}</option>
          </select>
        </div>

        <h2 class="h5">History</h2>
        <ul class="list-unstyled small">
          <li v-for="entry in history" :key="entry.id" class="mb-2">
            <div class="text-muted">{{ formatDate(entry.changedAt) }} · {{ entry.changedBy }}</div>
            <div>
              {{ entry.field }}:
              <template v-if="entry.oldValue">{{ statusLabel(entry.oldValue) }} → </template>
              <strong>{{ statusLabel(entry.newValue ?? '') }}</strong>
            </div>
          </li>
        </ul>
      </div>
    </div>
  </template>
</template>

<style scoped>
/* Keep line breaks the user typed in descriptions and comments. */
.description {
  white-space: pre-wrap;
}
</style>
