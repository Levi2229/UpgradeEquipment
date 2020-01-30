using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using UpgradeEquipment.UI;
using static Terraria.ModLoader.ModContent;

namespace UpgradeEquipment.Items
{
    public class UpgradeEquipmentGlobalItem : GlobalItem
    {
        public byte upgradeTier;

        internal static bool disableSizeChange;
        internal static bool disableKnockbackChange;

        public UpgradeEquipmentGlobalItem()
        {
            upgradeTier = 0;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (upgradeTier > 0)
            {

                TooltipLine line = new TooltipLine(mod, "upgradeTier", "Upgrade Tier " + upgradeTier)
                {
                    isModifier = true,
                    overrideColor = Color.Firebrick
                };
                tooltips.Add(line);

                float calcedDamageDisplay = 1f * (PrefixHelper.getDamageMult(upgradeTier) -1f ) * 100;

                TooltipLine line2 = new TooltipLine(mod, "damage", "+" + calcedDamageDisplay + "% Damage")
                {
                    isModifier = true,
                    overrideColor = Color.BlueViolet
                };
                tooltips.Add(line2);

                float calcedSpeedDisplay = PrefixHelper.getSpeedMult(upgradeTier);

                TooltipLine line3 = new TooltipLine(mod, "damage", "+" + (int)calcedSpeedDisplay + "% Speed")
                {
                    isModifier = true,
                    overrideColor = Color.BlueViolet
                };
                tooltips.Add(line3);

                TooltipLine line4 = new TooltipLine(mod, "crit", "+" + upgradeTier + "% Crit Chance")
                {
                    isModifier = true,
                    overrideColor = Color.BlueViolet
                };
                tooltips.Add(line4);

                TooltipLine line5 = new TooltipLine(mod, "crit", "+" + 1f * upgradeTier + "% Knockback")
                {
                    isModifier = true,
                    overrideColor = Color.BlueViolet
                };
                tooltips.Add(line5);
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

        // As a modder, you could also opt to make these overrides also sealed. Up to the modder
        public override void ModifyWeaponDamage(Item item, Player player, ref float add, ref float mult, ref float flat)
        {
            mult *= PrefixHelper.getDamageMult(upgradeTier);
            System.Console.WriteLine(mult +"");
        }

        public override void GetWeaponKnockback(Item item, Player player, ref float knockback)
        {
            // Adds knockback bonuses
            if (!disableKnockbackChange && upgradeTier > 1)
            {
                knockback *= 1f + 0.01f * upgradeTier;;
            }
        }

        public override float MeleeSpeedMultiplier(Item item, Player player)
        {
            if (PrefixHelper.getSpeedMult(upgradeTier) > 1f) {
                float speedmult = PrefixHelper.getSpeedMult(upgradeTier);
                return speedmult;
            } else
            {
                return 1f;
            }
        }

        public override void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref int damage, ref float knockback)
        {
            int ugt = weapon.GetGlobalItem<UpgradeEquipmentGlobalItem>().upgradeTier;
            speed = 1f * PrefixHelper.getVelocityMult(ugt) * 10;
            weapon.shootSpeed = 1f * PrefixHelper.getSpeedMult(ugt) * 10;
        }

        public override void GetWeaponCrit(Item item, Player player, ref int crit)
        {
            // Adds crit bonuses
            crit = upgradeTier;
        }

        public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            scale = 1f + 0.01f * upgradeTier;
            return true;
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(upgradeTier);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            upgradeTier = reader.ReadByte();
        }
    }
}