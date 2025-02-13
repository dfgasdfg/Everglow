using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class SquamousRockProj : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 24;
		Projectile.height = 24;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.aiStyle = -1;
		Projectile.penetrate = 6;
		Projectile.timeLeft = 3600;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
		ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Projectile.type] = true;
	}

	internal int TimeTokill = -1;

	public override void AI()
	{
		Projectile.rotation = MathF.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
		if (TimeTokill >= 0 && TimeTokill <= 2)
		{
			Projectile.Kill();
		}

		if (TimeTokill <= 15 && TimeTokill > 0)
		{
			Projectile.velocity = Projectile.oldVelocity;
		}

		TimeTokill--;
		if (TimeTokill < 0)
		{
			Projectile.velocity.Y += 0.47f;
		}
		else
		{
			if (TimeTokill < 10)
			{
				Projectile.damage = 0;
				Projectile.friendly = false;
			}
			Projectile.velocity *= 0f;
		}
	}

	public void GenerateSmog(int Frequency)
	{
		for (int g = 0; g < Frequency / 2 + 1; g++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 4f)).RotatedByRandom(MathHelper.TwoPi);
			var somg = new RockSmogDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + newVelocity * 6,
				maxTime = Main.rand.Next(57, 125),
				scale = Main.rand.NextFloat(80f, 115f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 },
			};
			Ins.VFXManager.Add(somg);
		}
		for (int g = 0; g < Frequency * 4; g++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(12f, 20f)).RotatedByRandom(MathHelper.TwoPi);
			var somg = new RockSmog_ConeDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(15, 33),
				scale = 0,
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 },
			};
			Ins.VFXManager.Add(somg);
		}
	}

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		AmmoHit();
		return false;
	}

	public void AmmoHit()
	{
		TimeTokill = 60;
		Projectile.velocity = Projectile.oldVelocity;
		for (int x = 0; x < 16; x++)
		{
			Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity - new Vector2(4), Projectile.width, Projectile.height, ModContent.DustType<SquamousShellStone>(), 0f, 0f, 0, default, Main.rand.NextFloat(0.9f, 2.1f));
			dust.velocity = new Vector2(0, Main.rand.NextFloat(0f, 6f)).RotatedByRandom(MathHelper.TwoPi);
		}
		SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
		GenerateSmog(8);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		if (TimeTokill > 0)
		{
			return false;
		}
		else
		{
			var TexMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.spriteBatch.Draw(TexMain, Projectile.Center - Main.screenPosition - Projectile.velocity, null, lightColor, Projectile.rotation, TexMain.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}

	public override void PostDraw(Color lightColor)
	{
		DrawTrail_dark(lightColor);
		DrawTrail(lightColor);
	}

	public void DrawTrail(Color light)
	{
		float drawC = 0.4f;

		var bars = new List<Vertex2D>();
		int trueL = 0;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
			{
				if (i == 1)
				{
					return;
				}

				break;
			}

			trueL = i;
		}
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
			{
				break;
			}

			float width = 27;
			if (Projectile.timeLeft <= 30)
			{
				width *= Projectile.timeLeft / 30f;
			}

			var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
			normalDir = new Vector2(-normalDir.Y, normalDir.X).SafeNormalize(Vector2.Zero);

			var factor = i / (float)trueL;
			var color = Color.Lerp(new Color(drawC * light.R / 255f * 0.3f, drawC * light.G / 255f * 0.2f, drawC * light.B / 255f * 0.1f, 0), new Color(0, 0, 0, 0), factor);
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10) - Main.screenPosition, color, new Vector3(1, 0, 0)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10) - Main.screenPosition, color, new Vector3(1, 1, 0)));
		}
		if (bars.Count > 2)
		{
			Texture2D t = Commons.ModAsset.Trail_4.Value;
			Main.graphics.GraphicsDevice.Textures[0] = t;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
	}

	public void DrawTrail_dark(Color light)
	{
		float drawC = 0.4f;
		var bars = new List<Vertex2D>();
		int trueL = 0;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
			{
				if (i == 1)
				{
					return;
				}

				break;
			}
			trueL = i;
		}
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
			{
				break;
			}

			float width = 27;
			if (Projectile.timeLeft <= 30)
			{
				width *= Projectile.timeLeft / 30f;
			}

			var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
			normalDir = new Vector2(-normalDir.Y, normalDir.X).SafeNormalize(Vector2.Zero);

			var factor = i / (float)trueL;
			var color = Color.Lerp(new Color(drawC * light.R / 255f, drawC * light.G / 255f, drawC * light.B / 255f, drawC), new Color(0, 0, 0, 0), factor);
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10) - Main.screenPosition, color, new Vector3(1, 0, 0)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10) - Main.screenPosition, color, new Vector3(1, 1, 0)));
		}
		if (bars.Count > 2)
		{
			Texture2D t = Commons.ModAsset.Trail_4_black.Value;
			Main.graphics.GraphicsDevice.Textures[0] = t;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
	}
}