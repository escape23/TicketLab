# TicketLab frontend

Vue 3 + TypeScript + Vite + Bootstrap.

```bash
npm install     # once
npm run dev     # http://localhost:5173 (the API must be running on :5032)
npm run build   # type-check + production build
```

In development, Vite proxies `/api/*` to the ASP.NET Core API (see `vite.config.ts`).

## Structure

```
src/
  api.ts           all HTTP calls to the backend
  types.ts         TypeScript versions of the backend DTOs
  currentUser.ts   "acting as" user (temporary, until real login)
  format.ts        date and status formatting
  components/      small reusable pieces (badges)
  views/           pages: ticket list, new ticket, ticket details
  router/          URL -> page mapping
```
