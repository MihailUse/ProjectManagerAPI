namespace DAL
{
    internal interface ISoftDeletable
    {
        public DateTime? DeletedAt { get; set; }
    }
}
