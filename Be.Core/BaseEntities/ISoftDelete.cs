namespace Be.Core.BaseEntities
{
	/// <summary>
	/// Set delete = 1 or 0
	/// </summary>
	public interface ISoftDelete
	{
		bool IsDelete { get; set; }
	}
}
