using Automato.Tasks.Models;

namespace Automato.Tasks.Interfaces
{
    public interface IUserSettingsHandler
    {
        void LoadSettings(UserSettings userSettings, bool load);
    }
}
