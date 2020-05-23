using BaseLibrary;
using BaseLibrary.Tiles;
using BaseLibrary.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Gelum.Tiles
{
	public class EnergyRelay : BaseTile
	{
		public override string Texture => "Gelum/Textures/Tiles/EnergyRelay";

		public override void SetDefaults()
		{
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinatePadding = 0;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<TileEntities.EnergyRelay>().Hook_AfterPlacement, -1, 0, false);
			TileObjectData.addTile(Type);
			disableSmartCursor = true;

			ModTranslation name = CreateMapEntryName();
			AddMapEntry(Color.CornflowerBlue, name);
		}

		public override bool NewRightClick(int i, int j)
		{
			TileEntities.EnergyRelay relay = Utility.GetTileEntity<TileEntities.EnergyRelay>(i, j);
			if (relay == null) return false;

			PanelUI.Instance.HandleUI(relay);

			return true;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			TileEntities.EnergyRelay relay = Utility.GetTileEntity<TileEntities.EnergyRelay>(i, j);
			PanelUI.Instance.CloseUI(relay);

			Item.NewItem(i * 16, j * 16, 32, 32, ModContent.ItemType<Items.EnergyRelay>());
			relay.Kill(i, j);
		}
	}
}