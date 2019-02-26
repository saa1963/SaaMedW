using System;
using System.Collections.Generic;

namespace SaaMedW.Service
{
    public interface IKkm
    {
        bool IsInitialized { get; }
        bool Init();
        bool Register(List<Tuple<string, int, decimal>> sm, string emailOrPhone, bool electron);
        bool Back(decimal sm);
        bool ZReport();
        void Destroy();
        string Model { get; }
    }
}