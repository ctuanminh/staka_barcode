using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Be.Services.BaseApp
{
    public interface ICurrentUser
    {
        long GetId();
        long GetUserId();
        string GetIpAddress();
        string GetName();
        string GetEmail();
    }
}
