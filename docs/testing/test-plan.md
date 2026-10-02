# Test plan — TicketLab

## 1. Purpose

Describe what we test in TicketLab, how, and with which tools,
so that bugs are found before users find them.

## 2. Scope

**In scope (MVP):**
- Create, view and filter tickets
- Change status and priority
- Add comments
- Change history
- Validation and error messages (API and UI)

**Out of scope (for now):**
- Login and permissions (phase 6)
- Accessibility (phase 7)
- Performance / load testing

## 3. Test levels

| Level | What it checks | Tool | Step |
|-------|----------------|------|------|
| Unit | One rule in isolation, e.g. allowed status changes | xUnit | 4.2 |
| Integration / API | Request → API → database → response | xUnit + WebApplicationFactory | 4.4 |
| Frontend | One Vue component or function | Vitest | 4.5 |
| Acceptance (E2E) | A full user scenario in a real browser | Playwright | 4.6 |
| User test | Real people use the app, we observe | Manual session | 4.7 |
| Manual functional | Test cases below, run by hand | Browser / `.http` file | 4.1 |

## 4. Test environment

- Windows 11, Chrome/Edge
- API: `http://localhost:5032` (.NET 10), database `TicketLab` on `.\SQLEXPRESS`
- Frontend: `http://localhost:5173` (Vite)
- Test users: Anna (User), Ola (Developer), Kari (Admin)

## 5. Test cases

**Type** is one of: `Functional`, `Negative`, `Acceptance`.
**Status** is one of: `Not run`, `Pass`, `Fail` (link a bug report if it fails).

---

### TC-01 — Create a ticket with valid data

| | |
|---|---|
| **Type** | Functional |
| **Preconditions** | API and frontend are running. Acting as Anna (User). |
| **Steps** | 1. Open the ticket list and click **New ticket**.<br>2. Title: `Printer does not print`.<br>3. Description: `Shows "offline" since this morning.`<br>4. Priority: `High`, Category: `Hardware`.<br>5. Click **Create ticket**. |
| **Expected result** | The ticket page opens. Status is **New**, priority **High**, created by **Anna User**. History shows `Status: New`. The ticket appears in the list. |
| **Actual result** | |
| **Status** | Not run |

---

<!-- Copy the block above for each new test case: TC-02, TC-03, ... -->

## 6. Test run log

| Date | Tester | Test cases run | Pass | Fail | Notes |
|------|--------|----------------|------|------|-------|
| | | | | | |
