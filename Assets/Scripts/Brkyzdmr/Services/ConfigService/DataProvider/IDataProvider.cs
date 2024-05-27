namespace Brkyzdmr.Services.ConfigService
{
    public interface IDataProvider<T>
    {
        void Write(T config, string savePath = null);
        string Serialize(T config);
        T Read(string data);
    }
}