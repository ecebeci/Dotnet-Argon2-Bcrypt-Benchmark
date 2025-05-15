# Argon2 vs Bcrypt Benchmark

This project benchmarks password hashing and verification performance between Argon2 and Bcrypt algorithms using .NET.

## Features

- Argon2 hashing with configurable parameters (using Isopoh.Cryptography.Argon2)
- Bcrypt hashing with configurable work factor (using BCrypt.Net)
- BenchmarkDotNet for precise performance measurement
- Tests both hashing and verification speeds
- OWASP Password Storage Cheat Sheet recommendations followed: [OWASP Password Storage Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html)

## Setup

1. Clone the repo:

   ```bash
   git clone https://github.com/ecebeci/Dotnet-Argon2-Bcrypt-Benchmark
   cd argon2-bcrypt-benchmark
    ```

2. Run the benchmarks in Release mode

    ```bash
    dotnet run -c Release
    ```
