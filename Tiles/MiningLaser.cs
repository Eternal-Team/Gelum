using BaseLibrary;
using BaseLibrary.Tiles;
using BaseLibrary.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Gelum.Tiles
{
	public class MiningLaser : BaseTile
	{
		public override void SetDefaults()
		{
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileObjectData.newTile.Width = 5;
			TileObjectData.newTile.Height = 5;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, 5, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 0;
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<TileEntities.MiningLaser>().Hook_AfterPlacement, -1, 0, false);
			TileObjectData.addTile(Type);
			disableSmartCursor = true;

			ModTranslation name = CreateMapEntryName();
			AddMapEntry(Color.CornflowerBlue, name);
		}

		private static readonly Texture2D BaseTexture = ModContent.GetTexture("Gelum/Textures/Tiles/Laser_Base");
		private static readonly Texture2D HeadTexture = ModContent.GetTexture("Gelum/Textures/Tiles/Laser_Head");

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			TileEntities.MiningLaser laser = Utility.GetTileEntity<TileEntities.MiningLaser>(i, j);
			if (laser == null || !Main.tile[i, j].IsTopLeft()) return false;

			Vector2 position = new Point16(i, j).ToScreenCoordinates();

			float angle = 0f;
			if (laser.CurrentTile != Point16.NegativeOne)
			{
				Vector2 minedTile = laser.CurrentTile.ToWorldCoordinates() + (Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange));
				Vector2 laserOrigin = new Point16(i, j).ToWorldCoordinates(40, 36) + (Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange));

				Vector2 p = minedTile - laserOrigin;
				angle = -(float)Math.Atan2(p.X, p.Y);
				
				Utils.DrawLine(spriteBatch, laserOrigin + Vector2.Normalize(p) * 32f, minedTile, Color.Red, Color.Red, 2f);
			}

			spriteBatch.Draw(HeadTexture, position + new Vector2(40, 36), null, Color.White, angle, new Vector2(40, 10), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(BaseTexture, position, null, Color.White);

			return false;
		}

		public override bool NewRightClick(int i, int j)
		{
			TileEntities.MiningLaser laser = Utility.GetTileEntity<TileEntities.MiningLaser>(i, j);
			if (laser == null) return false;

			PanelUI.Instance.HandleUI(laser);

			return true;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			TileEntities.MiningLaser laser = Utility.GetTileEntity<TileEntities.MiningLaser>(i, j);
			PanelUI.Instance.CloseUI(laser);

			Item.NewItem(i * 16, j * 16, 80, 80, ModContent.ItemType<Items.MiningLaser>());
			laser.Kill(i, j);
		}
	}
}