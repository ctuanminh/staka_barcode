namespace Be.Core.BaseEntities
{
	public interface IAuditedEntity
	{
        DateTime CreatedAt { get; set; }
		long CreatedBy { get; set; }
		DateTime UpdatedAt { get; set; }
		long UpdatedBy { get; set; }
    }
}
