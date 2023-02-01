namespace DAL.Interfaces;

internal interface ISoftDeletable
{
    public DateTime? DeletedAt { get; set; }
}