# Rent Car

A simple Car Renting Website

---


## Local Development

### Requirement

- Visual Studio
- .NET 8 SDK
- .NET 8 Runtime

### Setup

1. Masukkan folder RentCar.Web dan jalankan RentCar.Web.sln
2. Buka Package Manager Console (PMC) dan jalankan Add-Migration, lalu Update-Database
3. Jalankan Query Data.sql ke database untuk mengisi data
4. Jalankan Development pada RentCar.Web

---

## Tech Stack and Packages🍕
- MVC - untuk drag and drop tasks
- EntityFrameworkCore - meng-integrasikan database ke code
- BCrypt.Net-Next - untuk hashing password

---

## Fitur Aplikasi 🤔
- Role Based Authentication and Authorization - Customer memiliki limit dan Owner memiliki akses penuh CRUD
- Full CRUD Operations - Owner can CRUD Car, Employee, Maintenances, and Rentals
- Profile Edit - User and Owner an customize its name, change email and password

---

## Known Issues
- ViewModel dan Controller kurang konsisten
- Payment Controller tidak berfungsi
