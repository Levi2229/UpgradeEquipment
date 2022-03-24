using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using UpgradeEquipment_hrr.UI;

namespace UpgradeEquipment_hrr.Items
{
	public class UpgradeEquipmentGlobalItem : GlobalItem
	{
		public byte UpgradeTier;
		private Color _tooltipColor = Color.CornflowerBlue;

		public UpgradeEquipmentGlobalItem()
		{
			UpgradeTier = 0;
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (UpgradeTier > 0)
			{
				// upgrade tier tooltip
				TooltipLine tooltipTier = new TooltipLine(mod, "UpgradeTier", "Upgrade Tier " + UpgradeTier)
				{
					isModifier = true,
					overrideColor = PrefixHelper.GetTierColor(UpgradeTier),
				};
				tooltips.Add(tooltipTier);

				// tooltip for non-summon weapons
				if (!item.summon)
				{
					float calcedDamageDisplay = 1f * (PrefixHelper.GetFinalDamageMult(UpgradeTier) - 1f) * 100;
					TooltipLine tooltipFinalDamage = new TooltipLine(mod, "damage", "+" + Math.Round(calcedDamageDisplay) + "% final damage")
					{
						isModifier = true,
						overrideColor = _tooltipColor
					};
					tooltips.Add(tooltipFinalDamage);

					TooltipLine tooltipCrit = new TooltipLine(mod, "crit", "+" + PrefixHelper.GetCriticalMult(UpgradeTier) + "% critical strike chance")
					{
						isModifier = true,
						overrideColor = _tooltipColor
					};
					tooltips.Add(tooltipCrit);
				}
				else

				// tooltip for summon weapons
				{
					float calcedDamageDisplay = 1f * (PrefixHelper.GetFinalDamageMult(UpgradeTier) - 1f) * 100;
					TooltipLine tooltipFinalDamage = new TooltipLine(mod, "damage", "+" + Math.Round(calcedDamageDisplay) + "% final damage")
					{
						isModifier = true,
						overrideColor = _tooltipColor
					};
					tooltips.Add(tooltipFinalDamage);
				}

				// use speed tooltip
				float calcedSpeedDisplay = (PrefixHelper.GetSpeedMult(UpgradeTier) - 1f) * 100;
				if (calcedSpeedDisplay >= 1)
				{
					TooltipLine tooltipSpeed = new TooltipLine(mod, "velocity", "+" + Math.Round(calcedSpeedDisplay) + "% speed")
					{
						isModifier = true,
						overrideColor = _tooltipColor
					};
					tooltips.Add(tooltipSpeed);
				}
			}
		}

		public override bool InstancePerEntity => true;

		public override GlobalItem Clone(Item item, Item itemClone)
		{
			UpgradeEquipmentGlobalItem myClone = (UpgradeEquipmentGlobalItem)base.Clone(item, itemClone);
			myClone.UpgradeTier = UpgradeTier;
			return myClone;
		}

		public override void Load(Item item, TagCompound tag)
		{
			UpgradeTier = tag.GetByte("UpgradeTier");
		}

		public override bool NeedsSaving(Item item)
		{
			return UpgradeTier > 0;
		}

		public override TagCompound Save(Item item)
		{
			return new TagCompound {
				{"UpgradeTier", UpgradeTier},
			};
		}

		// crit multiplier
		public override void GetWeaponCrit(Item item, Player player, ref int crit)
		{
			if (UpgradeTier > 0)
			{
				if (!item.summon)
				{
					int _initialCrit = crit;

					if (UpgradeTier > 0)
					{
						crit = _initialCrit + PrefixHelper.GetCriticalMult(UpgradeTier);
					}
				}
			}
		}

		// use speed multiplier
		public override float UseTimeMultiplier(Item item, Player player)
		{
			if (UpgradeTier > 0)
			{
				return PrefixHelper.GetSpeedMult(UpgradeTier);
			}

			return 1f;
		}

		public override void NetSend(Item item, BinaryWriter writer)
		{
			writer.Write(UpgradeTier);
		}

		public override void NetReceive(Item item, BinaryReader reader)
		{
			UpgradeTier = reader.ReadByte();
		}
	}
}