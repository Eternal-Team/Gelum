using BaseLibrary.UI;
using ContainerLibrary;
using Gelum.TileEntities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;

namespace Gelum.UI
{
	public class AutoExtractinatorPanel : BaseUIPanel<AutoExtractinator>, IItemHandlerUI
	{
		public string GetTexture(Item item) => "Gelum/Textures/Items/AutoExtractinator";
		public ItemHandler Handler => Container.Handler;

		public AutoExtractinatorPanel(AutoExtractinator container) : base(container)
		{
			Width.Pixels = 272;
			Height.Pixels = 348;
			BackgroundColor = new Color(38, 49, 90);

			UIText textLabel = new UIText(Language.GetText("Mods.Gelum.MapObject.AutoExtractinator"))
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

			slot = new UIContainerSlot(() => Handler, 1)
			{
				X = { Pixels = 0 },
				Y = { Percent = 50 },
				Width = { Pixels = 54 },
				Height = { Pixels = 54 }
			};
			Add(slot);

			UIGrid<UIContainerSlot> grid = new UIGrid<UIContainerSlot>(3)
			{
				Width = { Pixels = 54 * 3 + 8 },
				Height = { Pixels = 54 * 3 + 8 },
				X = { Percent = 100 },
				Y = { Pixels = 100 }
			};
			Add(grid);

			for (int i = 2; i < 11; i++)
			{
				slot = new UIContainerSlot(() => Handler, i)
				{
					Width = { Pixels = 54 },
					Height = { Pixels = 54 }
				};
				grid.Add(slot);
			}
		}
	}
}