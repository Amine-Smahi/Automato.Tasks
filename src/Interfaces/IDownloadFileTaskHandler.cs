namespace Automato.Tasks.Interfaces
{
    public interface IDownloadFileTaskHandler
    {
        bool DownloadFileAndReturnStatus(string url);
    }
}
