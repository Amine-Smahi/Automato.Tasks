namespace Automato.Tasks.Interfaces
{
    public interface IDownloadFileTaskHandler
    {
        public bool DownloadFileAndReturnStatus(string url, string downloadLocation);
    }
}
