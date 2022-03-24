﻿using System.ComponentModel;
using Newtonsoft.Json;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnassignedField.Global

namespace MagicStorageExtra
{
	public class MagicStorageConfig : ModConfig
	{
		[Label("Display new items/recipes")]
		[Tooltip("Toggles whether new items in the storage will glow to indicate they're new")]
		[DefaultValue(true)]
		public bool glowNewItems;

		[Label("Use default filter")]
		[Tooltip("Enable to use the filter below, disable to remember last filter selected in game(filter is still used on first open after mod load)")]
		[DefaultValue(true)]
		public bool useConfigFilter;

		[Label("Default recipe filter")]
		[Tooltip("Enable to default to all recipes, disable to default to available recipes")]
		[DefaultValue(true)]
		public bool showAllRecipes;

		[Label("Quick stack deposit mode")]
		[Tooltip("Enable to quick stack with control(ctrl) pressed, disable to quick stack with control(ctrl) released")]
		[DefaultValue(false)]
		public bool quickStackDepositMode;

		[Label("Clear search text")]
		[Tooltip("Enable to clear the search text when opening the UI")]
		[DefaultValue(false)]
		public bool clearSearchText;

		[Label("Clear selected recipe")]
		[Tooltip("Enable to clear the last selected recipe when opening the Crafting Interface")]
		[DefaultValue(true)]
		public bool clearSelectedRecipe;

		[Label("Show estimated item dps")]
		[Tooltip("Enable to show the estimated dps of the item as a tooltip")]
		[DefaultValue(true)]
		public bool showItemDps;

		public static MagicStorageConfig Instance => ModContent.GetInstance<MagicStorageConfig>();

		[JsonIgnore] public static bool GlowNewItems => Instance.glowNewItems;

		[JsonIgnore] public static bool UseConfigFilter => Instance.useConfigFilter;

		[JsonIgnore] public static bool ShowAllRecipes => Instance.showAllRecipes;

		[JsonIgnore] public static bool QuickStackDepositMode => Instance.quickStackDepositMode;

		[JsonIgnore] public static bool ClearSearchText => Instance.clearSearchText;

		[JsonIgnore] public static bool ClearSelectedRecipe => Instance.clearSelectedRecipe;

		[JsonIgnore] public static bool ShowItemDps => Instance.showItemDps;

		public override ConfigScope Mode => ConfigScope.ClientSide;
	}
}
