namespace DAL.Interfaces;

internal interface ISoftDeletable
{
    public DateTimeOffset? DeletedAt { get; set; }
}