# Roadmap

Each phase is tied to the competence goals (MÃ¥l) it helps document.
Already approved: 8, 10, 14, 15, 18, 20.

| Phase | What                                                      | MÃ¥l             | Status |
|-------|-----------------------------------------------------------|-----------------|--------|
| 0     | Setup: repo, structure, README                            | (14)            | âœ…     |
| 1     | Database: EF Core model, migrations, seed data            | 5               | âœ…     |
| 2     | Backend API: tickets, status, comments, history           | 5               | â¬œ     |
| 3     | Vue frontend: list, form, ticket page                     | 19              | â¬œ     |
| â€”     | **MVP done**                                              |                 |        |
| 4     | Testing: unit, integration, API, E2E, user test           | **16**, 3       | â¬œ     |
| 5     | CI + Cron: GitHub Actions, scheduled workflow             | **16**          | â¬œ     |
| 6     | Authentication, authorization, privacy by design          | **1**, 13, 17   | â¬œ     |
| 7     | Accessibility (WCAG)                                      | **9**, 3        | â¬œ     |
| 8     | Documentation + user guide                                | **6**, 7        | â¬œ     |
| 9     | Integration with another system                           | **5**           | â¬œ     |
| 10    | Dashboard: data collection, analysis, visualization       | **4**, 2        | â¬œ     |
| 11    | Technical debt + infrastructure requirements              | **11**, **12**, 2 | â¬œ   |
| 12    | GDPR (data retention) + dependency licenses               | **13**          | â¬œ     |
| ongoing | Reflection: Vue vs Razor/Appframe, EF Core vs procedures | **19**         | â¬œ     |

Goals mainly documented through work at Omega365 (not this project): 7 (partly), 17 (partly), 21.

## Phase 4 â€” MÃ¥l 16 plan (testing and debugging)

| Requirement          | Tool                                         |
|----------------------|----------------------------------------------|
| Unit tests           | xUnit                                        |
| Integration tests    | xUnit + WebApplicationFactory + test database |
| API testing          | `.http` files (REST Client) + automated tests |
| Negative testing     | Same tools, invalid/unauthorized input       |
| Frontend tests       | Vitest + Vue Test Utils                      |
| Acceptance tests     | Playwright (`npm test`)                      |
| Testing with users   | Session with 2â€“3 colleagues                  |
| Automation           | GitHub Actions on every PR                   |
| Cron                 | Scheduled workflow + background job in API   |
| Debugging            | Bug report â†’ failing test â†’ fix â†’ green test |
