using Isopoh.Cryptography.Argon2;
using Isopoh.Cryptography.SecureArray;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Argon2_BCrypt_Benchmark;

// Note: The benchmarks include salt generation to reflect a real-world scenario. Therefore, some fluctuations in the results are expected.
[Config(typeof(AntivirusFriendlyConfig))]
[MemoryDiagnoser]
public class BenchmarkTests
{
    private readonly string password;
    private readonly string bcryptHash;
    private readonly string argon2RecommendedHash;
    private readonly string argon2CustomHash;

    public BenchmarkTests()
    {
        password = "password123!";
        bcryptHash = BcryptHash();
        argon2RecommendedHash = Argon2RecommendedHash();
        argon2CustomHash = Argon2CustomHash();
    }

    [Benchmark]
    public string BcryptHash()
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 10);
    }

    [Benchmark]
    public string Argon2RecommendedHash()
    {
        var salt = GenerateSalt(16);

        var argon2Config = new Argon2Config()
        {
            Type = Argon2Type.HybridAddressing, // Argon2id
            Version = Argon2Version.Nineteen,
            TimeCost = 2,
            MemoryCost = 19 * 1024, // 19 MiB
            Lanes = 1, // 1 thread
            Threads = 1,
            Password = Encoding.UTF8.GetBytes(password),
            HashLength = 32,
            Salt = salt
        }; ;

        var argon2A = new Argon2(argon2Config);
        using (SecureArray<byte> hashA = argon2A.Hash())
        {
            return argon2Config.EncodeString(hashA.Buffer);
        }
    }

    [Benchmark]
    public string Argon2CustomHash()
    {
        var salt = GenerateSalt(16);

        var argon2Config = new Argon2Config()
        {
            Type = Argon2Type.HybridAddressing, // Argon2id
            Version = Argon2Version.Nineteen,
            TimeCost = 2,
            MemoryCost = 19 * 1024, // 19 MiB
            Lanes = 2, // 2 thread
            Threads = Environment.ProcessorCount,
            Password = Encoding.UTF8.GetBytes(password),
            HashLength = 32,
            Salt = salt
        }; ;

        var argon2A = new Argon2(argon2Config);
        using (SecureArray<byte> hashA = argon2A.Hash())
        {
            return argon2Config.EncodeString(hashA.Buffer);
        }
    }

    [Benchmark]
    public bool BcryptVerify()
    {
        return BCrypt.Net.BCrypt.Verify(password, bcryptHash);
    }

    [Benchmark]
    public bool Argon2VerifyRecommended()
    {
        return Argon2.Verify(argon2RecommendedHash, password);
    }

    [Benchmark]
    public bool Argon2VerifyCustom()
    {
        return Argon2.Verify(argon2CustomHash, password);
    }

    [Benchmark]
    public bool BcryptVerifyWrong()
    {
        return BCrypt.Net.BCrypt.Verify("wrong-password", bcryptHash);
    }

    [Benchmark]
    public bool Argon2VerifyRecommendedWrong()
    {
        return Argon2.Verify(argon2RecommendedHash, "wrong-password");
    }

    [Benchmark]
    public bool Argon2VerifyCustomWrong()
    {
        return Argon2.Verify(argon2CustomHash, "wrong-password");
    }

    private static byte[] GenerateSalt(int size = 16)
    {
        var salt = new byte[size];
        using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }
}

public static class Program
{
    [System.Diagnostics.Conditional("DEBUG")]
    private static void CheckReleaseMode()
    {
        Console.WriteLine("Program is running in DEBUG mode. Please run in RELEASE mode for benchmarking.");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Use: dotnet run -c Release");
        Console.ResetColor();
        Console.WriteLine("Exiting...");
        Environment.Exit(1);
    }

    public static void Main()
    {
        CheckReleaseMode();
        BenchmarkRunner.Run<BenchmarkTests>();
    }
}