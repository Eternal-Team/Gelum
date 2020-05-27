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
	public class AutoExtractinator : BaseTile
	{
		public override string Texture => "Gelum/Textures/Tiles/AutoExtractinator";
		
		public override void SetDefaults()
		{
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(0, 2);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 3, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 0;
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<TileEntities.AutoExtractinator>().Hook_AfterPlacement, -1, 0, false);
			TileObjectData.addTile(Type);
			disableSmartCursor = true;
			animationFrameHeight = 48;

			ModTranslation name = CreateMapEntryName();
			AddMapEntry(Color.CornflowerBlue, name);
		}
		
		public override bool NewRightClick(int i, int j)
		{
			TileEntities.AutoExtractinator extractinator = Utility.GetTileEntity<TileEntities.AutoExtractinator>(i, j);
			if (extractinator == null) return false;

			PanelUI.Instance.HandleUI(extractinator);

			return true;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			TileEntities.AutoExtractinator extractinator = Utility.GetTileEntity<TileEntities.AutoExtractinator>(i, j);
			PanelUI.Instance.CloseUI(extractinator);

			Item.NewItem(i * 16, j * 16, 48, 48, ModContent.ItemType<Items.AutoExtractinator>());
			extractinator.Kill(i, j);
		}
	}
}