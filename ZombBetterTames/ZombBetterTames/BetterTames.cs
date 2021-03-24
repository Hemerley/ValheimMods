using BepInEx;
using System.Reflection;
using HarmonyLib;

namespace ZombBetterTames
{
    // Setup Base Configuration
    [BepInPlugin(MODID, MODNAME, MODVER)]

    public class BetterTames : BaseUnityPlugin
    {
        // mod information
        private const string MODID = "zombehian.BetterTames";
        public const string MODNAME = "Zomb Better Tames";
        private const string MODVER = "0.0.1";

        // harmony project settings
        private Harmony _harmony;
        internal static BetterTames Instance { get; private set; }

        private void Awake()
        {
            // harmony patcher
            this._harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
        }

    }
}
