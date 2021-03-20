using System;
using HarmonyLib;
using UnityEngine;
using Common.Util;

namespace ZombMoreArrows
{
	class Arrows
	{
		[HarmonyPatch(typeof(Projectile), "Setup")]
		public static class RecollectArrows
		{
			public static System.Random rnd = new System.Random();
			public static void Prefix(ref Projectile __instance, Character owner)
			{
				if (__instance.name.Contains("bow_projectile"))
				{
					if (owner.IsPlayer())
					{
						Player localPlayer = Player.m_localPlayer;
						if (localPlayer != null)
						{
							string arrowType = localPlayer.GetAmmoItem().m_dropPrefab.name;
							if (arrowType != null)
							{
								foreach (var rec in AssetHelper.recipes.recipes)
								{
									if (arrowType == rec.name)
									{
                                        if (rec.enabled)
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
	}
}
