using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using UpgradeEquipment.UI;

namespace UpgradeEquipment.Items
{
	public class UpgradeEquipmentGlobalItem : GlobalItem
	{
		public byte upgradeTier;
		internal static bool disableSizeChange;
		internal static bool disableKnockbackChange;
		private int initialCrit = 0;

		private Color tooltipColor = Color.CornflowerBlue;

		public UpgradeEquipmentGlobalItem()
		{
			upgradeTier = 0;
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (upgradeTier > 0)
			{
				// upgrade tier tooltip
				TooltipLine line = new TooltipLine(mod, "upgradeTier", "Upgrade Tier " + upgradeTier)
				{
					isModifier = true,
					overrideColor = PrefixHelper.GetTierColor(upgradeTier),
				};
				tooltips.Add(line);

				// tooltip for non-summon weapons
				if (!item.summon)
				{
					float calcedDamageDisplay = 1f * (PrefixHelper.GetFinalDamageMult(upgradeTier) - 1f) * 100;
					TooltipLine line2 = new TooltipLine(mod, "damage", "+" + Math.Round(calcedDamageDisplay) + "% final damage")
					{
						isModifier = true,
						overrideColor = tooltipColor
					};
					tooltips.Add(line2);

					if (upgradeTier / 2 > 0)
					{

						TooltipLine line4 = new TooltipLine(mod, "crit", "+" + PrefixHelper.GetCriticalMult(upgradeTier) + "% critical strike chance")
						{
							isModifier = true,
							overrideColor = tooltipColor
						};
						tooltips.Add(line4);
					}
				}
				// tooltip for summon weapons
				else
				{
					float calcedDamageDisplay = 1f * (PrefixHelper.GetSummonDamageMult(upgradeTier) - 1f) * 100;
					TooltipLine line2 = new TooltipLine(mod, "damage", "+" + Math.Round(calcedDamageDisplay) + "% damage")
					{
						isModifier = true,
						overrideColor = tooltipColor
					};
					tooltips.Add(line2);
				}

				// use speed tooltip
				float calcedSpeedDisplay = (PrefixHelper.GetSpeedMult(upgradeTier) - 1f) * 100;
				if (calcedSpeedDisplay >= 1)
				{
					TooltipLine line6 = new TooltipLine(mod, "velocity", "+" + Math.Round(calcedSpeedDisplay) + "% speed")
					{
						isModifier = true,
						overrideColor = tooltipColor
					};
					tooltips.Add(line6);
				}
			}
		}

		public override bool InstancePerEntity => true;

		public override GlobalItem Clone(Item item, Item itemClone)
		{
			UpgradeEquipmentGlobalItem myClone = (UpgradeEquipmentGlobalItem)base.Clone(item, itemClone);
			myClone.upgradeTier = upgradeTier;
			return myClone;
		}

		public override void Load(Item item, TagCompound tag)
		{
			upgradeTier = tag.GetByte("upgradeTier");
		}

		public override bool NeedsSaving(Item item)
		{
			return upgradeTier > 0;
		}

		public override TagCompound Save(Item item)
		{
			return new TagCompound {
				{"upgradeTier", upgradeTier},
			};
		}

		public override void ModifyWeaponDamage(Item item, Player player, ref float add, ref float mult, ref float flat)
		{
			if (upgradeTier > 0)
			{
				if (item.summon)
				{
					mult *= PrefixHelper.GetSummonDamageMult(upgradeTier);
				}
			}
		}

		// crit multiplier
		public override void GetWeaponCrit(Item item, Player player, ref int crit)
		{
			if (upgradeTier > 0)
			{
				if (!item.summon)
				{
					int _initialCrit = item.GetGlobalItem<UpgradeEquipmentGlobalItem>().initialCrit;

					if (_initialCrit == 0f)
					{
						item.GetGlobalItem<UpgradeEquipmentGlobalItem>().initialCrit = crit;
						_initialCrit = crit;
					}

					if (upgradeTier > 0)
					{
						crit = _initialCrit + PrefixHelper.GetCriticalMult(upgradeTier);
					}
				}
			}
		}

		// use speed multiplier
		public override float UseTimeMultiplier(Item item, Player player)
		{
			if (upgradeTier > 0)
			{
				return PrefixHelper.GetSpeedMult(upgradeTier);
			}
			return 1f;
		}

		public override void NetSend(Item item, BinaryWriter writer)
		{
			writer.Write(upgradeTier);
		}

		public override void NetReceive(Item item, BinaryReader reader)
		{
			upgradeTier = reader.ReadByte();
		}

		/* knockback multiplier
        public override void GetWeaponKnockback(Item item, Player player, ref float knockback)
        {
            // Adds knockback bonuses
            if (!disableKnockbackChange && upgradeTier > 1)
            {
                knockback *= 1f + 0.01f * upgradeTier;;
            }
        }
        */

		/* size multiplier
        public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (upgradeTier > 0)
            {
                scale = 1f + 0.01f * upgradeTier;
            }
            return true;
        }
        */

		/* velocity multi
        public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int ugt = item.GetGlobalItem<UpgradeEquipmentGlobalItem>().upgradeTier;

            if (ugt > 0)
            {
                float _initialShootSpeed = item.GetGlobalItem<UpgradeEquipmentGlobalItem>().initialShootSpeed;

                if (_initialShootSpeed == 0f)
                {
                    item.GetGlobalItem<UpgradeEquipmentGlobalItem>().initialShootSpeed = item.shootSpeed;
                    _initialShootSpeed = item.shootSpeed;
                }

                float velMult = PrefixHelper.getVelocityMult(ugt);
                if (velMult > 0) {
                    item.shootSpeed = _initialShootSpeed *= velMult;
                }
            }
            return true;
        }

        public override void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref int damage, ref float knockback)
        {
            int ugt = weapon.GetGlobalItem<UpgradeEquipmentGlobalItem>().upgradeTier;
             if (ugt > 0)
            {
                float _initialAmmoShootSpeed = weapon.GetGlobalItem<UpgradeEquipmentGlobalItem>().initialAmmoShootSpeed;
                if (_initialAmmoShootSpeed == 0f)
                {
                    _initialAmmoShootSpeed  = weapon.shootSpeed;
                    weapon.GetGlobalItem<UpgradeEquipmentGlobalItem>().initialAmmoShootSpeed = weapon.shootSpeed;
                }
                speed = _initialAmmoShootSpeed * PrefixHelper.getVelocityMult(ugt);
                weapon.shootSpeed = speed;
            }
        }
        */
	}
}