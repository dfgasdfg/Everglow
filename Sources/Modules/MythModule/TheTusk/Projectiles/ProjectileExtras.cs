﻿using Terraria.GameContent;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Projectiles
{
    public static class ProjectileExtras
    {
        public static void YoyoAI(int index, float seconds, float length, float acceleration = 14f, float rotationSpeed = 0.45f)
        {
            Projectile projectile = Main.projectile[index];
            bool flag = false;
            for (int i = 0; i < projectile.whoAmI; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == projectile.owner && Main.projectile[i].type == projectile.type)
                {
                    flag = true;
                }
            }
            if (projectile.owner == Main.myPlayer)
            {
                projectile.localAI[0] += 1f;
                if (flag)
                {
                    projectile.localAI[0] += (float)Main.rand.Next(10, 31) * 0.1f;
                }
                float num = projectile.localAI[0] / 60f;
                num /= (1f + Main.player[projectile.owner].GetAttackSpeed(DamageClass.Generic)) / 2f;
                if (num > seconds)
                {
                    projectile.ai[0] = -1f;
                }
            }
            bool flag2 = false;
            if (Main.player[projectile.owner].dead)
            {
                projectile.Kill();
                return;
            }
            if (!flag2 && !flag)
            {
                Main.player[projectile.owner].heldProj = projectile.whoAmI;
                Main.player[projectile.owner].itemAnimation = 2;
                Main.player[projectile.owner].itemTime = 2;
                if (projectile.position.X + (float)(projectile.width / 2) > Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2))
                {
                    Main.player[projectile.owner].ChangeDir(1);
                    projectile.direction = 1;
                }
                else
                {
                    Main.player[projectile.owner].ChangeDir(-1);
                    projectile.direction = -1;
                }
            }
            if (Utils.HasNaNs(projectile.velocity))
            {
                projectile.Kill();
            }
            projectile.timeLeft = 6;
            float num2 = length;
            if (Main.player[projectile.owner].yoyoString)
            {
                num2 = num2 * 1.25f + 30f;
            }
            num2 /= (1f + Main.player[projectile.owner].GetAttackSpeed(DamageClass.Generic) * 3f) / 4f;
            float num3 = acceleration / ((1f + Main.player[projectile.owner].GetAttackSpeed(DamageClass.Generic) * 3f) / 4f);
            float num4 = 14f - num3 / 2f;
            float num5 = 5f + num3 / 2f;
            if (flag)
            {
                num5 += 20f;
            }
            if (projectile.ai[0] >= 0f)
            {
                if (projectile.velocity.Length() > num3)
                {
                    projectile.velocity *= 0.98f;
                }
                bool flag3 = false;
                bool flag4 = false;
                Vector2 vector = Main.player[projectile.owner].Center - projectile.Center;
                if (vector.Length() > num2)
                {
                    flag3 = true;
                    if ((double)vector.Length() > (double)num2 * 1.3)
                    {
                        flag4 = true;
                    }
                }
                if (projectile.owner == Main.myPlayer)
                {
                    if (!Main.player[projectile.owner].channel || Main.player[projectile.owner].stoned || Main.player[projectile.owner].frozen)
                    {
                        projectile.ai[0] = -1f;
                        projectile.ai[1] = 0f;
                        projectile.netUpdate = true;
                    }
                    else
                    {
                        Vector2 vector2 = Main.ReverseGravitySupport(Main.MouseScreen, 0f) + Main.screenPosition;
                        float x = vector2.X;
                        float y = vector2.Y;
                        Vector2 vector3 = new Vector2(x, y) - Main.player[projectile.owner].Center;
                        if (vector3.Length() > num2)
                        {
                            vector3.Normalize();
                            vector3 *= num2;
                            vector3 = Main.player[projectile.owner].Center + vector3;
                            x = vector3.X;
                            y = vector3.Y;
                        }
                        if (projectile.ai[0] != x || projectile.ai[1] != y)
                        {
                            Vector2 value = new Vector2(x, y);
                            Vector2 vector4 = value - Main.player[projectile.owner].Center;
                            if (vector4.Length() > num2 - 1f)
                            {
                                vector4.Normalize();
                                vector4 *= num2 - 1f;
                                value = Main.player[projectile.owner].Center + vector4;
                                x = value.X;
                                y = value.Y;
                            }
                            projectile.ai[0] = x;
                            projectile.ai[1] = y;
                            projectile.netUpdate = true;
                        }
                    }
                }
                if (flag4 && projectile.owner == Main.myPlayer)
                {
                    projectile.ai[0] = -1f;
                    projectile.netUpdate = true;
                }
                if (projectile.ai[0] >= 0f)
                {
                    if (flag3)
                    {
                        num4 /= 2f;
                        num3 *= 2f;
                        if (projectile.Center.X > Main.player[projectile.owner].Center.X && projectile.velocity.X > 0f)
                        {
                            projectile.velocity.X = projectile.velocity.X * 0.5f;
                        }
                        if (projectile.Center.Y > Main.player[projectile.owner].Center.Y && projectile.velocity.Y > 0f)
                        {
                            projectile.velocity.Y = projectile.velocity.Y * 0.5f;
                        }
                        if (projectile.Center.X < Main.player[projectile.owner].Center.X && projectile.velocity.X > 0f)
                        {
                            projectile.velocity.X = projectile.velocity.X * 0.5f;
                        }
                        if (projectile.Center.Y < Main.player[projectile.owner].Center.Y && projectile.velocity.Y > 0f)
                        {
                            projectile.velocity.Y = projectile.velocity.Y * 0.5f;
                        }
                    }
                    Vector2 value2 = new Vector2(projectile.ai[0], projectile.ai[1]);
                    Vector2 vector5 = value2 - projectile.Center;
                    projectile.velocity.Length();
                    if (vector5.Length() > num5)
                    {
                        vector5.Normalize();
                        vector5 *= num3;
                        projectile.velocity = (projectile.velocity * (num4 - 1f) + vector5) / num4;
                    }
                    else if (flag)
                    {
                        if ((double)projectile.velocity.Length() < (double)num3 * 0.6)
                        {
                            vector5 = projectile.velocity;
                            vector5.Normalize();
                            vector5 *= num3 * 0.6f;
                            projectile.velocity = (projectile.velocity * (num4 - 1f) + vector5) / num4;
                        }
                    }
                    else
                    {
                        projectile.velocity *= 0.8f;
                    }
                    if (flag && !flag3 && (double)projectile.velocity.Length() < (double)num3 * 0.6)
                    {
                        projectile.velocity.Normalize();
                        projectile.velocity *= num3 * 0.6f;
                    }
                }
            }
            else
            {
                num4 = (float)((int)((double)num4 * 0.8));
                num3 *= 1.5f;
                projectile.tileCollide = false;
                Vector2 vector6 = Main.player[projectile.owner].position - projectile.Center;
                float num6 = vector6.Length();
                if (num6 < num3 + 10f || num6 == 0f)
                {
                    projectile.Kill();
                }
                else
                {
                    vector6.Normalize();
                    vector6 *= num3;
                    projectile.velocity = (projectile.velocity * (num4 - 1f) + vector6) / num4;
                }
            }
            projectile.rotation += rotationSpeed;
        }
        public static void DrawString(int index, Vector2 to = default(Vector2))
        {
            Projectile projectile = Main.projectile[index];
            Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
            Vector2 vector = mountedCenter;
            vector.Y += Main.player[projectile.owner].gfxOffY;
            if (to != default(Vector2))
            {
                vector = to;
            }
            float num = projectile.Center.X - vector.X;
            float num2 = projectile.Center.Y - vector.Y;
            Math.Sqrt((double)(num * num + num2 * num2));
            float rotation = (float)Math.Atan2((double)num2, (double)num) - 1.57f;
            if (!projectile.counterweight)
            {
                int num3 = -1;
                if (projectile.position.X + (float)(projectile.width / 2) < Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2))
                {
                    num3 = 1;
                }
                num3 *= -1;
                Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(num2 * (float)num3), (double)(num * (float)num3));
            }
            bool flag = true;
            if (num == 0f && num2 == 0f)
            {
                flag = false;
            }
            else
            {
                float num4 = (float)Math.Sqrt((double)(num * num + num2 * num2));
                num4 = 12f / num4;
                num *= num4;
                num2 *= num4;
                vector.X -= num * 0.1f;
                vector.Y -= num2 * 0.1f;
                num = projectile.position.X + (float)projectile.width * 0.5f - vector.X;
                num2 = projectile.position.Y + (float)projectile.height * 0.5f - vector.Y;
            }
            while (flag)
            {
                float num5 = 12f;
                float num6 = (float)Math.Sqrt((double)(num * num + num2 * num2));
                float num7 = num6;
                if (float.IsNaN(num6) || float.IsNaN(num7))
                {
                    flag = false;
                }
                else
                {
                    if (num6 < 20f)
                    {
                        num5 = num6 - 8f;
                        flag = false;
                    }
                    num6 = 12f / num6;
                    num *= num6;
                    num2 *= num6;
                    vector.X += num;
                    vector.Y += num2;
                    num = projectile.position.X + (float)projectile.width * 0.5f - vector.X;
                    num2 = projectile.position.Y + (float)projectile.height * 0.1f - vector.Y;
                    if (num7 > 12f)
                    {
                        float num8 = 0.3f;
                        float num9 = Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y);
                        if (num9 > 16f)
                        {
                            num9 = 16f;
                        }
                        num9 = 1f - num9 / 16f;
                        num8 *= num9;
                        num9 = num7 / 80f;
                        if (num9 > 1f)
                        {
                            num9 = 1f;
                        }
                        num8 *= num9;
                        if (num8 < 0f)
                        {
                            num8 = 0f;
                        }
                        num8 *= num9;
                        num8 *= 0.5f;
                        if (num2 > 0f)
                        {
                            num2 *= 1f + num8;
                            num *= 1f - num8;
                        }
                        else
                        {
                            num9 = Math.Abs(projectile.velocity.X) / 3f;
                            if (num9 > 1f)
                            {
                                num9 = 1f;
                            }
                            num9 -= 0.5f;
                            num8 *= num9;
                            if (num8 > 0f)
                            {
                                num8 *= 2f;
                            }
                            num2 *= 1f + num8;
                            num *= 1f - num8;
                        }
                    }
                    rotation = (float)Math.Atan2((double)num2, (double)num) - 1.57f;
                    int stringColor = Main.player[projectile.owner].stringColor;
                    Color color = WorldGen.paintColor(stringColor);
                    if (color.R < 75)
                    {
                        color.R = 75;
                    }
                    if (color.G < 75)
                    {
                        color.G = 75;
                    }
                    if (color.B < 75)
                    {
                        color.B = 75;
                    }
                    if (stringColor == 13)
                    {
                        color = new Color(20, 20, 20);
                    }
                    else if (stringColor == 14 || stringColor == 0)
                    {
                        color = new Color(200, 200, 200);
                    }
                    else if (stringColor == 28)
                    {
                        color = new Color(163, 116, 91);
                    }
                    else if (stringColor == 27)
                    {
                        color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
                    }
                    color.A = (byte)((float)color.A * 0.4f);
                    float num10 = 0.5f;
                    color = Lighting.GetColor((int)vector.X / 16, (int)(vector.Y / 16f), color);
                    color = new Color((int)((byte)((float)color.R * num10)), (int)((byte)((float)color.G * num10)), (int)((byte)((float)color.B * num10)), (int)((byte)((float)color.A * num10)));
                    Texture2D tex = TextureAssets.FishingLine.Value;
                    Main.spriteBatch.Draw(tex, new Vector2(vector.X - Main.screenPosition.X + (float)tex.Width * 0.5f, vector.Y - Main.screenPosition.Y + (float)tex.Height * 0.5f) - new Vector2(6f, 0f), new Rectangle?(new Rectangle(0, 0, tex.Width, (int)num5)), color, rotation, new Vector2((float)tex.Width * 0.5f, 0f), 1f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}
