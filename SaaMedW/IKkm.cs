namespace SaaMedW
{
    public interface IKkm
    {
        bool IsInitialized { get }
        bool Init();
        bool Register(decimal sm);
        bool Back(decimal sm);
        bool ZReport();
        void Close();
    }
}