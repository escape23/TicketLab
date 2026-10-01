<script setup lang="ts">
import { computed } from 'vue'
import { statusLabel } from '@/format'
import type { TicketStatus } from '@/types'

const props = defineProps<{ status: TicketStatus }>()

const colors: Record<TicketStatus, string> = {
  New: 'text-bg-primary',
  InProgress: 'text-bg-warning',
  Resolved: 'text-bg-success',
  Closed: 'text-bg-secondary',
}

const colorClass = computed(() => colors[props.status])
</script>

<template>
  <span class="badge" :class="colorClass">{{ statusLabel(status) }}</span>
</template>
