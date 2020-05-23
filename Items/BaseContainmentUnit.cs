using BaseLibrary.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Gelum.Items
{
	public abstract class BaseContainmentUnit : BaseItem
	{
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 1;
			item.rare = ItemRarityID.Pink;
			item.value = Item.sellPrice(gold: 8);
		}
	}

	public class BasicContainmentUnit : BaseContainmentUnit
	{
		public override void SetStaticDefaults()
		{
		}
	}
}