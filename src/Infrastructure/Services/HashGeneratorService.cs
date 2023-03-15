using Application.Interfaces;
using Isopoh.Cryptography.Argon2;

namespace Infrastructure.Services;

public class HashGeneratorService : IHashGenerator
{
    public string GetHash(string value) => Argon2.Hash(value, parallelism: Environment.ProcessorCount);
    public bool Verify(string hash, string value) => Argon2.Verify(hash, value);
}