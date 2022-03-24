using Terraria.ModLoader;

namespace UpgradeEquipment.Items
{
	internal class UpgradeToken : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Upgrade Token");
			Tooltip.SetDefault("Used upgrade equipment at the Upgrader");
		}

		public override void SetDefaults()
		{
			item.value = 50;
			item.maxStack = 99999;
			item.width = 25;
			item.height = 25;
		}
	}
}