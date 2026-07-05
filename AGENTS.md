# CoreShop AI Development Guide

## Purpose

This document defines the engineering standards, architecture principles, coding conventions, and development philosophy for the CoreShop project.

Every AI assistant working on this repository must read and follow this document before making any code changes.

If any instruction conflicts with this document, prioritize maintainability, security, and production-quality engineering.

---

# Project Goal

CoreShop is not just a school project.

The goal is to build a production-quality portfolio application that demonstrates professional software engineering skills.

Every improvement should increase:

- Code Quality
- Maintainability
- Scalability
- Security
- Readability
- User Experience
- Portfolio Quality

---

# Development Philosophy

Always prefer:

1. Maintainability
2. Readability
3. Simplicity
4. Scalability
5. Security

Never choose quick fixes over clean architecture.

If refactoring improves the project, refactor.

Avoid technical debt.

Never leave "temporary" solutions.

---

# Architecture Rules

Controllers must remain thin.

Business logic belongs inside Services.

Repositories should only access data.

Entities must never be used directly in forms.

Use ViewModels for all user input.

Separate concerns properly.

Follow SOLID principles whenever appropriate.

Avoid duplicated code.

Favor composition over duplication.

---

# Security Rules

Never trust client-side data.

Validate every user input.

Always protect state-changing actions.

Prefer secure defaults.

Never expose sensitive information.

Use proper password hashing.

Never implement insecure shortcuts.

---

# UI Rules

Keep a consistent design language.

Avoid inline CSS.

Reuse components whenever possible.

Maintain consistent spacing.

Maintain consistent typography.

Design mobile-first.

Accessibility matters.

---

# Coding Standards

Write self-explanatory code.

Use meaningful naming.

Avoid abbreviations.

Delete dead code.

Delete unused usings.

Avoid magic strings.

Keep methods focused.

Prefer readability over clever code.

---

# Refactoring Policy

Refactoring is encouraged.

If previous code can be improved, improve it.

Never avoid refactoring because something already works.

Quality is more important than speed.

---

# Milestone Rules

Development follows milestone-based progression.

Do not skip milestones.

Do not continue until every exit criterion has been satisfied.

Each milestone must finish with:

- Successful build
- Manual verification
- Clean code
- No regressions
- Meaningful commit

---

# Git Rules

Create meaningful commits.

One logical change per commit.

Prefer Conventional Commit format.

Examples:

feat:
fix:
refactor:
style:
docs:
test:
perf:
chore:

---

# Code Review Checklist

Before considering any task complete:

✔ Solution builds successfully

✔ No compiler warnings

✔ Naming is consistent

✔ Validation exists

✔ Error handling exists

✔ Security reviewed

✔ No duplicated logic

✔ UI consistency maintained

✔ Code follows architecture

---

# AI Instructions

Always explain important architectural decisions.

Never generate code that conflicts with this document.

Prefer production-quality solutions.

Treat this project as if it will be reviewed by a Senior Software Engineer during a hiring process.