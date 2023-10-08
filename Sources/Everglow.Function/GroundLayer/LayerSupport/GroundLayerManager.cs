using Everglow.Commons.GroundLayer.Basics;

namespace Everglow.Commons.GroundLayer.LayerSupport
{
	public class GroundLayerManager : ModSystem
	{
		static GroundLayerManager()
		{
			On_Main.DoDraw_WallsTilesNPCs += On_Main_DoDraw_WallsTilesNPCs;
			On_Main.DrawLiquid += On_Main_DrawLiquid;
		}
		private static void On_Main_DrawLiquid(On_Main.orig_DrawLiquid orig, Main self, bool bg, int waterStyle, float Alpha, bool drawSinglePassLiquids)
		{
			orig(self, bg, waterStyle, Alpha, drawSinglePassLiquids);
			Instance.DrawForegroundLayers(Main.spriteBatch);
		}
		private static void On_Main_DoDraw_WallsTilesNPCs(On_Main.orig_DoDraw_WallsTilesNPCs orig, Main self)
		{
			Instance.SetCameraPos(new(Main.LocalPlayer.Center, 1600));
			Instance.DrawBackgroundLayers(Main.spriteBatch);
			orig(self);
		}
		public static GroundLayerManager Instance => ModContent.GetInstance<GroundLayerManager>();
		StructCollection<Layer> layerCollection = new();
		Vector3 CameraPos;
		public bool WaitLoadTexture = false;
		Dictionary<string, int> layerMap = new();
		bool needResort;
		public bool AddLayer(string layerName, string layerTexturePath, Vector3 layerPos)
		{
			if (layerMap.ContainsKey(layerName))
			{
				return false;
			}
			var layer = new Layer(layerTexturePath) { Position = layerPos };
			if (layerCollection.Add(layer))
			{
				layerMap[layerName] = layer.UniqueID;
				needResort = true;
				return true;
			}
			return false;
		}
		public bool RemoveLayer(string layerName)
		{
			if (!layerMap.TryGetValue(layerName, out int id))
			{
				return false;
			}
			return layerCollection.Remove(id);
		}
		public void Clear()
		{
			layerMap.Clear();
			layerCollection.Clear();
		}
		public bool GetLayer(string layerName, ref Layer layer)
		{
			if (layerMap.TryGetValue(layerName, out int id))
			{
				layer = layerCollection[id];
				return true;
			}
			layer = default;
			return false;
		}
		public Vector3 GetCameraPos() => CameraPos;
		public void SetCameraPos(Vector3 cameraPos)
		{
			if (cameraPos.Z < 0)
			{
				throw new Exception("Can't Support Camera Set After Screen");
			}
			CameraPos = cameraPos;
		}
		private void Resort()
		{
			if(!needResort)
			{
				return;
			}
			layerCollection.Sort((i1, i2) => i1.Position.Z.CompareTo(i2.Position.Z));
			needResort = false;
		}
		public void DrawForegroundLayers(SpriteBatch sprite)
		{
			Resort();
			Layer[] layers = layerCollection.GetUpdateElements();
			for (int i = 0; i < layers.Length; i++)
			{
				Layer drawLayer = layers[i];
				if (drawLayer.Position.Z < 0)
				{
					continue;
				}
				if (drawLayer.Position.Z > CameraPos.Z)
				{
					return;
				}
				if (!drawLayer.PrePareDraw(out Texture2D texture, !WaitLoadTexture))
				{
					continue;
				}
				float f = drawLayer.Position.Z / CameraPos.Z;

				int leftX = (int)(Main.screenPosition.X * (1 - f) + CameraPos.X * f);
				int rightX = (int)((Main.screenPosition.X + Main.screenWidth) * (1 - f) + CameraPos.X * f);
				int topY = (int)(Main.screenPosition.Y * (1 - f) + CameraPos.Y * f);
				int bottomY = (int)((Main.screenPosition.Y + Main.screenHeight) * (1 - f) + CameraPos.Y * f);

				Rectangle r1 = new((int)drawLayer.Position.X, (int)drawLayer.Position.Y, texture.Width, texture.Height);
				Rectangle r2 = new(leftX, topY, rightX - leftX, bottomY - topY);
				Rectangle.Intersect(ref r1, ref r2, out Rectangle r3);

				if (r3.Width == 0 || r3.Height == 0)
				{
					continue;
				}

				float f2 = CameraPos.Z / (CameraPos.Z - drawLayer.Position.Z);
				Rectangle r4 = new((int)(CameraPos.X + (r3.X - CameraPos.X) * f2),
					(int)(CameraPos.Y + (r3.Y - CameraPos.Y) * f2),
					(int)(r3.Width * f2),
					(int)(r3.Height * f2));

				sprite.Draw(texture,
					new Rectangle((int)(r4.X - Main.screenPosition.X), (int)(r4.Y - Main.screenPosition.Y), r4.Width, r4.Height),
					new Rectangle((int)(r3.X - drawLayer.Position.X), (int)(r3.Y - drawLayer.Position.Y), r3.Width, r3.Height),
					Color.White);
			}
		}
		public void DrawBackgroundLayers(SpriteBatch sprite)
		{
			Resort();
			Layer[] layers = layerCollection.GetUpdateElements();
			for (int i = 0; i < layers.Length; i++)
			{
				Layer drawLayer = layers[i];
				if (drawLayer.Position.Z > 0)
				{
					return;
				}
				if (!drawLayer.PrePareDraw(out var texture, !WaitLoadTexture))
				{
					continue;
				}
				float f = drawLayer.Position.Z / CameraPos.Z;

				int leftX = (int)(Main.screenPosition.X * (1 - f) + CameraPos.X * f);
				int rightX = (int)((Main.screenPosition.X + Main.screenWidth) * (1 - f) + CameraPos.X * f);
				int topY = (int)(Main.screenPosition.Y * (1 - f) + CameraPos.Y * f);
				int bottomY = (int)((Main.screenPosition.Y + Main.screenHeight) * (1 - f) + CameraPos.Y * f);

				Rectangle r1 = new((int)drawLayer.Position.X, (int)drawLayer.Position.Y, texture.Width, texture.Height);
				Rectangle r2 = new(leftX, topY, rightX - leftX, bottomY - topY);
				Rectangle.Intersect(ref r1, ref r2, out Rectangle r3);

				if (r3.Width == 0 || r3.Height == 0)
				{
					continue;
				}

				float f2 = CameraPos.Z / (CameraPos.Z - drawLayer.Position.Z);
				Rectangle r4 = new((int)(CameraPos.X + (r3.X - CameraPos.X) * f2),
					(int)(CameraPos.Y + (r3.Y - CameraPos.Y) * f2),
					(int)(r3.Width * f2),
					(int)(r3.Height * f2));

				sprite.Draw(texture,
					new Rectangle((int)(r4.X - Main.screenPosition.X), (int)(r4.Y - Main.screenPosition.Y), r4.Width, r4.Height),
					new Rectangle((int)(r3.X - drawLayer.Position.X), (int)(r3.Y - drawLayer.Position.Y), r3.Width, r3.Height),
					Color.White);
			}
		}
		public override void OnWorldUnload()
		{
			Clear();
		}
	}
	class TestCommand : ModCommand
	{
		public override string Command => "Layer";

		public override CommandType Type => CommandType.Chat;

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			if (args.Length == 0 || args[0] == "clear")
			{
				GroundLayerManager.Instance.Clear();
			}
			else if (args[0] == "new")
			{
				for (int i = 0; i < 100; i++)
				{
					GroundLayerManager.Instance.AddLayer("Test" + i, "Everglow/Commons/Textures/Noise_melting", new(Main.LocalPlayer.Center + Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * Main.rand.NextFloat(3200), Main.rand.NextFloat(-1600, 1600)));
				}
			}
		}
	}
}