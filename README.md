# Movie Ticket Booking – Seat Management System

## Overview

This project implements a **backend-only seat management system** for movie shows. The system focuses exclusively on managing seat availability and booking behaviour under real-world conditions such as **high concurrency, partial failures, retries, and system restarts**.

The goal is to ensure that **no seat is ever sold more than once** and that seat availability remains accurate at all times.

---

## Key Features

* Backend-only seat management (no UI, auth, or payments)
* Clear seat lifecycle:

  * **AVAILABLE → HELD → BOOKED**
* Concurrency-safe seat holding using:

  * Database transactions
  * Row-level locking
* Automatic **hold expiry** and seat restoration when booking is not completed
* Idempotent and consistent behaviour for:

  * Page refreshes
  * Retry requests
  * Network failures
* Safe recovery during **system restarts**
* APIs to query:

  * Available seats
  * Temporarily held seats
  * Successfully booked seats

---

## Seat Lifecycle

1. **AVAILABLE**

   * Seat is free and can be held by any user.

2. **HELD**

   * Seat is temporarily locked for a specific request.
   * Each hold has an expiry time.
   * If booking is not confirmed before expiry, the seat automatically returns to AVAILABLE.

3. **BOOKED**

   * Seat is permanently reserved.
   * Cannot be held or booked again.

---

## Handling Real-World Challenges

### Concurrent Booking Attempts

* Uses database-level transactions and row locks to prevent race conditions.
* Ensures two users cannot hold or book the same seat simultaneously.

### Incomplete Bookings

* Seats are held with a time limit.
* If confirmation does not happen within the expiry window, the hold is released automatically.

### Retries & Page Refresh

* Booking confirmation is validated against a valid hold.
* Duplicate or retry requests do not cause double booking.

### System Restarts

* Seat state is persisted in the database.
* Expired holds are detected and restored safely after restart.

---

## API Capabilities

The system can answer:

* How many seats are currently **available** for a show?
* How many seats are **temporarily held**?
* How many seats are **booked**?
* What happens when a booking is not completed?

---

## Assumptions

* Payment processing and user authentication are outside the scope.
* Seats are uniquely identifiable per show.
* Hold expiry duration is configurable.

---

## Limitations

* No distributed locking (single database instance assumed).
* No user-level booking history.
* No payment or cancellation workflows.

---

## Technology Stack

* Backend: ASP.NET Core Web API
* Database: SQL Server
* Data Access: Dapper

---

## How to Run

1. Clone the repository
2. Update database connection string in `appsettings.json`
3. Run database migrations / scripts
4. Start the API

---

## Submission Notes

This project was developed as part of the **Fantacode Hiring Task** with emphasis on:

* Correctness
* Simplicity
* Reliability under pressure

For any clarification or walkthrough, feel free to reach out.

---

**Author:** Anwar Ottayil
