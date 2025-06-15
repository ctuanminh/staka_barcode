using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Be.Common.Dtos.Identity
{
    public class CreateRoleRequest
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }
}
