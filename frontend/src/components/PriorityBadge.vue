<script setup lang="ts">
import { computed } from 'vue'
import type { TicketPriority } from '@/types'

const props = defineProps<{ priority: TicketPriority }>()

const colors: Record<TicketPriority, string> = {
  Low: 'text-bg-light border',
  Medium: 'text-bg-info',
  High: 'text-bg-warning',
  Critical: 'text-bg-danger',
}

const colorClass = computed(() => colors[props.priority])
</script>

<template>
  <span class="badge" :class="colorClass">{{ priority }}</span>
</template>
