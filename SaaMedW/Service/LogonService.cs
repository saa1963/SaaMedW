using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.Service
{
    public class LogonService : ILogonService
    {
        log4net.ILog log = log4net.LogManager.GetLogger(typeof(LogonService));

        public bool RegisterUser(string login, string password)
        {
            password = password ?? "";
            var ctx = new SaaMedEntities();
            var logonUser = ctx.Users.Where(s => s.Login == login).FirstOrDefault();
            ctx.Dispose();
            try
            {
                if (logonUser == null) throw new Exception($"Пользователь {login} не существует.");
                if (logonUser.Password != null)
                {
                    var hash = new System.Security.Cryptography.SHA1CryptoServiceProvider()
                        .ComputeHash(System.Text.Encoding.ASCII.GetBytes(password));
                    if (!hash.SequenceEqual(logonUser.Password))
                        throw new Exception($"Пароль для пользователя {login} неверен");
                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(password))
                        throw new Exception($"Пароль для пользователя {login} должен быть пустым");
                }
                Global.Source.rUser = logonUser;
                return true;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return false;
            }
        }
    }
}
