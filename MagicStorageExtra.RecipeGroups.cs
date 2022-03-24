using MagicStorageExtra.Items;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MagicStorageExtra
{
	public partial class MagicStorageExtra
	{
		public override void AddRecipeGroups()
		{
			RecipeGroup group = new RecipeGroup(() => Language.GetText("LegacyMisc.37") + " Chest", ItemID.Chest, ItemID.GoldChest, ItemID.ShadowChest,
				ItemID.EbonwoodChest, ItemID.RichMahoganyChest, ItemID.PearlwoodChest, ItemID.IvyChest, ItemID.IceChest, ItemID.LivingWoodChest,
				ItemID.SkywareChest, ItemID.ShadewoodChest, ItemID.WebCoveredChest, ItemID.LihzahrdChest, ItemID.WaterChest, ItemID.JungleChest,
				ItemID.CorruptionChest, ItemID.CrimsonChest, ItemID.HallowedChest, ItemID.FrozenChest, ItemID.DynastyChest, ItemID.HoneyChest,
				ItemID.SteampunkChest, ItemID.PalmWoodChest, ItemID.MushroomChest, ItemID.BorealWoodChest, ItemID.SlimeChest, ItemID.GreenDungeonChest,
				ItemID.PinkDungeonChest, ItemID.BlueDungeonChest, ItemID.BoneChest, ItemID.CactusChest, ItemID.FleshChest, ItemID.ObsidianChest,
				ItemID.PumpkinChest, ItemID.SpookyChest, ItemID.GlassChest, ItemID.MartianChest, ItemID.GraniteChest, ItemID.MeteoriteChest,
				ItemID.MarbleChest);
			RecipeGroup.RegisterGroup("MagicStorageExtra:AnyChest", group);

			group = new RecipeGroup(() => Language.GetText("LegacyMisc.37").Value + " " + Language.GetTextValue("Mods.MagicStorageExtra.SnowBiomeBlock"),
				ItemID.SnowBlock, ItemID.IceBlock, ItemID.PurpleIceBlock, ItemID.PinkIceBlock);
			if (bluemagicMod != null)
				group.ValidItems.Add(bluemagicMod.ItemType("DarkBlueIce"));
			RecipeGroup.RegisterGroup("MagicStorageExtra:AnySnowBiomeBlock", group);

			group = new RecipeGroup(() => Language.GetText("LegacyMisc.37").Value + " " + Lang.GetItemNameValue(ItemID.Diamond), ItemID.Diamond,
				ItemType("ShadowDiamond"));
			if (legendMod != null)
			{
				group.ValidItems.Add(legendMod.ItemType("GemChrysoberyl"));
				group.ValidItems.Add(legendMod.ItemType("GemAlexandrite"));
			}

			RecipeGroup.RegisterGroup("MagicStorageExtra:AnyDiamond", group);
			if (legendMod != null)
			{
				group = new RecipeGroup(() => Language.GetText("LegacyMisc.37").Value + " " + Lang.GetItemNameValue(ItemID.Amethyst), ItemID.Amethyst,
					legendMod.ItemType("GemOnyx"), legendMod.ItemType("GemSpinel"));
				RecipeGroup.RegisterGroup("MagicStorageExtra:AnyAmethyst", group);

				group = new RecipeGroup(() => Language.GetText("LegacyMisc.37").Value + " " + Lang.GetItemNameValue(ItemID.Topaz), ItemID.Topaz,
					legendMod.ItemType("GemGarnet"));
				RecipeGroup.RegisterGroup("MagicStorageExtra:AnyTopaz", group);

				group = new RecipeGroup(() => Language.GetText("LegacyMisc.37").Value + " " + Lang.GetItemNameValue(ItemID.Sapphire), ItemID.Sapphire,
					legendMod.ItemType("GemCharoite"));
				RecipeGroup.RegisterGroup("MagicStorageExtra:AnySapphire", group);

				group = new RecipeGroup(() => Language.GetText("LegacyMisc.37").Value + " " + Lang.GetItemNameValue(ItemID.Emerald),
					legendMod.ItemType("GemPeridot"));
				RecipeGroup.RegisterGroup("MagicStorageExtra:AnyEmerald", group);

				group = new RecipeGroup(() => Language.GetText("LegacyMisc.37").Value + " " + Lang.GetItemNameValue(ItemID.Ruby), ItemID.Ruby,
					legendMod.ItemType("GemOpal"));
				RecipeGroup.RegisterGroup("MagicStorageExtra:AnyRuby", group);
			}

			AddCompatibilityRecipeGroups();
		}

		private static void AddCompatibilityRecipeGroups()
		{
			AddCompatibilityRecipeGroup<StorageComponent>();
			AddCompatibilityRecipeGroup<LocatorDisk>();
			AddCompatibilityRecipeGroup<RadiantJewel>();

			AddCompatibilityRecipeGroup<StorageUnit>();
			AddCompatibilityRecipeGroup<StorageUnitTiny>();

			AddCompatibilityRecipeGroup<StorageUnitBlueChlorophyte>();
			AddCompatibilityRecipeGroup<StorageUnitCrimtane>();
			AddCompatibilityRecipeGroup<StorageUnitDemonite>();
			AddCompatibilityRecipeGroup<StorageUnitHallowed>();
			AddCompatibilityRecipeGroup<StorageUnitHellstone>();
			AddCompatibilityRecipeGroup<StorageUnitLuminite>();
			AddCompatibilityRecipeGroup<StorageUnitTerra>();

			AddCompatibilityRecipeGroup<UpgradeBlueChlorophyte>();
			AddCompatibilityRecipeGroup<UpgradeCrimtane>();
			AddCompatibilityRecipeGroup<UpgradeDemonite>();
			AddCompatibilityRecipeGroup<UpgradeHallowed>();
			AddCompatibilityRecipeGroup<UpgradeHellstone>();
			AddCompatibilityRecipeGroup<UpgradeLuminite>();
			AddCompatibilityRecipeGroup<UpgradeTerra>();
		}

		private static void AddCompatibilityRecipeGroup<TModItem>() where TModItem : ModItem
		{
			ModItem item = ModContent.GetInstance<TModItem>();
			RecipeGroup group = new RecipeGroup(() => item.DisplayName.GetTranslation(Language.ActiveCulture), item.item.type);

			if (MagicStorage != null)
				group.ValidItems.Add(MagicStorage.ItemType(item.Name));

			RecipeGroup.RegisterGroup($"MagicStorageExtra:Any{item.Name}", group);
		}
	}
}
