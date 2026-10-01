import type { TicketStatus } from './types'

export function formatDate(value: string): string {
  return new Date(value).toLocaleString('nb-NO', { dateStyle: 'short', timeStyle: 'short' })
}

// "InProgress" -> "In progress"
export function statusLabel(status: TicketStatus | string): string {
  return status === 'InProgress' ? 'In progress' : status
}
