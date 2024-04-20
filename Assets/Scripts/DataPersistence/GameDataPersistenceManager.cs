using RenderDream.GameEssentials;

namespace Game
{
    public class GameDataPersistenceManager : DataPersistenceManager<SettingsData, GameData>
    {
        protected override GameData NewGameData() => new();
        protected override SettingsData NewSettingsData() => new();
    }
}
