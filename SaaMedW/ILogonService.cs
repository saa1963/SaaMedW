using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public interface ILogonService
    {
        bool RegisterUser(string login, string password);
    }
}
