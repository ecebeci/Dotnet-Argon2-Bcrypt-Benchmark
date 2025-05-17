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
   cd Dotnet-Argon2-Bcrypt-Benchmark
    ```

2. Run the benchmarks in Release mode

    ```bash
    dotnet run -c Release
    ```

## Results

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.5335/23H2/2023Update/SunValley3)
AMD Ryzen 5 5600H with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.204
  [Host] : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX2

Job=MediumRun  Toolchain=InProcessNoEmitToolchain  IterationCount=15
LaunchCount=2  WarmupCount=10

| Method                       | Mean     | Error    | StdDev   | Gen0       | Gen1      | Gen2      | Allocated    |
|----------------------------- |---------:|---------:|---------:|-----------:|----------:|----------:|-------------:|
| BcryptHash                   | 57.70 ms | 0.422 ms | 0.592 ms |          - |         - |         - |      5.58 KB |
| Argon2RecommendedHash        | 85.08 ms | 2.175 ms | 3.255 ms | 11142.8571 | 2428.5714 | 1285.7143 | 120821.72 KB |
| Argon2CustomHash             | 59.75 ms | 1.429 ms | 2.094 ms | 13555.5556 | 2333.3333 | 1333.3333 | 120905.62 KB |
| BcryptVerify                 | 57.65 ms | 0.321 ms | 0.461 ms |          - |         - |         - |      5.44 KB |
| Argon2VerifyRecommended      | 81.89 ms | 0.715 ms | 1.002 ms | 11142.8571 | 2428.5714 | 1285.7143 | 120819.47 KB |
| Argon2VerifyCustom           | 58.13 ms | 1.043 ms | 1.529 ms | 13555.5556 | 2222.2222 | 1222.2222 | 121051.48 KB |
| BcryptVerifyWrong            | 56.84 ms | 0.219 ms | 0.321 ms |          - |         - |         - |      5.45 KB |
| Argon2VerifyRecommendedWrong | 81.76 ms | 0.391 ms | 0.560 ms | 11142.8571 | 2428.5714 | 1285.7143 |  120819.6 KB |
| Argon2VerifyCustomWrong      | 54.89 ms | 1.090 ms | 1.597 ms | 13444.4444 | 2222.2222 | 1111.1111 | 120938.96 KB |

## Interpretation

- **Performance**: Bcrypt is faster and uses significantly less memory, making it suitable for low-resource environments.
- **Security tradeoff**: Argon2 is slower and memory-heavy by design, which provides stronger resistance against GPU and parallel brute-force attacks.
- **Custom vs Recommended**: Custom Argon2 settings can offer better performance while keeping most of the security benefits. The recommended settings are stricter but costlier.
