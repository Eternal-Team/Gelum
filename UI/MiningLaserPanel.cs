using BaseLibrary.UI;
using ContainerLibrary;
using Gelum.TileEntities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;

namespace Gelum.UI
{
	public class MiningLaserPanel : BaseUIPanel<MiningLaser>, IItemHandlerUI
	{
		public string GetTexture(Item item) => "Gelum/Textures/Items/MiningLaser";
		public ItemHandler Handler => Container.Handler;

		public MiningLaserPanel(MiningLaser container) : base(container)
		{
			Width.Pixels = 400;
			Height.Pixels = 232;
			BackgroundColor = new Color(38, 49, 90);

			UIText textLabel = new UIText(Language.GetText("Mods.Gelum.MapObject.MiningLaser"))
			{
				X = { Percent = 50 },
				HorizontalAlignment = HorizontalAlignment.Center
			};
			Add(textLabel);

			UIEnergyStore energy = new UIEnergyStore(container)
			{
				Width = { Pixels = 64 },
				Height = { Pixels = 64 },
				X = { Percent = 50 },
				Y = { Pixels = 28 }
			};
			Add(energy);

			UIContainerSlot slot = new UIContainerSlot(() => Handler)
			{
				X = { Percent = 50 },
				Y = { Percent = 100 },
				Width = { Pixels = 54 },
				Height = { Pixels = 54 }
			};
			Add(slot);

			#region Radius
			UIText textRadius = new UIText("Radius: " + container.radius)
			{
				X = { Percent = 50 },
				Y = { Pixels = 105 },
				HorizontalAlignment = HorizontalAlignment.Center
			};
			Add(textRadius);

			UITextButton buttonDecRadius = new UITextButton("--")
			{
				Y = { Pixels = 100 },
				Width = { Pixels = 40 },
				Height = { Pixels = 20 },
				Padding = Padding.Zero
			};
			buttonDecRadius.OnClick += _ =>
			{
				if (container.radius > 10)
				{
					container.radius -= 10;
					container.CurrentTile = Point16.NegativeOne;
				}

				textRadius.Text = "Radius: " + container.radius;
			};
			Add(buttonDecRadius);

			buttonDecRadius = new UITextButton("-")
			{
				X = { Pixels = 48 },
				Y = { Pixels = 100 },
				Width = { Pixels = 40 },
				Height = { Pixels = 20 },
				Padding = Padding.Zero
			};
			buttonDecRadius.OnClick += _ =>
			{
				if (container.radius > 1)
				{
					container.radius--;
					container.CurrentTile = Point16.NegativeOne;
				}

				textRadius.Text = "Radius: " + container.radius;
			};
			Add(buttonDecRadius);

			UITextButton buttonIncRadius = new UITextButton("+")
			{
				X = { Percent = 100, Pixels = -48 },
				Y = { Pixels = 100 },
				Width = { Pixels = 40 },
				Height = { Pixels = 20 },
				Padding = Padding.Zero
			};
			buttonIncRadius.OnClick += _ =>
			{
				container.radius++;
				container.CurrentTile = Point16.NegativeOne;
				textRadius.Text = "Radius: " + container.radius;
			};
			Add(buttonIncRadius);

			buttonIncRadius = new UITextButton("++")
			{
				X = { Percent = 100 },
				Y = { Pixels = 100 },
				Width = { Pixels = 40 },
				Height = { Pixels = 20 },
				Padding = Padding.Zero
			};
			buttonIncRadius.OnClick += _ =>
			{
				container.radius += 10;
				container.CurrentTile = Point16.NegativeOne;
				textRadius.Text = "Radius: " + container.radius;
			};
			Add(buttonIncRadius);
			#endregion

			#region Depth
			UIText textDepth = new UIText("Depth: " + container.height)
			{
				X = { Percent = 50 },
				Y = { Pixels = 128 },
				HorizontalAlignment = HorizontalAlignment.Center
			};
			Add(textDepth);

			UITextButton buttonDecDepth = new UITextButton("---")
			{
				Y = { Pixels = 128 },
				Width = { Pixels = 40 },
				Height = { Pixels = 20 },
				Padding = Padding.Zero
			};
			buttonDecDepth.OnClick += _ =>
			{
				if (container.height > 100)
				{
					container.height -= 100;
					container.CurrentTile = Point16.NegativeOne;
				}

				textDepth.Text = "Depth: " + container.height;
			};
			Add(buttonDecDepth);

			buttonDecDepth = new UITextButton("--")
			{
				X = { Pixels = 48 },
				Y = { Pixels = 128 },
				Width = { Pixels = 40 },
				Height = { Pixels = 20 },
				Padding = Padding.Zero
			};
			buttonDecDepth.OnClick += _ =>
			{
				if (container.height > 10)
				{
					container.height -= 10;
					container.CurrentTile = Point16.NegativeOne;
				}

				textDepth.Text = "Depth: " + container.height;
			};
			Add(buttonDecDepth);

			buttonDecDepth = new UITextButton("-")
			{
				X = { Pixels = 96 },
				Y = { Pixels = 128 },
				Width = { Pixels = 40 },
				Height = { Pixels = 20 },
				Padding = Padding.Zero
			};
			buttonDecDepth.OnClick += _ =>
			{
				if (container.height > 1)
				{
					container.height--;
					container.CurrentTile = Point16.NegativeOne;
				}

				textDepth.Text = "Depth: " + container.height;
			};
			Add(buttonDecDepth);

			UITextButton buttonIncDepth = new UITextButton("+")
			{
				X = { Percent = 100, Pixels = -96 },
				Y = { Pixels = 128 },
				Width = { Pixels = 40 },
				Height = { Pixels = 20 },
				Padding = Padding.Zero
			};
			buttonIncDepth.OnClick += _ =>
			{
				container.height++;
				container.CurrentTile = Point16.NegativeOne;
				textDepth.Text = "Depth: " + container.height;
			};
			Add(buttonIncDepth);

			buttonIncDepth = new UITextButton("++")
			{
				X = { Percent = 100, Pixels = -48 },
				Y = { Pixels = 128 },
				Width = { Pixels = 40 },
				Height = { Pixels = 20 },
				Padding = Padding.Zero
			};
			buttonIncDepth.OnClick += _ =>
			{
				container.height += 10;
				container.CurrentTile = Point16.NegativeOne;
				textDepth.Text = "Depth: " + container.height;
			};
			Add(buttonIncDepth);

			buttonIncDepth = new UITextButton("+++")
			{
				X = { Percent = 100 },
				Y = { Pixels = 128 },
				Width = { Pixels = 40 },
				Height = { Pixels = 20 },
				Padding = Padding.Zero
			};
			buttonIncDepth.OnClick += _ =>
			{
				container.height += 100;
				container.CurrentTile = Point16.NegativeOne;
				textDepth.Text = "Depth: " + container.height;
			};
			Add(buttonIncDepth);
			#endregion
		}
	}
}