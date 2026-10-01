import { ref, watch } from 'vue'

// MVP shortcut (known technical debt): "who am I" is picked in a dropdown
// and sent to the API as the X-User-Id header. Replaced by real login in phase 6.

const storageKey = 'ticketlab.userId'

export const currentUserId = ref<number>(Number(localStorage.getItem(storageKey)) || 1)

watch(currentUserId, (id) => localStorage.setItem(storageKey, String(id)))
