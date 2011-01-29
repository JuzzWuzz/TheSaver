using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Tumbleweed
{
    Texture2D texture;
    float radius;

    public Tumbleweed(float radius, Texture2D tumbleTex)
    {
        texture = tumbleTex;
        this.radius = radius;
    }
}
