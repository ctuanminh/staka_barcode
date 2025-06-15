namespace Be.Core.BaseEntities
{
	/// <summary>
	/// abstract AuditedEntity chưa các trường dữ liệu
	/// và kế thừa từ entity<TKey> để có trường Id 
	/// </summary>
	public abstract class AuditedEntity : AuditedEntity<long>
	{

	}

	public abstract class AuditedEntity<TKey> : Entity<TKey>, IAuditedEntity where TKey : IEquatable<TKey>
	{
		public DateTime CreatedAt { get; set; }
		public long CreatedBy { get; set; }
		public DateTime UpdatedAt { get; set; }
		public long UpdatedBy { get; set; }
	}
}
