using Gelum.TileEntities;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Gelum
{
	public class GelumWorld : ModWorld
	{
		public override void PostDrawTiles()
		{
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			foreach (GelumNetwork network in GelumNetwork.Networks)
			{
				foreach (BaseGelumTE tile in network.Tiles)
				{
					// Main.spriteBatch.Draw(Main.magicPixel, new Rectangle((int)(tile.Position.X * 16 - Main.screenPosition.X), (int)(tile.Position.Y * 16 - Main.screenPosition.Y), 16, 16), network.debugColor * 0.5f);
				}
			}

			Main.spriteBatch.End();
		}
	}
}