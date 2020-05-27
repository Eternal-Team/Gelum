using BaseLibrary;
using BaseLibrary.Tiles.TileEntites;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;

namespace Gelum.TileEntities
{
	public abstract class BaseGelumTE : BaseTE
	{
		public abstract Vector2 InsertionPoint { get; }

		public GelumNetwork Network;

		public IEnumerable<BaseGelumTE> GetNeighbors()
		{
			foreach (KeyValuePair<Point16, TileEntity> pair in ByPosition)
			{
				if (pair.Value is BaseGelumTE te && te != this && Vector2.DistanceSquared(pair.Key.ToWorldCoordinates(), Position.ToWorldCoordinates()) <= 160 * 160)
				{
					yield return te;
				}
			}
		}

		public static void GetNeighborsRecursive(BaseGelumTE duct, List<Point16> points)
		{
			foreach (BaseGelumTE neighbor in duct.GetNeighbors())
			{
				if (!points.Contains(neighbor.Position))
				{
					points.Add(neighbor.Position);
					GetNeighborsRecursive(neighbor, points);
				}
			}
		}

		public override void OnPlace()
		{
			List<GelumNetwork> networks = GetNeighbors().Select(duct => duct.Network).Distinct().ToList();
			GelumNetwork network = new GelumNetwork
			{
				Tiles = networks.SelectMany(routedNetwork => routedNetwork.Tiles).Concat(this).ToList()
			};

			foreach (BaseGelumTE duct in network.Tiles)
			{
				GelumNetwork.Networks.Remove(duct.Network);
				duct.Network = network;
			}
		}

		public override void OnKill()
		{
			if (Network.Tiles.Count == 1) GelumNetwork.Networks.Remove(Network);
			else if (GetNeighbors().Count() == 1) Network.Tiles.Remove(this);
			else
			{
				List<Point16> visited = new List<Point16>();
				List<List<Point16>> newNetworks = new List<List<Point16>>();

				foreach (BaseGelumTE duct in GetNeighbors())
				{
					if (visited.Contains(duct.Position)) continue;

					visited.Add(duct.Position);

					List<Point16> p = new List<Point16> { Position, duct.Position };
					GetNeighborsRecursive(duct, p);
					visited.AddRange(p);

					p.Remove(Position);
					newNetworks.Add(p);
				}

				if (newNetworks.Count <= 1)
				{
					Network.Tiles.Remove(this);
				}
				else
				{
					for (int i = 0; i < newNetworks.Count; i++)
					{
						GelumNetwork network = new GelumNetwork
						{
							Tiles = newNetworks[i].Select(position => ByPosition[position] as BaseGelumTE).ToList()
						};
						foreach (BaseGelumTE duct in network.Tiles)
						{
							GelumNetwork.Networks.Remove(duct.Network);
							duct.Network = network;
						}
					}
				}
			}
		}
	}
}