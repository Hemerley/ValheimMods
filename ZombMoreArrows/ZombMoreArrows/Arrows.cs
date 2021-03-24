using static Common.Util.AssetHelper;
using HarmonyLib;
using UnityEngine;
using static ZombMoreArrows.ZombMoreArrows;

namespace ZombMoreArrows
{
	class Arrows
	{

		[HarmonyPatch(typeof(Projectile), "Setup")]
		public static class Recollect_Arrows
		{
			public static System.Random rnd = new System.Random();
			public static void Prefix(ref Projectile __instance, Character owner)
			{
				if (__instance.name.Contains("zomb_projectile"))
				{

					if (owner.IsPlayer())
					{
						if (betterArrowPhysicsEnabled.Value)
						{
							__instance.m_gravity = arrowGravity.Value;
						}

						Player localPlayer = Player.m_localPlayer;
						if (localPlayer != null)
						{
							string arrowType = localPlayer.GetAmmoItem().m_dropPrefab.name;
							if (arrowType != null)
							{
								foreach (var rec in recipes.recipes)
								{
									if (arrowType == rec.name)
									{
										if (recollectArrowsEnabled.Value)
										{
											int randomChance = rnd.Next(1, 101);
											if (randomChance <= rec.retrieveChance)
											{
												GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(rec.name);
												__instance.m_spawnOffset = new Vector3(0.5f, 0.05f, -0.5f);
												__instance.m_spawnOnHitChance = 1f;
												__instance.m_spawnOnHit = itemPrefab;
												__instance.m_hideOnHit = __instance.gameObject;
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

        [HarmonyPatch(typeof(Attack), "FireProjectileBurst")]
        public static class FireProjectileBurst_Patch
        {
            public static bool Prefix(ref Attack __instance)
            {
                bool result;
                if (!__instance.m_character.IsPlayer() || !betterArrowPhysicsEnabled.Value)
                {
                    result = true;
                }
                else
                {

                    ItemDrop.ItemData ammoItem = Player.m_localPlayer.m_ammoItem;
                    if (ammoItem != null && ammoItem.m_shared.m_attack.m_attackProjectile != null)
                    {
                        ammoItem.m_shared.m_attack.m_projectileVel = arrowVelocity.Value;
                    }
                    result = true;
                }
                return result;
            }
        }

        [HarmonyPatch(typeof(Attack), "GetProjectileSpawnPoint")]
        public static class GetProjectileSpawnPoint_Patch
        {
            public static void Postfix(ref Attack __instance, ref Vector3 spawnPoint, ref Vector3 aimDir)
            {
                if (!__instance.m_character.IsPlayer() || betterArrowPhysicsEnabled.Value)
                {
                    aimDir += arrowAimDirection.Value;
                }
            }
        }
    }
}
