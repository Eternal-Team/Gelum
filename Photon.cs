using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace Gelum
{
	public struct Photon
	{
		public Vector2 position;
		public Vector2 velocity;
		public Color color;
		public int timeLeft;
		public bool active;
		public float scale;
		public Action OnDeath;

		public void Update()
		{
			position += velocity;

			timeLeft--;
			if (timeLeft == 0)
			{
				active = false;
				OnDeath?.Invoke();
			}
		}

		private static int index;

		public static void Spawn(Vector2 position, Vector2 velocity, Color color, int timeLeft = -1, Action onDeath = null)
		{
			ref Photon dust = ref Gelum.Photons[index];
			dust.scale = Main.rand.NextFloat(0.12f, 0.2f);
			dust.position = position;
			dust.velocity = velocity;
			dust.color = color;
			dust.timeLeft = timeLeft;
			dust.active = true;
			dust.OnDeath = onDeath;

			index++;
			if (index >= Gelum.Photons.Length) index = 0;
		}
	}
}