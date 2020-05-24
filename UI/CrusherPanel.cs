using BaseLibrary.UI;
using ContainerLibrary;
using Gelum.TileEntities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;

namespace Gelum.UI
{
	public class CrusherPanel : BaseUIPanel<Crusher>, IItemHandlerUI
	{
		public string GetTexture(Item item) => "Gelum/Textures/Items/Crusher";
		public ItemHandler Handler => Container.Handler;

		public CrusherPanel(Crusher container) : base(container)
		{
			Width.Pixels = 272;
			Height.Pixels = 232;
			BackgroundColor = new Color(38, 49, 90);

			UIText textLabel = new UIText(Language.GetText("Mods.Gelum.MapObject.Crusher"))
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
				Y = { Pixels = 36 }
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
				Y = { Pixels = 108 },
				Width = { Pixels = 54 },
				Height = { Pixels = 54 }
			};
			Add(slot);

			slot = new UIContainerSlot(() => Handler, 2)
			{
				X = { Pixels = 62 },
				Y = { Pixels = 108 },
				Width = { Pixels = 54 },
				Height = { Pixels = 54 }
			};
			Add(slot);

			slot = new UIContainerSlot(() => Handler, 1)
			{
				X = { Pixels = 0 },
				Y = { Pixels = 108 },
				Width = { Pixels = 54 },
				Height = { Pixels = 54 }
			};
			Add(slot);

			slot = new UIContainerSlot(() => Handler, 2)
			{
				X = { Pixels = 62 },
				Y = { Pixels = 108 },
				Width = { Pixels = 54 },
				Height = { Pixels = 54 }
			};
			Add(slot);
		
			slot = new UIContainerSlot(() => Handler, 3)
			{
				X = { Percent = 100, Pixels = -62 },
				Y = { Pixels = 108 },
				Width = { Pixels = 54 },
				Height = { Pixels = 54 }
			};
			Add(slot);
			
			slot = new UIContainerSlot(() => Handler, 4)
			{
				X = { Percent = 100 },
				Y = { Pixels = 108 },
				Width = { Pixels = 54 },
				Height = { Pixels = 54 }
			};
			Add(slot);
		}
	}
}