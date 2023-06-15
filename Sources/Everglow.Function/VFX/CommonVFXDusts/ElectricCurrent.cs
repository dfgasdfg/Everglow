using Everglow.Commons.Enums;
using Everglow.Commons.Vertex;

namespace Everglow.Commons.VFX.CommonVFXDusts;

internal class ElectricCurrentPipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.ElectricCurrent;
		effect.Value.Parameters["uNoise"].SetValue(ModAsset.Noise_cell.Value);
		effect.Value.Parameters["uHeatMap"].SetValue(ModAsset.HeatMap_electricCurrent.Value);
	}
	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.Parameters["uLight"].SetValue(0.4f);
		Texture2D FlameColor = ModAsset.Trail.Value;
		Ins.Batch.BindTexture<Vertex2D>(FlameColor);
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicClamp;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.LinearClamp, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}
[Pipeline(typeof(ElectricCurrentPipeline))]
internal class ElectricCurrent : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;
	public List<Vector2> oldPos = new List<Vector2>();
	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public ElectricCurrent() { }

	public override void Update()
	{
		UpdateInside();
		UpdateInside();
	}
	private void UpdateInside()
	{
		if (position.X <= 720 || position.X >= Main.maxTilesX * 16 - 720)
		{
			timer = maxTime;
		}
		if (position.Y <= 720 || position.Y >= Main.maxTilesY * 16 - 720)
		{
			timer = maxTime;
		}
		oldPos.Add(position);

		for(int x = 0;x < oldPos.Count;x++)
		{
			oldPos[x] += new Vector2(0, Main.rand.NextFloat(2f)).RotatedByRandom(6.283);
		}
		timer++;
		if (timer > maxTime)
			Active = false;
		if (Collision.SolidCollision(position, 0, 0))
		{
			velocity *= 0;
			scale *= 0.9f;
		}
		if (Main.tile[(int)(position.X / 16f), (int)(position.Y / 16f)].LiquidAmount > 0)
		{
			position += velocity.RotatedBy(Main.rand.NextFloat(-1f, 1f) / (scale * scale) * 108f) * Main.rand.NextFloat(0.75f, 1.25f);
			timer -= 0.4f;
			Vector2 newPosX = position + new Vector2(velocity.X, 0);
			if (Main.tile[(int)(newPosX.X / 16f), (int)(newPosX.Y / 16f)].LiquidAmount <= 0)
			{
				velocity.X *= -1;
				position += velocity * 2;
			}

			Vector2 newPosY = position + new Vector2(0, velocity.Y);
			if (Main.tile[(int)(newPosY.X / 16f), (int)(newPosY.Y / 16f)].LiquidAmount <= 0 || newPosY.Y % 16 > Main.tile[(int)(newPosY.X / 16f), (int)(newPosY.Y / 16f)].LiquidAmount / 16f)
			{
				velocity.Y *= -1;
				position += velocity * 2;
			}
		}
		else
		{
			position += velocity.RotatedBy(Main.rand.NextFloat(-1f, 1f) / scale * 12f) * Main.rand.NextFloat(0.75f, 1.25f);
		}
		float pocession = 1 - timer / maxTime;
		float c = pocession * scale * 0.04f;
		Lighting.AddLight(position, c * 0.7f, c * 0.7f, c * 0.9f);
		velocity = velocity.RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f) / scale * 48f * ai[2]);
	}
	public override void Draw()
	{
		Vector2[] pos = oldPos.Reverse<Vector2>().ToArray();
		float pocession = timer / maxTime;
		int len = pos.Length;
		if (len <= 2)
			return;
		var bars = new Vertex2D[len * 2 - 1];
		for (int i = 1; i < len; i++)
		{
			Vector2 normal = oldPos[i] - oldPos[i - 1];

			Vector2 normal2 = oldPos[i] - oldPos[i - 1];
			if(i < len - 1)
			{
				normal2 = oldPos[i + 1] - oldPos[i];
			}
			normal = (normal + normal2);
			normal = Vector2.Normalize(normal).RotatedBy(Math.PI * 0.5);


			float k = i / (float)len;
			bars[2 * i - 1] = new Vertex2D(oldPos[i] + normal * scale, new Color(pocession + 1 - MathF.Sin(k * MathF.PI), 0, 0, 0), new Vector3(0 + ai[0], (i + 15 - len) / 10f + timer / 1500f * velocity.Length(), 0.3f));
			bars[2 * i] = new Vertex2D(oldPos[i] - normal * scale, new Color(pocession + 1 - MathF.Sin(k * MathF.PI), 0, 0, 0), new Vector3(3.4f + ai[0], (i + 15 - len) / 10f + timer / 1500f * velocity.Length(), 0.7f));
		}
		bars[0] = new Vertex2D((bars[1].position + bars[2].position) * 0.5f, Color.White, new Vector3(0.5f, 0, 0));
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}