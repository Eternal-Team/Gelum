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
	public class Timer : BaseTile
	{
		public override string Texture => "Gelum/Textures/Tiles/Timer";
		
		public override void SetDefaults()
		{
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = false;
			Main.tileLavaDeath[Type] = false;
			
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 0;
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<TileEntities.Timer>().Hook_AfterPlacement, -1, 0, false);
			TileObjectData.addTile(Type);
			disableSmartCursor = true;

			ModTranslation name = CreateMapEntryName();
			AddMapEntry(Color.Red, name);
		}
		
		public override bool NewRightClick(int i, int j)
		{
			TileEntities.Timer timer = Utility.GetTileEntity<TileEntities.Timer>(i, j);
			if (timer == null) return false;

			PanelUI.Instance.HandleUI(timer);

			return true;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			TileEntities.Timer timer = Utility.GetTileEntity<TileEntities.Timer>(i, j);
			PanelUI.Instance.CloseUI(timer);

			Item.NewItem(i * 16, j * 16, 16, 16, ModContent.ItemType<Items.Timer>());
			timer.Kill(i, j);
		}
	}
}