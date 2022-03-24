using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MagicStorageExtra.Items
{
	public class StorageUnitCrimtane : StorageItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crimtane Storage Unit");
			DisplayName.AddTranslation(GameCulture.Russian, "Кримтановая Ячейка Хранилища");
			DisplayName.AddTranslation(GameCulture.Polish, "Jednostka magazynująca (Karmazynium)");
			DisplayName.AddTranslation(GameCulture.French, "Unité de stockage (Carmitane)");
			DisplayName.AddTranslation(GameCulture.Spanish, "Unidad de Almacenamiento (Carmesí)");
			DisplayName.AddTranslation(GameCulture.Chinese, "存储单元(血腥)");
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
			item.value = Item.sellPrice(0, 0, 32);
			item.createTile = ModContent.TileType<Components.StorageUnit>();
			item.placeStyle = 2;
		}

		public override void AddRecipe(ModItem result)
		{
			var recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("MagicStorageExtra:AnyStorageUnit");
			recipe.AddRecipeGroup("MagicStorageExtra:AnyUpgradeCrimtane");
			recipe.SetResult(result);
			recipe.AddRecipe();
		}
	}
}
