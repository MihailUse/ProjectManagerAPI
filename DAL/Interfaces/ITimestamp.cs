namespace DAL.Interfaces;

internal interface ITimestamp
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}