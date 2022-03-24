using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UpgradeEquipment_hrr.Items;
using UpgradeEquipment_hrr.UI;

namespace UpgradeEquipment_hrr
{
	class UpgradeEquipmentPlayer : ModPlayer
	{
		private int lastKnownTier = 0;
		private int lastKnownSummonTier = 0;

		public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
		{
			int tier = item.GetGlobalItem<UpgradeEquipmentGlobalItem>().UpgradeTier;
			float mult = PrefixHelper.GetFinalDamageMult(tier);
			damage = Convert.ToInt32(damage * mult);
		}

		public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			// weird bunch of logic to try and handle swapping off an upgraded projectile/summon weapon
			// swapping from a lower + weapon to a higher one still works, but if people want to abuse that i'm all for it

			// check if player is holding something
			if (player.HeldItem.type != ItemID.None)
			{
				// if yes get the tier of the held item
				int tier = player.HeldItem.GetGlobalItem<UpgradeEquipmentGlobalItem>().UpgradeTier;

				// if the held item is a summon, set the last known summon weapon tier
				if (player.HeldItem.summon && tier > 0)
				{
					if (tier != lastKnownSummonTier)
					{
						lastKnownSummonTier = tier;
					}
				}

				// if the held item is not a summon, set the last known general weapon tier
				else
				{
					if (tier != lastKnownTier)
					{
						lastKnownTier = tier;
					}
				}
			}

			float mult;

			// if the projectile IS a minion or belongs to one, use the summon weapon tier
			if (proj.minion || ProjectileID.Sets.MinionShot[proj.type])
			{
				mult = PrefixHelper.GetFinalDamageMult(lastKnownSummonTier);
			}

			// otherwise use the generic weapon tier
			else
			{
				mult = PrefixHelper.GetFinalDamageMult(lastKnownTier);
			}

			damage = Convert.ToInt32(damage * mult);
		}
	}
}