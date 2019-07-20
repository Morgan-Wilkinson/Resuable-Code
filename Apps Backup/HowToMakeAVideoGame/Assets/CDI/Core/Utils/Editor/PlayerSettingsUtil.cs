using UnityEditor;

namespace cdi
{
    /// <summary>
    /// Ultility for PlayerSettings
    /// </summary>
    public static class PlayerSettingsUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="build"></param>
        /// <param name="symbol"></param>
        static void RemoveDefineSymbol(BuildTargetGroup build, string symbol)
        {
            string flags = PlayerSettings.GetScriptingDefineSymbolsForGroup(build);
            if (flags.Equals(symbol))
            {
                flags = "";
                PlayerSettings.SetScriptingDefineSymbolsForGroup(build, flags);
            }
            else if (flags.StartsWith(symbol + ";"))
            {
                flags = flags.Substring(symbol.Length + 1);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(build, flags);
            }
            else if (flags.EndsWith(";" + symbol))
            {
                flags = flags.Substring(0, flags.Length - symbol.Length - 1);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(build, flags);
            }
            else if (flags.Contains(";" + symbol + ";"))
            {
                flags = flags.Replace(";" + symbol + ";", ";");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(build, flags);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="build"></param>
        /// <param name="symbol"></param>
        static void AddDefineSymbol(BuildTargetGroup build, string symbol)
        {
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(build);
            if (!symbols.Equals(symbol)
                && !symbols.StartsWith(symbol + ";")
                && !symbols.Contains(";" + symbol + ";")
                && !symbols.EndsWith(";" + symbol))
            {
                symbols = symbols + (symbols.Length > 0 ? ";" : "") + symbol;
                PlayerSettings.SetScriptingDefineSymbolsForGroup(build, symbols);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="active"></param>
        public static void UpdateDefineSymbol(string symbol, bool active)
        {
            if (active)
            {
                AddDefineSymbol(BuildTargetGroup.Android, symbol);
                AddDefineSymbol(BuildTargetGroup.iOS, symbol);
            }
            else
            {
                RemoveDefineSymbol(BuildTargetGroup.Android, symbol);
                RemoveDefineSymbol(BuildTargetGroup.iOS, symbol);
            }
        }
    }
}