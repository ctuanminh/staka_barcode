namespace Be.Data.BaseApp
{
	public interface ICurrentUserService
	{
		Guid GetId();
		string GetUserId();
		string GetIp();
		string GetName();
		string GetEmail();
	}
}
