using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using static Terraria.ModLoader.ModContent;

namespace UpgradeEquipment.Items
{
    public class UpgradeEquipmentGlobalItem : GlobalItem
    {
        public string originalOwner;
        public byte upgradeTier;
        public bool examplePersonFreeGift;

        public UpgradeEquipmentGlobalItem()
        {
            originalOwner = "";
            upgradeTier = 0;
        }

        public override int ChoosePrefix(Item item, UnifiedRandom rand)
        {
            if ((item.accessory || item.damage > 0) && item.maxStack == 1 && rand.NextBool(30))
            {
                return mod.PrefixType(rand.Next(2) == 0 ? "+1 " : "+2 ");
            }
            return -1;
        }


        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!item.social && item.prefix > 0)
            {
                int upgradeTierBonus = Main.cpItem.GetGlobalItem<UpgradeEquipmentGlobalItem>().upgradeTier;
                if (upgradeTierBonus > 0)
                {
                    TooltipLine line = new TooltipLine(mod, "upgradeTier", "Upgrade Tier " + upgradeTierBonus)
                    {
                        isModifier = true
                    };
                    tooltips.Add(line);
                }

            }
            if (originalOwner.Length > 0)
            {
                TooltipLine line = new TooltipLine(mod, "CraftedBy", "Crafted by: " + originalOwner)
                {
                    overrideColor = Color.LimeGreen
                };
                tooltips.Add(line);

                /*foreach (TooltipLine line2 in tooltips)
				{
					if (line2.mod == "Terraria" && line2.Name == "ItemName")
					{
						line2.text = originalOwner + "'s " + line2.text;
					}
				}*/
            }
        }

        public override bool InstancePerEntity => true;

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            UpgradeEquipmentGlobalItem myClone = (UpgradeEquipmentGlobalItem)base.Clone(item, itemClone);
            myClone.originalOwner = originalOwner;
            myClone.upgradeTier = upgradeTier;
            myClone.examplePersonFreeGift = examplePersonFreeGift;
            return myClone;
        }

        public override void Load(Item item, TagCompound tag)
        {
            originalOwner = tag.GetString("originalOwner");
            examplePersonFreeGift = tag.GetBool(nameof(examplePersonFreeGift));
        }

        public override bool NeedsSaving(Item item)
        {
            return originalOwner.Length > 0 || examplePersonFreeGift;
        }

        public override TagCompound Save(Item item)
        {
            return new TagCompound {
                {"originalOwner", originalOwner},
                {nameof(examplePersonFreeGift), examplePersonFreeGift},
            };
        }

        public override void OnCraft(Item item, Recipe recipe)
        {
            if (item.maxStack == 1)
            {
                originalOwner = Main.LocalPlayer.name;
            }
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(originalOwner);
            writer.Write(upgradeTier);
            writer.Write(examplePersonFreeGift);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            originalOwner = reader.ReadString();
            upgradeTier = reader.ReadByte();
            examplePersonFreeGift = reader.ReadBoolean();
        }
    }
}