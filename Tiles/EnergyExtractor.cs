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
	public class EnergyExtractor : BaseTile
	{
		public override string Texture => "Gelum/Textures/Tiles/EnergyExtractor";

		public override void SetDefaults()
		{
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.Origin = new Point16(0, 2);
			TileObjectData.newTile.CoordinatePadding = 0;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<TileEntities.EnergyExtractor>().Hook_AfterPlacement, -1, 0, false);
			TileObjectData.addTile(Type);
			disableSmartCursor = true;

			ModTranslation name = CreateMapEntryName();
			AddMapEntry(Color.CornflowerBlue, name);
		}

		public override bool NewRightClick(int i, int j)
		{
			TileEntities.EnergyExtractor extractor = Utility.GetTileEntity<TileEntities.EnergyExtractor>(i, j);
			if (extractor == null) return false;

			PanelUI.Instance.HandleUI(extractor);

			return true;
		}

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
		{
			Main.specX[nextSpecialDrawIndex] = i;
			Main.specY[nextSpecialDrawIndex] = j;
			nextSpecialDrawIndex++;
		}

		private static readonly Color color = new Color(0, 237, 217);

		public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
		{
			TileEntities.EnergyExtractor extractor = Utility.GetTileEntity<TileEntities.EnergyExtractor>(i, j);
			if (extractor == null) return;

			Tile tile = Main.tile[i, j];
			if (!tile.IsTopLeft()) return;

			Vector2 position = new Point16(i, j).ToScreenCoordinates() + new Vector2(24, 24);

			float progress = (extractor.EnergyHandler.Energy / (float)extractor.EnergyHandler.Capacity).Remap(0f, 1f, 0.05f, 0.15f);

			spriteBatch.Draw(Gelum.OrbTexture, position, null, color, -(float)Hooking.time.TotalGameTime.TotalSeconds, Gelum.OrbTexture.Size() * 0.5f, progress, SpriteEffects.None, 0f);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			TileEntities.EnergyExtractor extractor = Utility.GetTileEntity<TileEntities.EnergyExtractor>(i, j);
			PanelUI.Instance.CloseUI(extractor);

			Item.NewItem(i * 16, j * 16, 48, 48, ModContent.ItemType<Items.EnergyExtractor>());
			extractor.Kill(i, j);
		}
	}
}