namespace DAL.Interfaces;

internal interface ITimestamp
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}