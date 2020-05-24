using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;

namespace Gelum.TileEntities
{
	public class GelumNetwork
	{
		public static readonly List<GelumNetwork> Networks = new List<GelumNetwork>();

		public Color debugColor;

		public List<BaseGelumTE> Tiles = new List<BaseGelumTE>();

		internal GelumNetwork()
		{
			Networks.Add(this);
			debugColor = new Color(Main.rand.Next(256), Main.rand.Next(256), Main.rand.Next(256));
		}
	}
}