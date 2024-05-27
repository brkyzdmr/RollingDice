namespace Brkyzdmr.Services.SaveService
{
    public interface ISaveService
    {
        int GetInt(string key, int defaultValue);
        void SetInt(string key, int value);

        string GetString(string key, string defaultValue);
        void SetString(string key, string value);
    }
}