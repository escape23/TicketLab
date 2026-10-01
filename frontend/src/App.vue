<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { api, errorMessage } from './api'
import { currentUserId } from './currentUser'
import type { User } from './types'

const users = ref<User[]>([])
const error = ref('')

onMounted(async () => {
  try {
    users.value = await api.getUsers()
  } catch (e) {
    error.value = errorMessage(e)
  }
})
</script>

<template>
  <nav class="navbar bg-dark" data-bs-theme="dark">
    <div class="container">
      <RouterLink class="navbar-brand" to="/tickets">TicketLab</RouterLink>

      <div class="d-flex align-items-center gap-2">
        <label for="current-user" class="text-light small text-nowrap">Acting as</label>
        <select id="current-user" v-model="currentUserId" class="form-select form-select-sm">
          <option v-for="user in users" :key="user.id" :value="user.id">
            {{ user.name }} ({{ user.role }})
          </option>
        </select>
      </div>
    </div>
  </nav>

  <main class="container py-4">
    <div v-if="error" class="alert alert-danger">{{ error }}</div>
    <RouterView />
  </main>
</template>
