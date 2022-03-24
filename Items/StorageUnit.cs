﻿using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MagicStorageExtra.Items
{
	public class StorageUnit : StorageItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.AddTranslation(GameCulture.Russian, "Ячейка Хранилища");
			DisplayName.AddTranslation(GameCulture.Polish, "Jednostka magazynująca");
			DisplayName.AddTranslation(GameCulture.French, "Unité de stockage");
			DisplayName.AddTranslation(GameCulture.Spanish, "Unidad de Almacenamiento");
			DisplayName.AddTranslation(GameCulture.Chinese, "存储单元");
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
			item.rare = ItemRarityID.White;
			item.value = Item.sellPrice(0, 0, 6);
			item.createTile = ModContent.TileType<Components.StorageUnit>();
		}

		public override void AddRecipe(ModItem result)
		{
			var recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("MagicStorageExtra:AnyStorageComponent");
			recipe.AddRecipeGroup("MagicStorageExtra:AnyChest");
			recipe.AddIngredient(ItemID.SilverBar, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(result);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("MagicStorageExtra:AnyStorageComponent");
			recipe.AddRecipeGroup("MagicStorageExtra:AnyChest");
			recipe.AddIngredient(ItemID.TungstenBar, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(result);
			recipe.AddRecipe();
		}
	}
}
