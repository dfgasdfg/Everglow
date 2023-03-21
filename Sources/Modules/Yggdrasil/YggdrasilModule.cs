using Everglow.Commons.Modules;
using Everglow.Commons.VFX;
using Everglow.Yggdrasil.Common;
using Terraria.Graphics.Effects;

namespace Everglow.Yggdrasil;

internal class YggdrasilModule : EverglowModule
{
	public override string Name => "Yggdrasil";

	private RenderTarget2D screen = null, OcclusionRender = null, EffectTarget = null, TotalEffeftsRender = null;

	private Effect ScreenOcclusion;

	public override void Load()
	{
		if (!Main.dedServ)
		{
			On_FilterManager.EndCapture += FilterManager_EndCapture;
			ScreenOcclusion = ModContent.Request<Effect>("Everglow/Yggdrasil/Effects/Occlusion", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		}
	}

	private void FilterManager_EndCapture(On_FilterManager.orig_EndCapture orig, FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
	{
		// 直接从RT池子里取
		var renderTargets = Ins.RenderTargetPool.GetRenderTarget2DArray(4);
		screen = renderTargets.Resource[0];
		OcclusionRender = renderTargets.Resource[1];
		EffectTarget = renderTargets.Resource[2];
		TotalEffeftsRender = renderTargets.Resource[3];

		GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
		GetOrig(graphicsDevice);

		graphicsDevice.SetRenderTarget(OcclusionRender);
		graphicsDevice.Clear(Color.Transparent);
		bool flag = DrawOcclusion(Ins.Batch);

		graphicsDevice.SetRenderTarget(EffectTarget);
		graphicsDevice.Clear(Color.Transparent);
		DrawEffect(Ins.Batch);

		if (flag)
		{
			//保存原图
			graphicsDevice.SetRenderTarget(screen);
			graphicsDevice.Clear(Color.Transparent);
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
			Main.spriteBatch.End();

			graphicsDevice.SetRenderTarget(TotalEffeftsRender);
			graphicsDevice.Clear(Color.Transparent);
			graphicsDevice.Textures[1] = OcclusionRender;
			graphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			ScreenOcclusion.CurrentTechnique.Passes[0].Apply();

			Main.spriteBatch.Draw(EffectTarget, Vector2.Zero, Color.White);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

			graphicsDevice.SetRenderTarget(Main.screenTarget);
			graphicsDevice.Clear(Color.Transparent);

			Main.spriteBatch.Draw(screen, Vector2.Zero, Color.White);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			Main.spriteBatch.Draw(TotalEffeftsRender, Vector2.Zero, Color.White);
			Main.spriteBatch.End();
		}
		screen = null;
		renderTargets.Release();
		orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
	}

	private bool DrawOcclusion(VFXBatch spriteBatch)//遮盖层
	{
		spriteBatch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.AnisotropicWrap, RasterizerState.CullNone);

		Effect effect = YggdrasilContent.QuickEffect("Effects/Null");
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(0, 0, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes[0].Apply();
		bool flag = false;
		foreach (Projectile proj in Main.projectile)
		{
			if (proj.active)
			{
				if (proj.ModProjectile is IOcclusionProjectile ModProj)
				{
					flag = true;
					ModProj.DrawOcclusion(spriteBatch);
				}
			}
		}
		spriteBatch.End();
		return flag;
	}

	private bool DrawEffect(VFXBatch spriteBatch)//特效层
	{
		spriteBatch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.AnisotropicWrap, RasterizerState.CullNone);

		Effect MeleeTrail = YggdrasilContent.QuickEffect("Effects/FlameTrail");
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
		MeleeTrail.Parameters["uTime"].SetValue((float)Main.timeForVisualEffects * 0.007f);
		Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Yggdrasil/CorruptWormHive/Projectiles/FlameLine", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>("Everglow/Yggdrasil/CorruptWormHive/Projectiles/DeathSickle_Color", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
		MeleeTrail.CurrentTechnique.Passes["Trail"].Apply();
		bool flag = false;
		foreach (Projectile proj in Main.projectile)
		{
			if (proj.active)
			{
				if (proj.ModProjectile is IOcclusionProjectile ModProj)
				{
					flag = true;
					ModProj.DrawEffect(spriteBatch);
				}
			}
		}
		spriteBatch.End();
		return flag;
	}

	private void GetOrig(GraphicsDevice graphicsDevice)
	{
		graphicsDevice.SetRenderTarget(screen);
		graphicsDevice.Clear(Color.Transparent);
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.AnisotropicWrap, RasterizerState.CullNone);
		Ins.Batch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
		Ins.Batch.End();
	}
}