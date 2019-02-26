namespace SaaMedW.Service
{
    public interface IKkm
    {
        bool IsInitialized { get; }
        bool Init();
        bool Register(decimal sm, string email);
        bool Back(decimal sm);
        bool ZReport();
        void Destroy();
        string Model { get; }
    }
}