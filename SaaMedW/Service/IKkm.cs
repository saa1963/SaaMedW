using System;
using System.Collections.Generic;

namespace SaaMedW.Service
{
    public interface IKkm
    {
        bool IsInitialized { get; }
        bool Init();
        bool Register(List<Tuple<string, int, decimal>> uslugi,
           decimal oplata, enumPaymentType vidOplata,
           string emailOrPhone, bool electron);
        bool Back(decimal sm);
        bool OpenShift();
        bool ZReport();
        void Destroy();
        string Model { get; }
        /// <summary>
        /// Неотправленные в ОФД документы
        /// </summary>
        /// <param name="timeOfFirstUnsentDocument">Дата и время первого неотправленного документа</param>
        /// <returns>Кол-во неотправленных документов</returns>
        bool NumberOfUnsentDocuments(out uint unsentCount, out DateTime timeOfFirstUnsentDocument);
        int GetNumShift();
    }
}