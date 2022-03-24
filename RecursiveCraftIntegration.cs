﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MagicStorageExtra.Components;
using RecursiveCraft;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;
using OnPlayer = On.Terraria.Player;

namespace MagicStorageExtra
{
	public static class RecursiveCraftIntegration
	{
		// Here we store a reference to the RecursiveCraft Mod instance. We can use it for many things.
		// You can call all the Mod methods on it just like we do with our own Mod instance: RecursiveCraftMod.ItemType("ExampleItem")
		private static Mod RecursiveCraftMod;

		// Here we define a bool property to quickly check if RecursiveCraft is loaded.
		public static bool Enabled => RecursiveCraftMod != null;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Load()
		{
			RecursiveCraftMod = ModLoader.GetMod("RecursiveCraft");
			if (Enabled)
				Load_Inner(); // Move that logic into another method to prevent this.
		}

		// Be aware of inlining. Inlining can happen at the whim of the runtime. Without this Attribute, this mod happens to crash the 2nd time it is loaded on Linux/Mac. (The first call isn't inlined just by chance.) This can cause headaches.
		// To avoid TypeInitializationException (or ReflectionTypeLoadException) problems, we need to specify NoInlining on methods like this to prevent inlining (methods containing or accessing Types in the Weakly referenced assembly).
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void Load_Inner()
		{
			// This method will only be called when Enable is true, preventing TypeInitializationException
			Members.recipeCache = new Dictionary<Recipe, RecipeInfo>();
			OnPlayer.QuickSpawnItem_int_int += OnPlayerOnQuickSpawnItem_int_int;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void InitRecipes()
		{
			if (Enabled)
				InitRecipes_Inner();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void InitRecipes_Inner()
		{
			Members.compoundRecipe = new CompoundRecipe(RecursiveCraftMod);
			Members.threadCompoundRecipe = new CompoundRecipe(RecursiveCraftMod);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Unload()
		{
			if (Enabled) // Here we properly unload, making sure to check Enabled before setting RecursiveCraftMod to null.
				Unload_Inner(); // Once again we must separate out this logic.
			RecursiveCraftMod = null; // Make sure to null out any references to allow Garbage Collection to work.
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void Unload_Inner()
		{
			Members.recipeCache = null;
			Members.compoundRecipe = null;
			Members.threadCompoundRecipe = null;
			OnPlayer.QuickSpawnItem_int_int -= OnPlayerOnQuickSpawnItem_int_int;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void OnPlayerOnQuickSpawnItem_int_int(OnPlayer.orig_QuickSpawnItem_int_int orig, Player self, int type, int stack)
		{
			if (CraftingGUI.compoundCrafting)
			{
				var item = new Item();
				item.SetDefaults(type);
				item.stack = stack;
				CraftingGUI.compoundCraftSurplus.Add(item);
				return;
			}

			orig(self, type, stack);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Dictionary<int, int> FlatDict(IEnumerable<Item> items)
		{
			var dictionary = new Dictionary<int, int>();
			foreach (Item item in items)
				if (dictionary.ContainsKey(item.type))
					dictionary[item.type] += item.stack;
				else
					dictionary[item.type] = item.stack;
			return dictionary;
		}

		// Make sure to extract the .dll from the .tmod and then add them to your .csproj as references.
		// As a convention, I rename the .dll file ModName_v1.2.3.4.dll and place them in Mod Sources/Mods/lib.
		// I do this for organization and so the .csproj loads properly for others using the GitHub repository.
		// Remind contributors to download the referenced mod itself if they wish to build the mod.
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void RecursiveRecipes()
		{
			if (Main.rand == null)
				Main.rand = new UnifiedRandom((int)DateTime.UtcNow.Ticks);
			Dictionary<int, int> storedItems = GetStoredItems();
			if (storedItems == null)
				return;

			var recipeCache = (Dictionary<Recipe, RecipeInfo>)Members.recipeCache;
			lock (recipeCache)
			{
				var recursiveSearch = new RecursiveSearch(storedItems, GuiAsCraftingSource());
				recipeCache.Clear();
				foreach (int i in RecursiveCraft.RecursiveCraft.SortedRecipeList)
				{
					Recipe recipe = Main.recipe[i];
					if (recipe is CompoundRecipe compound)
						SingleSearch(recursiveSearch, compound.OverridenRecipe);
					else
						SingleSearch(recursiveSearch, recipe);
				}
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Dictionary<int, int> GetStoredItems()
		{
			Player player = Main.LocalPlayer;
			var modPlayer = player.GetModPlayer<StoragePlayer>();
			TEStorageHeart heart = modPlayer.GetStorageHeart();
			if (heart == null)
				return null;
			return FlatDict(heart.GetStoredItems());
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static CraftingSource GuiAsCraftingSource() =>
			new CraftingSource
			{
				AdjTile = CraftingGUI.adjTiles,
				AdjWater = CraftingGUI.adjWater,
				AdjHoney = CraftingGUI.adjHoney,
				AdjLava = CraftingGUI.adjLava,
				ZoneSnow = CraftingGUI.zoneSnow,
				AlchemyTable = CraftingGUI.alchemyTable,
			};

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void SingleSearch(RecursiveSearch recursiveSearch, Recipe recipe)
		{
			RecipeInfo recipeInfo;
			lock (BlockRecipes.activeLock)
			{
				BlockRecipes.active = false;
				recipeInfo = recursiveSearch.FindIngredientsForRecipe(recipe);
				BlockRecipes.active = true;
			}

			if (recipeInfo != null && recipeInfo.RecipeUsed.Count > 1)
			{
				var recipeCache = (Dictionary<Recipe, RecipeInfo>)Members.recipeCache;
				recipeCache.Add(recipe, recipeInfo);
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static bool IsCompoundRecipe(Recipe recipe) => recipe is CompoundRecipe;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Recipe GetOverriddenRecipe(Recipe recipe) => recipe is CompoundRecipe compound ? compound.OverridenRecipe : recipe;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static bool UpdateRecipe(Recipe recipe)
		{
			if (recipe is CompoundRecipe compound)
				recipe = compound.OverridenRecipe;
			else
				return false;

			Dictionary<int, int> storedItems = GetStoredItems();
			var recipeCache = (Dictionary<Recipe, RecipeInfo>)Members.recipeCache;

			if (storedItems != null)
				lock (recipeCache)
				{
					recipeCache.Remove(recipe);
					var recursiveSearch = new RecursiveSearch(storedItems, GuiAsCraftingSource());
					SingleSearch(recursiveSearch, recipe);
				}

			return recipeCache.ContainsKey(recipe);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Recipe ApplyCompoundRecipe(Recipe recipe)
		{
			if (recipe is CompoundRecipe compound)
				recipe = compound.OverridenRecipe;

			var recipeCache = (Dictionary<Recipe, RecipeInfo>)Members.recipeCache;

			if (recipeCache.TryGetValue(recipe, out RecipeInfo recipeInfo))
			{
				int index = Array.IndexOf(Main.recipe, recipe);
				var compoundRecipe = (CompoundRecipe)Members.compoundRecipe;

				compoundRecipe.Apply(index, recipeInfo);
				return compoundRecipe;
			}

			return recipe;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Recipe ApplyThreadCompoundRecipe(Recipe recipe)
		{
			var recipeCache = (Dictionary<Recipe, RecipeInfo>)Members.recipeCache;

			if (recipeCache.TryGetValue(recipe, out RecipeInfo recipeInfo))
			{
				int index = Array.IndexOf(Main.recipe, recipe);
				var threadCompoundRecipe = (CompoundRecipe)Members.threadCompoundRecipe;

				threadCompoundRecipe.Apply(index, recipeInfo);
				return threadCompoundRecipe;
			}

			return recipe;
		}

		private static class Members
		{
			public static object recipeCache;
			public static object compoundRecipe;
			public static object threadCompoundRecipe;
		}
	}
}
