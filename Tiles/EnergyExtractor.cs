using BaseLibrary;
using BaseLibrary.Tiles;
using BaseLibrary.UI;
using Microsoft.Xna.Framework;
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
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
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

		// todo: spawn blue particles that symbolize energy being pulled in
		
		// public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
		// {
		// 	TileEntities.EnergyExtractor qeChest = Utility.GetTileEntity<TileEntities.EnergyExtractor>(i, j);
		// 	if (qeChest == null) return;
		//
		// 	Main.specX[nextSpecialDrawIndex] = i;
		// 	Main.specY[nextSpecialDrawIndex] = j;
		// 	nextSpecialDrawIndex++;
		// }
		//
		// public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
		// {
		// 	TileEntities.EnergyExtractor qeChest = Utility.GetTileEntity<TileEntities.EnergyExtractor>(i, j);
		// 	if (qeChest == null) return;
		//
		// 	Tile tile = Main.tile[i, j];
		// 	if (!tile.IsTopLeft()) return;
		//
		// 	Vector2 position = new Point16(i, j).ToScreenCoordinates();
		//
		// 	spriteBatch.Draw(QuantumStorage.textureGemsSide, position + new Vector2(5, 9), new Rectangle(6 * (int)qeChest.frequency[0], 0, 6, 10), Color.White, 0f, new Vector2(3, 5), 1f, SpriteEffects.None, 0f);
		// 	spriteBatch.Draw(QuantumStorage.textureGemsMiddle, position + new Vector2(12, 4), new Rectangle(8 * (int)qeChest.frequency[1], 0, 8, 10), Color.White);
		// 	spriteBatch.Draw(QuantumStorage.textureGemsSide, position + new Vector2(24, 4), new Rectangle(6 * (int)qeChest.frequency[2], 0, 6, 10), Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.FlipHorizontally, 0f);
		// }

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			TileEntities.EnergyExtractor extractor = Utility.GetTileEntity<TileEntities.EnergyExtractor>(i, j);
			PanelUI.Instance.CloseUI(extractor);

			Item.NewItem(i * 16, j * 16, 32, 32, ModContent.ItemType<Items.EnergyExtractor>());
			extractor.Kill(i, j);
		}
	}
}