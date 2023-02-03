namespace Application.Common.Interfaces;

public interface IHashGeneratorService
{
    public string GetHash(string value);
    public bool Verify(string hash, string value);
}
