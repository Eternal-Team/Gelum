using BaseLibrary;
using BaseLibrary.Tiles;
using BaseLibrary.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Gelum.Tiles
{
	public class Crusher : BaseTile
	{
		public override string Texture => "Gelum/Textures/Tiles/Crusher";
		
		public override void SetDefaults()
		{
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.Height = 5;
			TileObjectData.newTile.Origin = new Point16(0, 4);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 4, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 0;
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<TileEntities.Crusher>().Hook_AfterPlacement, -1, 0, false);
			TileObjectData.addTile(Type);
			disableSmartCursor = true;

			ModTranslation name = CreateMapEntryName();
			AddMapEntry(Color.CornflowerBlue, name);
		}
		
		public override bool NewRightClick(int i, int j)
		{
			TileEntities.Crusher crusher = Utility.GetTileEntity<TileEntities.Crusher>(i, j);
			if (crusher == null) return false;

			PanelUI.Instance.HandleUI(crusher);

			return true;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			TileEntities.Crusher crusher = Utility.GetTileEntity<TileEntities.Crusher>(i, j);
			PanelUI.Instance.CloseUI(crusher);

			Item.NewItem(i * 16, j * 16, 32, 32, ModContent.ItemType<Items.Crusher>());
			crusher.Kill(i, j);
		}
	}
}