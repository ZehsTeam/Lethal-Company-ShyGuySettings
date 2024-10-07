using BepInEx.Bootstrap;

namespace com.github.zehsteam.ShyGuySettings.Dependencies;

internal static class ScopophobiaProxy
{
    public const string PLUGIN_GUID = "Scopophobia";
    public static bool Enabled => Chainloader.PluginInfos.ContainsKey(PLUGIN_GUID);

    public static void DisableSpawnPatches()
    {
        Scopophobia.Config.DisableSpawnRatesConfig.Value = true;

        Plugin.logger.LogMessage("Disabled Scopophobia spawn patches.");
    }
}
