using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MagicStorageExtra.Items
{
	public class CraftingAccess : StorageItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Storage Crafting Interface");
			DisplayName.AddTranslation(GameCulture.Russian, "Модуль Создания Предметов");
			DisplayName.AddTranslation(GameCulture.Polish, "Interfejs Rzemieślniczy Magazynu");
			DisplayName.AddTranslation(GameCulture.French, "Interface de Stockage Artisanat");
			DisplayName.AddTranslation(GameCulture.Spanish, "Interfaz de Elaboración de almacenamiento");
			DisplayName.AddTranslation(GameCulture.Chinese, "制作存储单元");
		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 26;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(0, 1, 16, 25);
			item.createTile = ModContent.TileType<Components.CraftingAccess>();
		}

		public override void AddRecipe(ModItem result)
		{
			var recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("MagicStorageExtra:AnyStorageComponent");
			recipe.AddRecipeGroup("MagicStorageExtra:AnyDiamond");
			if (MagicStorageExtra.legendMod is null)
				recipe.AddIngredient(ItemID.Sapphire, 3);
			else
				recipe.AddRecipeGroup("MagicStorageExtra:AnySapphire", 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(result);
			recipe.AddRecipe();
		}
	}
}
