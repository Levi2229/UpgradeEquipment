using Terraria.ModLoader;

namespace MagicStorageExtra.Items
{
	public abstract class StorageItem : ModItem
	{
		public sealed override void AddRecipes()
		{
			AddRecipe(this);
			if (MagicStorageExtra.MagicStorage != null)
			{
				ModItem modItem = MagicStorageExtra.MagicStorage.GetItem(Name);
				AddRecipe(modItem);
				AddReverseRecipes(modItem);
			}
		}

		public void AddReverseRecipes(ModItem other)
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.SetResult(other);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(other);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public virtual void AddRecipe(ModItem result)
		{
		}
	}
}
