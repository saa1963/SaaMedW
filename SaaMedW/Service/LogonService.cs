using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.Service
{
    public class LogonService : ILogonService
    {
        //private static pdbUser m_user;
        public bool RegisterUser(string login, string password)
        {
            password = password ?? "";
            var ctx = new SaaMedEntities();
            var logonUser = ctx.Users.Where(s => s.Login == login).FirstOrDefault();
            ctx.Dispose();
            var hash = new System.Security.Cryptography.SHA1CryptoServiceProvider()
                    .ComputeHash(System.Text.Encoding.ASCII.GetBytes(password));
            if ((logonUser.Password == null && password == "") 
                || hash.SequenceEqual(logonUser.Password))
            {
                Global.Source.rUser = logonUser;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
