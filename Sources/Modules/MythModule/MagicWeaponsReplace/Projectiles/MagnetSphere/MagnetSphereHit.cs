﻿using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Modules.MythModule.Common;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.MagnetSphere
{
    public class MagnetSphereHit : ModProjectile, IWarpProjectile, IBloomProjectile
    {
        protected override bool CloneNewInstances => false;
        public override bool IsCloneable => false;

        public override void SetDefaults()
        {
            Projectile.width = 240;
            Projectile.height = 240;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 4;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.95f;

            if (Projectile.timeLeft <= 198)
            {
                Projectile.friendly = false;
            }
            float LightS = Projectile.timeLeft / 2f - 95f;
            if(LightS > 0)
            {
                Lighting.AddLight((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16), 0, LightS * 0.83f, LightS * 0.8f);
            }

            int MaxC = (int)(Projectile.ai[0] / 4 + 5);
            MaxC = Math.Min(26, MaxC);
            if (Projectile.timeLeft >= 200)
            {
                for (int x = 0; x < MaxC; x++)
                {
                    SparkVelocity[x] = new Vector2(0, Projectile.ai[0]).RotatedByRandom(6.283) * Main.rand.NextFloat(0.35f, 0.7f);
                    SparkOldPos[x, 0] = Projectile.Center;
                }
            }

            for (int x = 0; x < MaxC; x++)
            {
                for (int y = 139; y > 0; y--)
                {
                    SparkOldPos[x, y] = SparkOldPos[x, y - 1];
                }
                if (Collision.SolidCollision(SparkOldPos[x, 0] + new Vector2(SparkVelocity[x].X, 0), 0, 0))
                {
                    SparkVelocity[x].X *= -0.95f;
                }
                if (Collision.SolidCollision(SparkOldPos[x, 0] + new Vector2(0, SparkVelocity[x].Y), 0, 0))
                {
                    SparkVelocity[x].Y *= -0.95f;
                }
                SparkOldPos[x, 0] += SparkVelocity[x];

                if (SparkVelocity[x].Length() > 0.3f)
                {
                    SparkVelocity[x] *= 0.95f;
                }
                if(Main.rand.NextBool(4))
                {
                    SparkVelocity[x] = SparkVelocity[x].RotatedBy(Main.rand.NextFloat(-0.8f,0.8f));
                }
            }
            Projectile.velocity *= 0;
        }
        private static void DrawTexCircle(float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
        {
            List<Vertex2D> circle = new List<Vertex2D>();
            for (int h = 0; h < radious / 2; h++)
            {
                circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 1, 0)));
                circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0, 0)));
            }
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0.5f, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(addRot), color, new Vector3(0.5f, 0, 0)));
            if (circle.Count > 0)
            {
                Main.graphics.GraphicsDevice.Textures[0] = tex;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, circle.ToArray(), 0, circle.Count - 2);
            }
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D Shadow = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CursedFlames/CursedHitLight");
            float Dark = Math.Max(((Projectile.timeLeft - 150) / 50f), 0);
            Main.spriteBatch.Draw(Shadow, Projectile.Center - Main.screenPosition, null, new Color(0, 229, 206,0) * Dark, 0, Shadow.Size() / 2f, 2.2f * Projectile.ai[0] / 15f * Dark, SpriteEffects.None, 0);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D Shadow = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CursedFlames/CursedHit");
            float Dark = Math.Max(((Projectile.timeLeft - 150) / 50f), 0);
            Main.spriteBatch.Draw(Shadow,Projectile.Center - Main.screenPosition,null,Color.White * Dark,0,Shadow.Size() / 2f,2.2f * Projectile.ai[0] / 15f, SpriteEffects.None,0);
            Texture2D light = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CursedFlames/CursedHitStar");
            Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), 0 + Projectile.ai[1], light.Size() / 2f, new Vector2(1f, Dark * Dark) * Projectile.ai[0] / 20f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), 1.57f + Projectile.ai[1], light.Size() / 2f, new Vector2(0.8f, Dark * Projectile.ai[0] / 20f), SpriteEffects.None, 0);

            float size = Math.Clamp(Projectile.timeLeft / 8f - 10, 0f, 20f);
            if(size > 0)
            {
                DrawSpark(Color.White * 0.5f, size * 3, MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/LineDark"));
                DrawSpark(new Color(0, 255, 240, 0), size * 3, MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/LineLight"));
            }
            Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), (float)(Math.PI / 4d) + Projectile.ai[1], light.Size() / 2f, new Vector2(0.6f, Dark * Projectile.ai[0] / 20f), SpriteEffects.None, 0);
            Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), (float)(Math.PI / 4d * 3) + Projectile.ai[1], light.Size() / 2f, new Vector2(0.6f, Dark * Projectile.ai[0] / 20f), SpriteEffects.None, 0);
            return false;
        }

        private Vector2[,] SparkOldPos = new Vector2[27, 140];
        private Vector2[] SparkVelocity = new Vector2[27];
        internal void DrawSpark(Color c0, float width, Texture2D tex)
        {
            int MaxC = (int)(Projectile.ai[0] / 4 + 5);
            MaxC = Math.Min(26, MaxC);
            List<Vertex2D> bars = new List<Vertex2D>();
            for (int x = 0; x < MaxC; x++)
            {
                int TrueL = 0;
                for (int i = 1; i < 140; ++i)
                {
                    if (SparkOldPos[x, i] == Vector2.Zero)
                    {
                        break;
                    }

                    TrueL++;
                }
                for (int i = 1; i < 140; ++i)
                {
                    if (SparkOldPos[x, i] == Vector2.Zero)
                    {
                        break;
                    }

                    var normalDir = SparkOldPos[x, i - 1] - SparkOldPos[x, i];
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                    var factor = i / (float)TrueL;
                    var w = MathHelper.Lerp(1f, 0.05f, factor);
                    float x0 = 1 - factor;
                    if (i == 1)
                    {
                        bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * -width + new Vector2(5f, 5f) - Main.screenPosition, Color.Transparent, new Vector3(x0, 1, w)));
                        bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * width + new Vector2(5f, 5f) - Main.screenPosition, Color.Transparent, new Vector3(x0, 0, w)));
                    }
                    bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * -width + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 1, w)));
                    bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * width + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 0, w)));
                    if (i == 139)
                    {
                        bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * -width + new Vector2(5f, 5f) - Main.screenPosition, Color.Transparent, new Vector3(x0, 1, w)));
                        bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * width + new Vector2(5f, 5f) - Main.screenPosition, Color.Transparent, new Vector3(x0, 0, w)));
                    }
                }
                Texture2D t = tex;
                Main.graphics.GraphicsDevice.Textures[0] = t;
            }
            if (bars.Count > 3)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }
        }

        public void DrawWarp()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Effect KEx = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/DrawWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            KEx.CurrentTechnique.Passes[0].Apply();
            float value = (200 - Projectile.timeLeft) / (float)Projectile.timeLeft * 1.4f;
            float colorV = 0.9f * (1 - value);
            if(Projectile.ai[0] >= 10)
            {
                colorV *= Projectile.ai[0] / 10f;
            }
            Texture2D t = MythContent.QuickTexture("OmniElementItems/Projectiles/Wave");
            DrawTexCircle(value * 16 * Projectile.ai[0], 100, new Color(colorV, colorV * 0.2f, colorV, 0f), Projectile.Center - Main.screenPosition, t);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public void DrawBloom()
        {
            float size = Math.Clamp(Projectile.timeLeft / 8f - 60, 0f, 20f);
            if (size > 0)
            {
                DrawSpark(new Color(255, 20, 229, 206), size, MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/SparkLight"));
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
        }
    }
}