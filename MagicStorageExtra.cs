using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MagicStorageExtra.Edits;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace MagicStorageExtra
{
	public partial class MagicStorageExtra : Mod
	{
		public static MagicStorageExtra Instance;
		public static Mod bluemagicMod;
		public static Mod legendMod;
		public static Mod MagicStorage;
		public static Mod ItemChecklist;

		public static string GithubUserName => "ExterminatorX99";
		public static string GithubProjectName => "MagicStorageExtra";

		public static ModHotKey IsItemKnownHotKey { get; private set; }

		public Mod[] AllMods { get; private set; }

		public override void Load()
		{
			//if (ModLoader.GetMod("MagicStorage") != null)
			//    throw new Exception("\"Magic Storage - Extra\" and \"Magic Storage\" are not compatible");
			Instance = this;
			InterfaceHelper.Initialize();
			legendMod = ModLoader.GetMod("LegendOfTerraria3");
			bluemagicMod = ModLoader.GetMod("Bluemagic");
			MagicStorage = ModLoader.GetMod("MagicStorage");
			ItemChecklist = ModLoader.GetMod("ItemChecklist");
			AddTranslations();
			AddGlobalItem("MagicStorageExtraItemSaveLoadHook", new ItemSaveLoadHook());
			IsItemKnownHotKey = RegisterHotKey("Is This Item Known?", "");
			RecursiveCraftIntegration.Load();
			EditsLoader.Load();
		}

		public override void PostAddRecipes()
		{
			RecursiveCraftIntegration.InitRecipes();
		}

		public override void Unload()
		{
			Instance = null;
			bluemagicMod = null;
			legendMod = null;
			MagicStorage = null;
			IsItemKnownHotKey = null;
			StorageGUI.Unload();
			CraftingGUI.Unload();
			RecursiveCraftIntegration.Unload();
			EditsLoader.Unload();
		}

		public override void PostSetupContent()
		{
			Type type = Assembly.GetAssembly(typeof(Mod)).GetType("Terraria.ModLoader.Mod");
			FieldInfo loadModsField = type.GetField("items", BindingFlags.Instance | BindingFlags.NonPublic);

			AllMods = ModLoader.Mods.Where(x => !x.Name.EndsWith("Library", StringComparison.OrdinalIgnoreCase))
				.Where(x => x.Name != "ModLoader")
				.Where(x => ((IDictionary<string, ModItem>)loadModsField.GetValue(x)).Count > 0)
				.ToArray();
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			NetHelper.HandlePacket(reader, whoAmI);
		}

		public override bool HijackGetData(ref byte messageType, ref BinaryReader reader, int playerNumber)
		{
			EditsLoader.MessageTileEntitySyncing = messageType == MessageID.TileSection;

			return false;
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			InterfaceHelper.ModifyInterfaceLayers(layers);
		}

		public override void PostUpdateInput()
		{
			if (!Main.instance.IsActive)
				return;
			StorageGUI.Update(null);
			CraftingGUI.Update(null);
		}
	}
}
