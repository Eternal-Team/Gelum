﻿using BaseLibrary.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Gelum.Items
{
	public class AutoExtractinator : BaseItem
	{
		public override string Texture => "Gelum/Textures/Items/AutoExtractinator";

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.rare = ItemRarityID.Pink;
			item.value = Item.sellPrice(gold: 8);
			item.createTile = ModContent.TileType<Tiles.AutoExtractinator>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Extractinator);
			recipe.AddIngredient(ItemID.GoldBar, 12);
			recipe.AddIngredient(ItemID.Chest);
			recipe.SetResult(this);
		}
	}
}