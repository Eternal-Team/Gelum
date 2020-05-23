using BaseLibrary;
using BaseLibrary.Tiles.TileEntites;
using BaseLibrary.UI;
using ContainerLibrary;
using EnergyLibrary;
using Gelum.Items;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace Gelum.TileEntities
{
	public interface IEnergyReceiver : IEnergyHandler
	{
	}

	public interface IEnergyTransmitter : IEnergyHandler
	{
	}

	public class Node
	{
		public List<Node> connections = new List<Node>();
		public Vector2 position;

		public Node(Vector2 position)
		{
			this.position = position;
		}
	}

	public abstract class BaseGelumTE : BaseTE
	{
		public static List<Node> Nodes = new List<Node>();

		public static void ConstructGraph()
		{
			foreach (Node node in Nodes)
			{
				node.connections = new List<Node>();

				foreach (Node other in Nodes.Where(other => node != other).Where(other => Vector2.DistanceSquared(node.position, other.position) < 160 * 160))
				{
					node.connections.Add(other);
				}
			}
		}

		private Node node;
		public override void OnPlace()
		{
			node = new Node(Position.ToWorldCoordinates(16, 16));
			Nodes.Add(node);

			ConstructGraph();
		}

		public override void OnKill()
		{
			 Nodes.Remove(node);
			 
			 ConstructGraph();
		}
	}

	public class EnergyRelay : BaseGelumTE, IItemHandler, IHasUI, IEnergyHandler
	{
		public override Type TileType => typeof(Tiles.EnergyRelay);

		public ItemHandler Handler { get; }
		public EnergyHandler EnergyHandler { get; }

		public Guid UUID { get; set; }
		public BaseUIPanel UI { get; set; }
		public LegacySoundStyle CloseSound { get; }
		public LegacySoundStyle OpenSound { get; }

		private Timer timer;

		public EnergyRelay()
		{
			timer = new Timer(60, Callback);

			Handler = new ItemHandler();
			Handler.IsItemValid += (slot, item) => item.modItem is BaseContainmentUnit;

			EnergyHandler = new EnergyHandler(1000, 1000);
		}


		private void Callback()
		{
			if (EnergyHandler.Energy <= 0) return;

			foreach (KeyValuePair<Point16, TileEntity> pair in ByPosition)
			{
				if (pair.Value != this && pair.Value is IEnergyReceiver other && other.EnergyHandler.Energy < other.EnergyHandler.Capacity)
				{
					// calculate dust velocity and lifespan


					long energy = -EnergyHandler.ExtractEnergy(100);

					return;
				}
			}
		}

		public override void Update()
		{
			timer.Update();
		}

		public override TagCompound Save() => new TagCompound
		{
			["UUID"] = UUID,
			["Items"] = Handler.Save(),
			["Energy"] = EnergyHandler.Save()
		};

		public override void Load(TagCompound tag)
		{
			UUID = tag.Get<Guid>("UUID");
			Handler.Load(tag.GetCompound("Items"));
			EnergyHandler.Load(tag.GetCompound("Energy"));
		}
	}
}