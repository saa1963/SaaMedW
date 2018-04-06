using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public interface ILocalStorage
    {
        string GetLoginName(string username);
        void SetLoginName(string username, string loginname);
    }
}
