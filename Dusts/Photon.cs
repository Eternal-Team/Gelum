// using Microsoft.Xna.Framework;
// using Terraria;
// using Terraria.ModLoader;
//
// namespace Gelum.Dusts
// {
// 	public class Photon : ModDust
// 	{
// 		public override bool Autoload(ref string name, ref string texture)
// 		{
// 			texture = "Gelum/Textures/Dusts/Photon";
// 			return base.Autoload(ref name, ref texture);
// 		}
//
// 		public override void OnSpawn(Dust dust)
// 		{
// 			dust.noGravity = true;
// 			dust.noLight = false;
// 			dust.color =  new Color(0, 237, 217);
// 			dust.frame=new Rectangle(0,0,32,32);
// 		}
//
// 		public override bool Update(Dust dust)
// 		{
// 			dust.position += dust.velocity;
// 			dust.rotation += 0.1f * (dust.dustIndex % 2 == 0 ? -1 : 1);
//
// 			Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), dust.color.R / 255f * 0.5f, dust.color.G / 255f * 0.5f, dust.color.B / 255f * 0.5f);
//
// 			return false;
// 		}
// 	}
// }