<script setup lang="ts">
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { api, errorMessage } from '@/api'
import { categories, priorities, type CreateTicketRequest } from '@/types'

const router = useRouter()

const form = reactive<CreateTicketRequest>({
  title: '',
  description: '',
  priority: 'Medium',
  category: 'Other',
})
const submitting = ref(false)
const error = ref('')

async function submit() {
  // The button is disabled while submitting, which prevents double submits.
  submitting.value = true
  error.value = ''
  try {
    const ticket = await api.createTicket(form)
    await router.push(`/tickets/${ticket.id}`)
  } catch (e) {
    error.value = errorMessage(e)
  } finally {
    submitting.value = false
  }
}
</script>

<template>
  <h1 class="h3 mb-3">New ticket</h1>

  <form style="max-width: 640px" @submit.prevent="submit">
    <div v-if="error" class="alert alert-danger" role="alert">{{ error }}</div>

    <div class="mb-3">
      <label for="title" class="form-label">Title</label>
      <input id="title" v-model="form.title" class="form-control" required maxlength="200" />
    </div>

    <div class="mb-3">
      <label for="description" class="form-label">Description</label>
      <textarea id="description" v-model="form.description" class="form-control" rows="5" maxlength="4000" />
    </div>

    <div class="row mb-3">
      <div class="col-sm">
        <label for="priority" class="form-label">Priority</label>
        <select id="priority" v-model="form.priority" class="form-select">
          <option v-for="priority in priorities" :key="priority" :value="priority">{{ priority }}</option>
        </select>
      </div>
      <div class="col-sm">
        <label for="category" class="form-label">Category</label>
        <select id="category" v-model="form.category" class="form-select">
          <option v-for="category in categories" :key="category" :value="category">{{ category }}</option>
        </select>
      </div>
    </div>

    <button type="submit" class="btn btn-primary" :disabled="submitting">
      {{ submitting ? 'Creating…' : 'Create ticket' }}
    </button>
    <RouterLink to="/tickets" class="btn btn-link">Cancel</RouterLink>
  </form>
</template>
