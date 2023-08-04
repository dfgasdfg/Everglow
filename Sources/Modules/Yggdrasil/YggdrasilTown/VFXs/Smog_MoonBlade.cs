namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;
public class Smog_MoonBladePipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.Smog_MoonBlade;
		effect.Value.Parameters["uHeatMap"].SetValue(Commons.ModAsset.Noise_perlin.Value);
		effect.Value.Parameters["uPowder"].SetValue(Commons.ModAsset.Noise_Sand.Value);
	}
	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		Texture2D halo = ModAsset.HeatMap_moonBlade_smog.Value;
		Ins.Batch.BindTexture<Vertex2D>(halo);
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicClamp;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.LinearWrap, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}
	public override void EndRender()
	{
		Ins.Batch.End();
	}
}
[Pipeline(typeof(Smog_MoonBladePipeline))]
public class Smog_MoonBladeDust : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;
	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	/// <summary>
	/// ai[0]x相位
	/// ai[1]角速度
	/// ai[2]宽度
	/// </summary>
	public List<Vector2> oldPos = new List<Vector2>();
	public float timer;
	public float maxTime;
	public float scale;
	public Smog_MoonBladeDust() { }
	public override void Update()
	{
		position += velocity;
		velocity.Y += 0.045f;
		oldPos.Add(position);
		if (oldPos.Count > 15)
			oldPos.RemoveAt(0);
		timer++;
		if (timer > maxTime)
			Active = false;
		velocity = velocity.RotatedBy(ai[1]);
		float pocession = 1 - timer / maxTime;
		float c = pocession * scale * 0.1f;
		Lighting.AddLight(position, c * 0.14f, c * 0.47f, c * 0.97f);
	}

	public override void Draw()
	{
		Vector2[] pos = oldPos.Reverse<Vector2>().ToArray();
		float timeValue = timer / maxTime;
		int len = pos.Length;
		if (len <= 2)
			return;
		var bars = new Vertex2D[len * 2 - 1];
		for (int i = 1; i < len; i++)
		{
			Vector2 normal = oldPos[i] - oldPos[i - 1];
			normal = Vector2.Normalize(normal).RotatedBy(Math.PI * 0.5);
			var fade = new Color(timeValue * timeValue * timeValue * 2 - 0.1f, 0, 0);
			float width = scale * (float)Math.Sin(i / (double)len * Math.PI);
			if (timer < 10)
			{
				width *= timer / 10f;
			}
			bars[2 * i - 1] = new Vertex2D(oldPos[i] + normal * width, fade, new Vector3(0 + ai[0], (i + 15 - len) / 80f, 0.8f - timeValue));
			bars[2 * i] = new Vertex2D(oldPos[i] - normal * width, fade, new Vector3(0.07f + ai[0], (i + 15 - len) / 80f, 0.8f - timeValue));
		}
		bars[0] = new Vertex2D((bars[1].position + bars[2].position) * 0.5f, Color.White, new Vector3(0.5f, 0, 0));
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}

[Pipeline(typeof(Smog_MoonBladePipeline))]
public class Smog_MoonBladeWave : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;
	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	/// <summary>
	/// ai[0]x相位
	/// ai[1]角速度
	/// ai[2]宽度
	/// </summary>
	public float timer;
	public float maxTime;
	public float radius;
	public Smog_MoonBladeWave() { }
	public override void Update()
	{
		position += velocity;
		radius += ai[1] * ((maxTime - timer) / maxTime);
		timer++;
		if (timer > maxTime)
			Active = false;


		float delC = ai[2] * 0.05f * (float)Math.Sin((maxTime - timer) / maxTime * Math.PI);
		Lighting.AddLight((int)(position.X / 16), (int)(position.Y / 16), 0.015f * delC, 0, 0.45f * delC);
	}

	public override void Draw()
	{
		float timeValue = timer / maxTime;
		int len = (int)(radius / 3f);
		if (len <= 2)
			return;
		var bars = new Vertex2D[len * 2 + 2];
		for (int i = 0; i < len + 1; i++)
		{
			Vector2 normal = new Vector2(0, 1).RotatedBy(i / (double)len * Math.PI * 2);
			Vector2 radiousDraw = normal * radius;

			var fade = new Color(timeValue * timeValue * timeValue * 2 - 0.1f, 0, 0);
			float width = ai[2];
			float texCoordWidth = 0.37f;
			if (width > radiousDraw.Length())
			{
				texCoordWidth *= radiousDraw.Length() / width;
				width = radiousDraw.Length();
			}

			bars[2 * i] = new Vertex2D(position + radiousDraw, fade, new Vector3(ai[0] + timer / maxTime, i / (float)len * 2, 0.8f - timeValue));
			bars[2 * i + 1] = new Vertex2D(position + radiousDraw - normal * width, fade, new Vector3(texCoordWidth + ai[0] + timer / maxTime, i / (float)len * 2, 0.8f - timeValue));
		}

		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}
