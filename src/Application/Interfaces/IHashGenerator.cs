namespace Application.Interfaces;

public interface IHashGenerator
{
    string GetHash(string value);
    bool Verify(string hash, string value);
}