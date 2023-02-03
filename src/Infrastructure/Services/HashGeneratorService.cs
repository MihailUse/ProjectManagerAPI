using Application.Common.Interfaces;
using Isopoh.Cryptography.Argon2;

namespace Domain.Services;

public class HashGeneratorService : IHashGeneratorService
{
    public string GetHash(string value) => Argon2.Hash(value, parallelism: Environment.ProcessorCount);
    public bool Verify(string hash, string value) => Argon2.Verify(hash, value);
}
