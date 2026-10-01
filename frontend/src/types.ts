// TypeScript versions of the backend DTOs (backend/TicketLab.Api/Dtos/TicketDtos.cs).
// If the API changes, these must be updated too.

export type UserRole = 'User' | 'Developer' | 'Admin'
export type TicketStatus = 'New' | 'InProgress' | 'Resolved' | 'Closed'
export type TicketPriority = 'Low' | 'Medium' | 'High' | 'Critical'
export type TicketCategory = 'Hardware' | 'Software' | 'Network' | 'Access' | 'Other'

export const statuses: TicketStatus[] = ['New', 'InProgress', 'Resolved', 'Closed']
export const priorities: TicketPriority[] = ['Low', 'Medium', 'High', 'Critical']
export const categories: TicketCategory[] = ['Hardware', 'Software', 'Network', 'Access', 'Other']

export interface User {
  id: number
  name: string
  role: UserRole
}

export interface TicketSummary {
  id: number
  title: string
  priority: TicketPriority
  category: TicketCategory
  status: TicketStatus
  createdBy: string
  assignedTo: string | null
  createdAt: string
  updatedAt: string
}

export interface TicketDetails extends TicketSummary {
  description: string
  comments: TicketComment[]
  allowedStatuses: TicketStatus[]
}

export interface TicketComment {
  id: number
  text: string
  author: string
  createdAt: string
}

export interface HistoryEntry {
  id: number
  field: string
  oldValue: string | null
  newValue: string | null
  changedBy: string
  changedAt: string
}

export interface CreateTicketRequest {
  title: string
  description: string
  priority: TicketPriority
  category: TicketCategory
}
