namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

public class DarkGlimmeringParticlePipeline : Pipeline
{
	public override void Load()
	{
		// Load the effect resource from mod asset and save in field
		effect = ModAsset.DarkGlimmeringParticle;

		// Set the parameter of the effect instance (same to what we did in LearningOpenGL)
	}

	public override void BeginRender()
	{
		// Create variables to save informations
		// -------------------------------------
		// Effect instance
		var effect = this.effect.Value;

		// Projection matrix (frustum)
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);

		// Model matrix
		// 1. translate according to the screen position
		// 2. transform with the game view matrix
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) *
			Main.GameViewMatrix.TransformationMatrix;

		// Get the transform matrix
		// Set the parameter of the effect instance as transform matrix
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.Parameters["uHeatMap"].SetValue(ModAsset.HeatMap_DarkGlimmering.Value);

		// Bind texture
		Texture2D halo = Commons.ModAsset.Point.Value;
		Ins.Batch.BindTexture<Vertex2D>(halo);

		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

		// Begin the first rendering
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.PointWrap, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}

[Pipeline(typeof(DarkGlimmeringParticlePipeline), typeof(BloomPipeline))]
public class DarkGlimmeringParticleDust : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float rotation;

	public DarkGlimmeringParticleDust()
	{
	}

	public override void Update()
	{
		position += velocity;
		velocity *= 0.9f;
		// If the particle is outside a specific range, then set the timer to max (duration to 0)
		if (position.X <= 320 || position.X >= Main.maxTilesX * 16 - 320)
		{
			timer = maxTime;
		}
		if (position.Y <= 320 || position.Y >= Main.maxTilesY * 16 - 320)
		{
			timer = maxTime;
		}

		// Slow down
		velocity *= 0.995f;

		// Scale
		scale = MathF.Sin(timer / maxTime * MathHelper.Pi) * ai[0];

		// Loop Instructions
		timer++;
		if (timer > maxTime)
		{
			Active = false;
		}
	}

	public override void Draw()
	{
		// Duration progress
		float progress = timer / maxTime;

		// Offset
		Vector2 toCorner = new Vector2(0, scale).RotatedBy(rotation);
		Color lightColor = new Color(1f, 1f, 1f, 0);
		float aValue = MathF.Sin((float)Main.time * 0.15f + ai[1]) * 0.5f + 0.4f;
		float addScale = 0f;
		if (aValue < 0)
		{
			addScale = -aValue * 15;
		}
		aValue = Math.Clamp(aValue, 0, 1);
		toCorner *= 1 + addScale;
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position + toCorner, new Color(0, 0f, progress, aValue), lightColor.ToVector3()),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5), new Color(0, 1f, progress, aValue), lightColor.ToVector3()),

			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1.5), new Color(1, 0f, progress, aValue), lightColor.ToVector3()),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1), new Color(1, 1f, progress, aValue), lightColor.ToVector3()),
		};

		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}