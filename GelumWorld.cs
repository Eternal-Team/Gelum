using BaseLibrary;
using Gelum.TileEntities;
using Microsoft.Xna.Framework;
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

			foreach (Node node in BaseGelumTE.Nodes)
			{
				foreach (Node connection in node.connections)
				{
					Main.spriteBatch.DrawLine(connection.position - Main.screenPosition, node.position - Main.screenPosition, Color.Goldenrod);
				}
			}

			Main.spriteBatch.End();
		}
	}
}