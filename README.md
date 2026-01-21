# 🎬 Movie Seat Management System (Backend)

A backend system focused on **managing seat availability for movie shows**.
This project is intentionally scoped to **seat holding and booking logic only**, designed to handle **high concurrency** and **real‑world booking scenarios**.

---

## 🚀 Features

* Manage movie show seats
* Hold seats temporarily (to prevent double booking)
* Automatically release expired holds
* Confirm bookings from valid holds
* Prevent overselling in concurrent requests
* RESTful API with Swagger documentation

---

## 🧠 Core Concepts

### Seat Status Flow

```
AVAILABLE → HELD → BOOKED
       ↑
   (Hold Expiry)
```

| Status    | Description                   |
| --------- | ----------------------------- |
| AVAILABLE | Seat is free and can be held  |
| HELD      | Temporarily locked for a user |
| BOOKED    | Permanently booked            |

---

## 🏗️ Tech Stack

* **.NET 8 / ASP.NET Core Web API**
* **Dapper** (lightweight ORM)
* **SQL Server**
* **Swagger (Swashbuckle)**

---

## 🗄️ Database Design (Simplified)

### Shows Table

```sql
CREATE TABLE Shows (
    ShowId UNIQUEIDENTIFIER PRIMARY KEY,
    MovieName NVARCHAR(100),
    ShowTime DATETIME
);
```

### Seats Table

```sql
CREATE TABLE Seats (
    SeatId UNIQUEIDENTIFIER PRIMARY KEY,
    ShowId UNIQUEIDENTIFIER,
    SeatNumber INT,
    Status INT, -- 0=Available, 1=Held, 2=Booked
    HoldId UNIQUEIDENTIFIER NULL,
    HoldExpiry DATETIME NULL
);
```

---

## 📌 API Endpoints

### 1️⃣ Hold Seats

**POST** `/api/seats/hold`

Request:

```json
{
  "showId": "85E789E4-09A4-4714-966C-EF79F08169A5",
  "seatCount": 2
}
```

Behavior:

* Picks available seats
* Marks them as `HELD`
* Generates `HoldId`
* Sets expiry time (e.g. 2 minutes)

---

### 2️⃣ Confirm Booking

**POST** `/api/seats/confirm/{holdId}`

Behavior:

* Validates hold
* Converts `HELD → BOOKED`
* Rejects expired holds

---

### 3️⃣ Release Expired Holds

Automatically handled:

* On every hold request **OR**
* Via background job (recommended)

Expired seats:

```
HELD → AVAILABLE
```

---

## 🔐 Concurrency Handling

The system ensures:

* No two users can book the same seat
* Seat updates happen inside **transactions**
* Expired holds are cleaned before allocation

Typical strategy:

* `UPDATE ... WHERE Status = AVAILABLE`
* `ROWLOCK / UPDLOCK`

---

## 📖 Swagger Notes (Important)

Swagger shows **example GUIDs only**.

> ⚠️ Swagger does NOT read values from the database.

You must manually replace example values with **actual ShowIds** from your DB when testing.

Example:

```json
{
  "showId": "85E789E4-09A4-4714-966C-EF79F08169A5",
  "seatCount": 2
}
```

---

## 🧪 Sample Data

Insert sample seats:

```sql
DECLARE @ShowId UNIQUEIDENTIFIER = '85E789E4-09A4-4714-966C-EF79F08169A5';

INSERT INTO Seats (SeatId, ShowId, SeatNumber, Status)
VALUES
(NEWID(), @ShowId, 1, 0),
(NEWID(), @ShowId, 2, 0),
(NEWID(), @ShowId, 3, 0),
(NEWID(), @ShowId, 4, 0);
```

---

## 🎯 Design Decisions

* Focused only on **seat logic** (no auth/payment)
* Dapper for performance & control
* Explicit seat states for clarity
* Clean separation: Controller → Service → Repository

---

## 

> "This service guarantees seat consistency under concurrent access by using transactional seat holds with expiry and state transitions."

---

## ✅ Possible Enhancements

* Background job (Quartz / Hangfire) for cleanup
* Seat selection by row/column
* Idempotent booking confirmation
* Distributed lock (Redis) for scale

---

## 👤 Author

**Anwar Ottayil**
Backend Developer | ASP.NET Core | SQL Server

---


