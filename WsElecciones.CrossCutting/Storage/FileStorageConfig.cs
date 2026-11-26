
namespace WsElecciones.CrossCutting.Storage
{
    public class FileStorageConfig
    {
        public Dictionary<string, StorageDetail> Modules { get; set; } = new();
    }

    public class StorageDetail
    {
        public string SharedPath { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
    }
}
