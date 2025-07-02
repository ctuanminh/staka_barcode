using System.Threading.Tasks;

namespace FrmMain.Utils
{
    public interface IReloadableForm
    {
        Task ReLoadData(string code, long id);
    }
}
