using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Be.Core.BaseEntities
{
	/// <summary>
	/// Định nghĩa abstract class cho các entity sẽ có 
	/// Field Id là long
	/// </summary>
	public abstract class Entity : IEntity<long>
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
	}

	public abstract class Entity<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public virtual TKey Id { get; set; }
	}
}
