import { currentUserId } from './currentUser'
import type {
  CreateTicketRequest,
  HistoryEntry,
  TicketComment,
  TicketDetails,
  TicketPriority,
  TicketStatus,
  TicketSummary,
  User,
} from './types'

// Thrown for every non-2xx response, so views can show a useful message.
export class ApiError extends Error {
  status: number
  details: string[]

  constructor(status: number, message: string, details: string[] = []) {
    super(message)
    this.status = status
    this.details = details
  }
}

async function request<T>(method: string, url: string, body?: unknown): Promise<T> {
  const headers: Record<string, string> = { 'X-User-Id': String(currentUserId.value) }
  if (body !== undefined) headers['Content-Type'] = 'application/json'

  const response = await fetch(url, {
    method,
    headers,
    body: body === undefined ? undefined : JSON.stringify(body),
  })

  if (!response.ok) {
    // ASP.NET Core returns errors as ProblemDetails JSON:
    // { "title": "...", "status": 400, "errors": { "Title": ["The Title field is required."] } }
    const problem = await response.json().catch(() => null)
    const details = problem?.errors ? Object.values(problem.errors as Record<string, string[]>).flat() : []
    throw new ApiError(response.status, problem?.title ?? `Request failed (${response.status})`, details)
  }

  return (await response.json()) as T
}

export const api = {
  getUsers: () => request<User[]>('GET', '/api/users'),

  getTickets: (status?: TicketStatus) =>
    request<TicketSummary[]>('GET', status ? `/api/tickets?status=${status}` : '/api/tickets'),

  getTicket: (id: number) => request<TicketDetails>('GET', `/api/tickets/${id}`),

  createTicket: (ticket: CreateTicketRequest) => request<TicketDetails>('POST', '/api/tickets', ticket),

  changeStatus: (id: number, status: TicketStatus) =>
    request<TicketDetails>('PATCH', `/api/tickets/${id}/status`, { status }),

  changePriority: (id: number, priority: TicketPriority) =>
    request<TicketDetails>('PATCH', `/api/tickets/${id}/priority`, { priority }),

  addComment: (id: number, text: string) =>
    request<TicketComment>('POST', `/api/tickets/${id}/comments`, { text }),

  getHistory: (id: number) => request<HistoryEntry[]>('GET', `/api/tickets/${id}/history`),
}

// Turns any caught error into text for an alert.
export function errorMessage(error: unknown): string {
  if (error instanceof ApiError) {
    return error.details.length > 0 ? `${error.message} ${error.details.join(' ')}` : error.message
  }
  return 'Could not reach the server. Is the API running?'
}
