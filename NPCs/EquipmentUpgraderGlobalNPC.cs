using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace UpgradeEquipment.NPCs
{
    class EquipmentUpgraderGlobalNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
         {
             Random random = new Random();
            if (npc.type == NPCID.MoonLordCore)
            {
                Item.NewItem(Main.LocalPlayer.getRect(), ItemType<Items.UpgradeToken>(), 350);
            }
            if (npc.boss)
            {
                if (npc.type == NPCID.TheDestroyer)
                {
                    Item.NewItem(npc.getRect(), ItemType<Items.UpgradeToken>(), 60);
                }
                else if (npc.type == NPCID.EaterofWorldsHead)
                {
                    Item.NewItem(npc.getRect(), ItemType<Items.UpgradeToken>(), 15);
                }
                else
                {
                    Item.NewItem(npc.getRect(), ItemType<Items.UpgradeToken>(), Main.rand.Next(npc.lifeMax / 900, npc.lifeMax / 800));
                }
            }
            else if (Main.rand.Next(10) == 0 && npc.lifeMax >= 1000 && npc.modNPC == null)
            {
                Item.NewItem(npc.getRect(), ItemType<Items.UpgradeToken>(), Main.rand.Next(npc.lifeMax / 900, npc.lifeMax / 800));
            }
            else if(Main.rand.Next(100) == 0 && npc.lifeMax >= 75 && npc.modNPC == null)
            {
                 Item.NewItem(npc.getRect(), ItemType<Items.UpgradeToken>(), 1);
            }
            else if (Main.rand.Next(100) == 0 && npc.lifeMax >= 15000 && npc.modNPC != null)
            {
                Item.NewItem(npc.getRect(), ItemType<Items.UpgradeToken>(), Main.rand.Next(npc.lifeMax / 1250, npc.lifeMax / 1100));
            }
            else if (Main.rand.Next(100) == 0 && npc.lifeMax >= 5000 && npc.modNPC != null)
            {
                Item.NewItem(npc.getRect(), ItemType<Items.UpgradeToken>(), Main.rand.Next(npc.lifeMax / 900, npc.lifeMax / 800));
            }
            else if (Main.rand.Next(60) == 0 && npc.lifeMax >= 1000 && npc.modNPC != null)
            {
                Item.NewItem(npc.getRect(), ItemType<Items.UpgradeToken>(), 1);
            }
        }
    }
}
