# CoreShop Development Roadmap

This roadmap defines the evolution of CoreShop from its current state into a production-quality portfolio application.

Every milestone must be completed before moving to the next one.

Milestones may be refactored if necessary.

Never skip exit criteria.

Ordering rationale: **trust → security → foundation → polish.** UI work is deliberately scheduled *after* the architecture milestones so that views are touched once, not twice. Testing is scheduled after the service layer exists, because today's fat controllers are untestable.

---

## v1.1 — Honesty & Hygiene

**Goal:** Make the repository trustworthy and clean before anything else.

- Rewrite README.md to match reality (in-memory demo store is a *feature*: zero-setup run)
- Delete tombstone files (`DbSeeder.cs`, `CoreShopDbContext.cs`)
- Fix `Entites` → `Entities` (folder + namespace, solution-wide)
- Unify namespaces (`CoreShop.UI.Controllers` → `CoreShop.Controllers`)
- Rename Turkish/cryptic identifiers (`sifre` → `password`, `_udb` → `_userService`)
- Remove template-leftover unused usings
- Add `.editorconfig`
- Meaningful Conventional Commits from this point on

**Exit criteria:** Every sentence in README is verifiable in code; solution builds clean; zero typo/namespace inconsistencies.

---

## v1.2 — Security Foundation

**Goal:** Close the critical security findings from the audit.

- Replace unsalted SHA-256 with ASP.NET Core `PasswordHasher<T>` (PBKDF2)
- Convert all state-changing GET actions to POST (cart add/remove/increase/decrease, admin delete)
- Global antiforgery token validation on all POSTs
- File upload validation (extension whitelist, size limit)
- Safe redirects (`Url.IsLocalUrl` instead of raw `Referer`)
- Cookie hardening (expiration, sliding, SameSite, secure policy)

**Exit criteria:** No write operation reachable via GET; all POSTs token-protected; seed admin login works with the new hasher; malicious file uploads rejected.

---

## v1.3 — Error Handling & Observability

**Goal:** The application never shows a raw error or blank page; critical events are traceable.

- Error page + controller (currently `/Error` path leads nowhere)
- Custom 404 via `UseStatusCodePagesWithReExecute`
- `ILogger` structured logging in critical flows (login, register, checkout, upload)

**Exit criteria:** Unknown URL → styled 404; thrown test exception → styled 500; meaningful log lines for critical flows.

---

## v1.4 — Architecture Correction (highest leverage)

**Goal:** Business logic lives in named services; controllers only translate HTTP.

- `CartService`: single owner of session cart (kills 3x duplicated deserialization)
- `CheckoutService`: atomic checkout — stock revalidation, order + details creation, stock decrement; `Create` returns the entity (kills `GetAll().Max()` race)
- `OrderStatus` enum + role constants (kills mixed TR/EN magic strings)
- Cart badge as a ViewComponent (kills the disappearing badge bug)
- Move `Description` off `CoreEntity` onto the entities that actually need it

**Exit criteria:** No order/stock/session logic in any controller; all flows manually verified end-to-end; cart badge correct on every page.

---

## v1.5 — Validation & Data Integrity

**Goal:** No form can persist invalid data; every failure gives field-level feedback.

- `RegisterVM` / `LoginVM` (no more entity binding; kills empty-registration NullReference)
- `ModelState` checks on every POST
- Admin product form validation (no negative price/stock)
- Expiry date past-date check
- Profile fixes: display `TempData` feedback, move order-detail filtering from view to controller
- Orphan image cleanup on product delete/update

**Exit criteria:** All forms reject invalid input with Turkish field-level messages; profile feedback visible.

---

## v1.6 — UI Design System

**Goal:** One visual identity, defined once.

- Consolidate ~4,200 lines of inline `<style>` into a single `site.css` with CSS variables
- Orange (#ff6000) brand identity applied consistently
- `_ProductCard` partial (3 duplicated cards → 1)
- New hero / navbar / footer
- Fix sort/filter/search query-param loss bug (forms preserve state; select shows current value)
- Unify Bootstrap/Font Awesome versions between storefront and admin

**Exit criteria:** No `<style>` blocks left in views; all pages share one design language; filter + sort + search work together.

---

## v1.7 — UX & Accessibility

**Goal:** The polish layer users actually feel.

- Add-to-cart toast notification
- Consistent Turkish price formatting (24.999 ₺)
- Empty states (empty cart, no filter results)
- Order number on success page
- `aria-label` / `alt` / focus states
- Image optimization (`loading="lazy"`, dimensions)

**Exit criteria:** Add-to-cart gives instant visual feedback; measurably improved Lighthouse accessibility score; no raw unformatted price anywhere.

---

## v1.8 — Tests & CI

**Goal:** Critical business rules under test protection; every push verified.

- xUnit test project; 15–25 meaningful unit tests for `CartService`, `CheckoutService`, filtering logic (stock limits, oversell prevention, totals, status transitions)
- GitHub Actions: build + test on every push
- Status badge in README

**Exit criteria:** `dotnet test` green; CI pipeline running; badge visible.

---

## v1.9 — Portfolio Packaging

**Goal:** A recruiter's 3-minute journey is flawless.

- README: screenshots, architecture diagram, honest trade-offs section (in-memory choice, simulated payment)
- Live demo deployment (free tier)
- Demo credentials documented

**Exit criteria:** One link → live demo + badged, illustrated README.

---

## v2.0 — Real Database (the big leap)

**Goal:** EF Core returns, properly this time; the project earns Mid-Level signal.

- EF Core (SQLite/SQL Server) behind the existing `ICoreService` abstraction
- Configuration switch: in-memory ↔ real database (demo mode stays as a feature)
- Async service signatures
- Migrations; correct query practices (`AsNoTracking` where appropriate)

**Exit criteria:** Same application passes all tests in both modes; clean install from migrations works.
