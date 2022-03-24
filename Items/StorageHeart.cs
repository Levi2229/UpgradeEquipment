﻿using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MagicStorageExtra.Items
{
	public class StorageHeart : StorageItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.AddTranslation(GameCulture.Russian, "Сердце Хранилища");
			DisplayName.AddTranslation(GameCulture.Polish, "Serce Jednostki Magazynującej");
			DisplayName.AddTranslation(GameCulture.French, "Cœur de Stockage");
			DisplayName.AddTranslation(GameCulture.Spanish, "Corazón de Almacenamiento");
			DisplayName.AddTranslation(GameCulture.Chinese, "存储核心");
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
			item.value = Item.sellPrice(0, 1, 35);
			item.createTile = ModContent.TileType<Components.StorageHeart>();
		}

		public override void AddRecipe(ModItem result)
		{
			var recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("MagicStorageExtra:AnyStorageComponent");
			recipe.AddRecipeGroup("MagicStorageExtra:AnyDiamond", 2);
			if (MagicStorageExtra.legendMod is null)
				recipe.AddIngredient(ItemID.Emerald, 3);
			else
				recipe.AddRecipeGroup("MagicStorageExtra:AnyEmerald", 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(result);
			recipe.AddRecipe();
		}
	}
}
