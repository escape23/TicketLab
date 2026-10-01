import { createRouter, createWebHistory } from 'vue-router'
import TicketListView from '@/views/TicketListView.vue'
import NewTicketView from '@/views/NewTicketView.vue'
import TicketDetailsView from '@/views/TicketDetailsView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/', redirect: '/tickets' },
    { path: '/tickets', component: TicketListView },
    { path: '/tickets/new', component: NewTicketView },
    // props: true passes the :id from the URL to the view as a prop.
    { path: '/tickets/:id', component: TicketDetailsView, props: true },
    { path: '/:pathMatch(.*)*', redirect: '/tickets' },
  ],
})

export default router
