using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;
using System;
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

        private float initialShootSpeed = 0f;
        private float initialManaCost = 0f;
        private int initialCrit = 0;

        public UpgradeEquipmentGlobalItem()
        {
            upgradeTier = 0;
        }

        public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int ugt = item.GetGlobalItem<UpgradeEquipmentGlobalItem>().upgradeTier;

            float _initialShootSpeed = item.GetGlobalItem<UpgradeEquipmentGlobalItem>().initialShootSpeed;

            if (_initialShootSpeed == 0f)
            {
                item.GetGlobalItem<UpgradeEquipmentGlobalItem>().initialShootSpeed = item.shootSpeed;
                _initialShootSpeed = item.shootSpeed;
            }

            if (ugt > 0)
            {
                float velMult = PrefixHelper.getVelocityMult(ugt);
                item.shootSpeed = _initialShootSpeed *= velMult;
            }
            return true;
        }

        public override void OnConsumeMana(Item item, Player player, int manaConsumed)
        {

        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (upgradeTier > 0)
            {


                int ugt = item.GetGlobalItem<UpgradeEquipmentGlobalItem>().upgradeTier;
                float _initialManaCost = item.GetGlobalItem<UpgradeEquipmentGlobalItem>().initialManaCost;

                if (_initialManaCost == 0f)
                {
                    item.GetGlobalItem<UpgradeEquipmentGlobalItem>().initialManaCost = item.mana;
                    _initialManaCost = item.mana;
                }

                if (ugt > 0)
                {
                    float velMult = PrefixHelper.getVelocityMult(ugt);
                    item.mana = (int)Math.Round(_initialManaCost * -(PrefixHelper.getVelocityMult(upgradeTier) - 2f));
                }

                TooltipLine line = new TooltipLine(mod, "upgradeTier", "Upgrade Tier " + upgradeTier)
                {
                    isModifier = true,
                    overrideColor = PrefixHelper.getTierColor(upgradeTier),
                };
                tooltips.Add(line);

                float calcedDamageDisplay = 1f * (PrefixHelper.getDamageMult(upgradeTier) -1f ) * 100;

                TooltipLine line2 = new TooltipLine(mod, "damage", "+" + Math.Round(calcedDamageDisplay) + "% Damage")
                {
                    isModifier = true,
                    overrideColor = Color.BlueViolet
                };
                tooltips.Add(line2);

                float calcedVelocityDisplay = (PrefixHelper.getVelocityMult(upgradeTier) -1f) * 100;

                if ((!item.melee || item.shootSpeed > 0) && Math.Round(calcedVelocityDisplay) > 0)
                {
                    TooltipLine line3 = new TooltipLine(mod, "velocity", "+" + Math.Round(calcedVelocityDisplay) + "% Velocity")
                    {
                        isModifier = true,
                        overrideColor = Color.BlueViolet
                    };
                    tooltips.Add(line3);
                }
                TooltipLine line4 = new TooltipLine(mod, "crit", "+" + ((upgradeTier / 2) + 1) + "% Crit Chance")
                {
                    isModifier = true,
                    overrideColor = Color.BlueViolet
                };
                tooltips.Add(line4);


                TooltipLine line5 = new TooltipLine(mod, "knockback", "+" + 1f * upgradeTier + "% Knockback")
                {
                    isModifier = true,
                    overrideColor = Color.BlueViolet
                };
                tooltips.Add(line5);

                float calcedSpeedDisplay = (PrefixHelper.getSpeedMult(upgradeTier) - 1f) * 100;
                if (calcedSpeedDisplay >= 1)
                {
                    TooltipLine line6 = new TooltipLine(mod, "velocity", "+" + Math.Round(calcedSpeedDisplay) + "% Speed")
                    {
                        isModifier = true,
                        overrideColor = Color.BlueViolet
                    };
                    tooltips.Add(line6);
                }
                if (item.mana > 0)
                {
                    TooltipLine line7 = new TooltipLine(mod, "manacost", "-" + Math.Round(calcedVelocityDisplay) + "% Mana cost")
                    {
                        isModifier = true,
                        overrideColor = Color.BlueViolet
                    };
                    tooltips.Add(line7);
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

        // As a modder, you could also opt to make these overrides also sealed. Up to the modder
        public override void ModifyWeaponDamage(Item item, Player player, ref float add, ref float mult, ref float flat)
        {
            if (upgradeTier > 0)
            {
                mult *= PrefixHelper.getDamageMult(upgradeTier);
            }
        }

        public override void GetWeaponKnockback(Item item, Player player, ref float knockback)
        {
            // Adds knockback bonuses
            if (!disableKnockbackChange && upgradeTier > 1)
            {
                knockback *= 1f + 0.01f * upgradeTier;;
            }
        }

        public override float UseTimeMultiplier(Item item, Player player)
        {
            if (PrefixHelper.getSpeedMult(upgradeTier) > 1f)
            {
                float speedmult = PrefixHelper.getSpeedMult(upgradeTier);
                return speedmult;
            }
            else
            {
                return 1f;
            }
        }


        public override void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref int damage, ref float knockback)
        {
            int ugt = weapon.GetGlobalItem<UpgradeEquipmentGlobalItem>().upgradeTier;
            if (ugt > 0)
            {
                speed = 1f * PrefixHelper.getVelocityMult(ugt) * 10;
                weapon.shootSpeed = 15f;
            }
        }

        public override void GetWeaponCrit(Item item, Player player, ref int crit)
        {
            if (upgradeTier > 0)
            {
                int _initialCrit = item.GetGlobalItem<UpgradeEquipmentGlobalItem>().initialCrit;

                if (_initialCrit == 0f)
                {
                    item.GetGlobalItem<UpgradeEquipmentGlobalItem>().initialCrit = crit;
                    _initialCrit = crit;
                }

                if (upgradeTier > 0)
                {
                    crit = _initialCrit + (upgradeTier / 2) + 1;
                }
            }
        }

        public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (upgradeTier > 0)
            {
                scale = 1f + 0.01f * upgradeTier;
            }
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