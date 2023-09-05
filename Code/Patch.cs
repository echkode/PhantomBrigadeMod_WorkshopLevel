using HarmonyLib;

using PhantomBrigade;
using PBOverworldBaseLevelSystem = PhantomBrigade.Overworld.Systems.OverworldBaseLevelSystem;

using UnityEngine;

namespace EchKode.PBMods.WorkshopLevel
{
	[HarmonyPatch]
	static class Patch
	{
		[HarmonyPatch(typeof(PBOverworldBaseLevelSystem), "RefreshBaseLevels")]
		[HarmonyPrefix]
		static bool Obls_RefreshBaseLevels()
		{
			var persistent = Contexts.sharedInstance.persistent;
			var currentLevel = persistent.hasBaseProvinceLevel ? persistent.baseProvinceLevel.level : 0;
			var inventoryParts = EquipmentUtility.GetPartsInInventory(IDUtility.playerBasePersistent);
			if (inventoryParts == null || inventoryParts.Count == 0)
			{
				return false;
			}

			var newLevel = currentLevel;
			foreach (var part in inventoryParts )
			{
				// Get highest level from parts in the base's inventory.

				if (part == null)
				{
					continue;
				}
				if (!part.hasLevel)
				{
					continue;
				}
				if (part.level.i <= newLevel)
				{
					continue;
				}
				newLevel = part.level.i;
			}

			foreach (var unit in Contexts.sharedInstance.persistent.GetEntitiesWithEntityLinkPersistentParent(IDUtility.playerBasePersistent.id.id))
			{
				// Parts loaded out on squad are not necessarily included in the base's inventory.

				foreach (var part in EquipmentUtility.GetPartsInUnit(unit))
				{
					if (part == null)
					{
						continue;
					}
					if (!part.hasLevel)
					{
						continue;
					}
					if (part.level.i <= newLevel)
					{
						continue;
					}
					newLevel = part.level.i;
				}
			}

			// If you want to adjust the level found, do the calculation here.
			//newLevel = Mathf.RoundToInt(newLevel * 0.9f);

			if (newLevel <= currentLevel)
			{
				// Workshop level only goes up.

				return false;
			}

			Debug.LogFormat(
				"Mod {0} ({1}) New base level: {0} based on highest part level in inventory or loaded out on squad | Previous base level: {1}",
				ModLink.modIndex,
				ModLink.modID,
				newLevel,
				currentLevel);
			persistent.ReplaceBaseProvinceLevel(newLevel);
			persistent.ReplaceWorkshopLevel(newLevel);

			return false;
		}
	}
}
