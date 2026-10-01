<script setup lang="ts">
import { ref, watch } from 'vue'
import { api, errorMessage } from '@/api'
import { formatDate, statusLabel } from '@/format'
import { statuses, type TicketStatus, type TicketSummary } from '@/types'
import StatusBadge from '@/components/StatusBadge.vue'
import PriorityBadge from '@/components/PriorityBadge.vue'

const tickets = ref<TicketSummary[]>([])
const statusFilter = ref<TicketStatus | ''>('')
const loading = ref(true)
const error = ref('')

async function loadTickets() {
  loading.value = true
  error.value = ''
  try {
    tickets.value = await api.getTickets(statusFilter.value || undefined)
  } catch (e) {
    error.value = errorMessage(e)
  } finally {
    loading.value = false
  }
}

// Reload whenever the filter changes; "immediate" also runs it once on page load.
watch(statusFilter, loadTickets, { immediate: true })
</script>

<template>
  <div class="d-flex justify-content-between align-items-center mb-3">
    <h1 class="h3 mb-0">Tickets</h1>
    <RouterLink to="/tickets/new" class="btn btn-primary">New ticket</RouterLink>
  </div>

  <div class="mb-3" style="max-width: 220px">
    <label for="status-filter" class="form-label">Status</label>
    <select id="status-filter" v-model="statusFilter" class="form-select">
      <option value="">All</option>
      <option v-for="status in statuses" :key="status" :value="status">{{ statusLabel(status) }}</option>
    </select>
  </div>

  <div v-if="error" class="alert alert-danger">{{ error }}</div>
  <p v-else-if="loading">Loading…</p>
  <p v-else-if="tickets.length === 0" class="text-muted">No tickets found.</p>

  <div v-else class="table-responsive">
    <table class="table table-hover align-middle">
      <thead>
        <tr>
          <th scope="col">#</th>
          <th scope="col">Title</th>
          <th scope="col">Status</th>
          <th scope="col">Priority</th>
          <th scope="col">Category</th>
          <th scope="col">Created by</th>
          <th scope="col">Updated</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="ticket in tickets" :key="ticket.id">
          <td>{{ ticket.id }}</td>
          <td><RouterLink :to="`/tickets/${ticket.id}`">{{ ticket.title }}</RouterLink></td>
          <td><StatusBadge :status="ticket.status" /></td>
          <td><PriorityBadge :priority="ticket.priority" /></td>
          <td>{{ ticket.category }}</td>
          <td>{{ ticket.createdBy }}</td>
          <td>{{ formatDate(ticket.updatedAt) }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
