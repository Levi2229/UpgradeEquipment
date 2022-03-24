using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace UpgradeEquipment.NPCs
{
	internal class UpgradeEquipmentGlobalNPC : GlobalNPC
	{
		public override void NPCLoot(NPC npc)
		{
			// prehardmode mobs 1/120 chance of 1
			// hardmode mobs 1/60 chance of 1
			// bosses drop based on hp, bonus in hardmode

			if (npc.boss)
			{
				int bossBase = npc.lifeMax / 1000;
				int bossBonus = bossBase / 2;
				int hardmodeBonus = bossBase / 2;
				int moonLordBonus = 200;

				if (Main.hardMode)
				{
					if (npc.type == NPCID.MoonLordCore)
					{
						Item.NewItem(npc.getRect(), ItemType<Items.UpgradeToken>(), moonLordBonus);
					}
					Item.NewItem(npc.getRect(), ItemType<Items.UpgradeToken>(), bossBase + Main.rand.Next(bossBonus) + Main.rand.Next(hardmodeBonus));
				}
				else
				{
					Item.NewItem(npc.getRect(), ItemType<Items.UpgradeToken>(), bossBase + Main.rand.Next(bossBonus));
				}
			}
			else
			{
				if (Main.hardMode)
				{
					if (Main.rand.Next(60) == 0)
					{
						Item.NewItem(npc.getRect(), ItemType<Items.UpgradeToken>(), 1);
					}
				}
				else
				{
					if (Main.rand.Next(120) == 0)
					{
						Item.NewItem(npc.getRect(), ItemType<Items.UpgradeToken>(), 1);
					}
				}
			}
		}
	}
}