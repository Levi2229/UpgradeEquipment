using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MagicStorageExtra.Items
{
	public class StorageUnitLuminite : StorageItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luminite Storage Unit");
			DisplayName.AddTranslation(GameCulture.Russian, "Люминитовая Ячейка Хранилища");
			DisplayName.AddTranslation(GameCulture.Polish, "Jednostka magazynująca (Luminowana)");
			DisplayName.AddTranslation(GameCulture.French, "Unité de stockage (Luminite)");
			DisplayName.AddTranslation(GameCulture.Spanish, "Unidad de Almacenamiento (Luminita)");
			DisplayName.AddTranslation(GameCulture.Chinese, "存储单元(夜明)");
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
			item.rare = ItemRarityID.Red;
			item.value = Item.sellPrice(0, 2, 50);
			item.createTile = ModContent.TileType<Components.StorageUnit>();
			item.placeStyle = 6;
		}

		public override void AddRecipe(ModItem result)
		{
			var recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("MagicStorageExtra:AnyStorageUnitBlueChlorophyte");
			recipe.AddRecipeGroup("MagicStorageExtra:AnyUpgradeLuminite");
			recipe.SetResult(result);
			recipe.AddRecipe();
		}
	}
}
