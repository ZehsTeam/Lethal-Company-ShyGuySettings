using BepInEx.Configuration;
using System.Collections.Generic;
using System.Reflection;

namespace com.github.zehsteam.ShyGuySettings;

internal class ConfigManager
{
    // General Settings
    public ConfigEntry<bool> ExtendedLogging { get; private set; }

    // Spawn Settings
    public ConfigEntry<string> SpawnProbabilityCurve { get; private set; }

    public ConfigManager()
    {
        BindConfigs();
        SetupChangedEvents();
        //ClearUnusedEntries();
    }

    private void BindConfigs()
    {
        ConfigHelper.SkipAutoGen();

        // General Settings
        ExtendedLogging = ConfigHelper.Bind("General Settings", "ExtendedLogging", defaultValue: false, requiresRestart: false, "Enable extended logging.");

        // Spawn Settings
        SpawnProbabilityCurve = ConfigHelper.Bind("Spawn Settings", "ProbabilityCurve", defaultValue: "1.0, 1.0, 1.0", requiresRestart: false, $"Determines how likely {EnemyDataManager.EnemyName} is to spawn throughout the day. Accepts an array of floats with each entry separated by a comma.");
    }

    private void SetupChangedEvents()
    {
        SpawnProbabilityCurve.SettingChanged += SpawnProbabilityCurve_SettingChanged;
    }

    private void SpawnProbabilityCurve_SettingChanged(object sender, System.EventArgs e)
    {
        EnemyHelper.SetProbabilityCurve(EnemyDataManager.EnemyName, Utils.ToFloatsArray(SpawnProbabilityCurve.Value));
    }

    private void ClearUnusedEntries()
    {
        ConfigFile configFile = Plugin.Instance.Config;

        // Normally, old unused config entries don't get removed, so we do it with this piece of code. Credit to Kittenji.
        PropertyInfo orphanedEntriesProp = configFile.GetType().GetProperty("OrphanedEntries", BindingFlags.NonPublic | BindingFlags.Instance);
        var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(configFile, null);
        orphanedEntries.Clear(); // Clear orphaned entries (Unbinded/Abandoned entries)
        configFile.Save(); // Save the config file to save these changes
    }
}
