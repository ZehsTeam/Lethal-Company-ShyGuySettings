using BepInEx;
using BepInEx.Logging;
using com.github.zehsteam.ShyGuySettings.Dependencies;
using com.github.zehsteam.ShyGuySettings.Patches;
using HarmonyLib;

namespace com.github.zehsteam.ShyGuySettings;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(ScopophobiaProxy.PLUGIN_GUID, BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency(LethalConfigProxy.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
internal class Plugin : BaseUnityPlugin
{
    private readonly Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

    internal static Plugin Instance;
    internal static ManualLogSource logger;

    internal static ConfigManager ConfigManager;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        logger = BepInEx.Logging.Logger.CreateLogSource(MyPluginInfo.PLUGIN_GUID);
        logger.LogInfo($"{MyPluginInfo.PLUGIN_NAME} has awoken!");

        harmony.PatchAll(typeof(StartOfRoundPatch));
        harmony.PatchAll(typeof(RoundManagerPatch));

        ConfigManager = new ConfigManager();

        ScopophobiaProxy.DisableSpawnPatches();
    }

    public void LogInfoExtended(object data)
    {
        if (ConfigManager.ExtendedLogging.Value)
        {
            logger.LogInfo(data);
        }
    }

    public void LogWarningExtended(object data)
    {
        if (ConfigManager.ExtendedLogging.Value)
        {
            logger.LogWarning(data);
        }
    }
}
