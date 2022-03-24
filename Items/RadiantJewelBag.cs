using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagicStorageExtra.Items
{
	public class RadiantJewelBag : GlobalItem
	{
		public override void OpenVanillaBag(string context, Player player, int arg)
		{
			if (MagicStorageExtra.MagicStorage != null)
				return;

			if (context == "bossBag" && arg == ItemID.MoonLordBossBag && Main.rand.Next(10) == 0)
				player.QuickSpawnItem(ModContent.ItemType<RadiantJewel>());
		}
	}
}
