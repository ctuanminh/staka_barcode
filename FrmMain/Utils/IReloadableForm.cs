using System.Threading.Tasks;

namespace FrmMain.Utils
{
    public interface IReloadableForm
    {
        Task ReloadData(string code, long id);
    }
}
