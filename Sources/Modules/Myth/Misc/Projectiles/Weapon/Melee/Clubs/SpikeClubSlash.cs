using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class SpikeClubSlash : ModProjectile, IWarpProjectile
{
	public override string Texture => "Everglow/Commons/Weapons/StabbingSwords/StabbingProjectile";
	private Vector2 StartCenter;
	public override void OnSpawn(IEntitySource source)
	{
		StartCenter = Projectile.Center;
	}
	public override void SetDefaults()
	{
		Projectile.width = 30;
		Projectile.height = 30;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.penetrate = -1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 60;
		Projectile.timeLeft = 120;
		Projectile.extraUpdates = 3;
	}
	public override bool PreAI()
	{
		if(Projectile.timeLeft > 120)
		{
			return false;
		}
		return base.PreAI();
	}
	public override bool ShouldUpdatePosition()
	{
		if (Projectile.timeLeft > 120)
		{
			return false;
		}
		return base.ShouldUpdatePosition();
	}
	public override void AI()
	{
		Projectile.friendly = true;
		Projectile.velocity *= 0.85f;

		if (Projectile.timeLeft == 114)
		{
			SoundEngine.PlaySound(new SoundStyle("Everglow/EternalResolve/Sounds/Slash").WithVolumeScale(0.33f), Projectile.Center);
		}
	}
	public override bool PreDraw(ref Color lightColor)
	{
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		float value0 = (120 - Projectile.timeLeft) / 120f;
		float value1 = MathF.Pow(value0, 0.5f);
		float width = (1 - MathF.Cos(value1 * 2f * MathF.PI)) * 10f;
		Vector2 normalizedVelocity = Utils.SafeNormalize(Projectile.velocity, Vector2.zeroVector);
		Vector2 normalize = normalizedVelocity.RotatedBy(Math.PI / 2d) * width;
		Color shadow = Color.White * 0.4f;
		List<Vertex2D> bars = new List<Vertex2D>
			{
				new Vertex2D(StartCenter + normalize - Main.screenPosition, shadow, new Vector3(0,0,0)),
				new Vertex2D(StartCenter - normalize - Main.screenPosition, shadow, new Vector3(0,1,0)),
				new Vertex2D(Projectile.Center + normalize - Main.screenPosition, shadow, new Vector3(1,0,0)),
				new Vertex2D(Projectile.Center - normalize - Main.screenPosition,shadow, new Vector3(1,1,0))
			};
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.StabbingProjectileShade.Value;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Color light = new Color(0.08f * lightColor.R / 255f, 0.08f * width / 10f * lightColor.G / 255f, 0.1f * width / 10f * lightColor.B / 255f, 0) * width * 0.25f;
		light *= width / 10f;
		normalize *= width * width / 450f;
		bars = new List<Vertex2D>
			{
				new Vertex2D(StartCenter + normalize - Main.screenPosition, light, new Vector3(0,0,0)),
				new Vertex2D(StartCenter - normalize - Main.screenPosition, light, new Vector3(0,1,0)),
				new Vertex2D(Projectile.Center + normalize - Main.screenPosition, light, new Vector3(1,0,0)),
				new Vertex2D(Projectile.Center - normalize - Main.screenPosition, light, new Vector3(1,1,0))
			};
		Main.graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.StabbingProjectile.Value;
		//Draw Twice to make the color more vivid.
		if (bars.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		return false;
	}
	public void DrawWarp(VFXBatch sb)
	{
		float time = (float)(Main.time * 0.03);
		float value0 = (120 - Projectile.timeLeft) / 120f;
		float value1 = MathF.Pow(value0, 0.5f);
		float width = (1 - MathF.Cos(value1 * 2f * MathF.PI)) * 5f;
		Vector2 normalizedVelocity = Utils.SafeNormalize(Projectile.velocity, Vector2.zeroVector);
		Vector2 normalize = normalizedVelocity.RotatedBy(Math.PI / 2d) * width;
		Vector2 start = StartCenter - Main.screenPosition;
		Vector2 end = Projectile.Center - Main.screenPosition;
		Vector2 middle = Vector2.Lerp(start, end, 0.5f);
		float rotation = MathF.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
		Color alphaColor = Color.White;
		alphaColor.A = 0;
		alphaColor.R = (byte)((rotation + Math.PI) % 6.283 / 6.283 * 255);
		alphaColor.G = 50;
		List<Vertex2D> bars = new List<Vertex2D>
				{
						new Vertex2D(start - normalize,new Color(alphaColor.R, alphaColor.G / 9, 0, 0),new Vector3(1 + time, 0.3f, 0)),
						new Vertex2D(start + normalize,new Color(alphaColor.R, alphaColor.G / 9, 0, 0),new Vector3(1 + time, 0.7f, 0)),
						new Vertex2D(middle - normalize,new Color(alphaColor.R, alphaColor.G / 3, 0, 0),new Vector3(0.5f + time, 0.3f, 0.5f)),
						new Vertex2D(middle + normalize,new Color(alphaColor.R, alphaColor.G / 3, 0, 0),new Vector3(0.5f + time, 0.7f, 0.5f)),
						new Vertex2D(end,alphaColor,new Vector3(0f + time, 0.5f, 1)),
						new Vertex2D(end,alphaColor,new Vector3(0f + time, 0.5f, 1))
				};
		sb.Draw(Commons.ModAsset.Trail_1.Value, bars, PrimitiveType.TriangleStrip);
	}
}