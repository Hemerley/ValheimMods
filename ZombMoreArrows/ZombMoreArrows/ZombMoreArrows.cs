using BepInEx;

namespace ZombMoreArrows
{
    [BepInPlugin(MODID, MODNAME, MODVER)]
    [BepInDependency(ValheimLib.ValheimLib.ModGuid)]
    class ZombMoreArrows : BaseUnityPlugin
    {

        public const string MODID = "zombehian.MoreArrows";
        public const string MODNAME = "Zomb More Arrows";
        public const string MODVER = "0.0.2";
        internal static ZombMoreArrows Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
            Util.AssetHelper.Init();
        }

    }
}
