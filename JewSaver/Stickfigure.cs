using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Stickfigure
{
    private Vector2 position;
    private bool moving = false;

    Stickfigure(Vector2 position)
    {
        this.position = position;
    }

    void draw() {    
        VertexPositionColor[] points = {new VertexPositionColor(position, Color.Black)};


        JewSaver.spriteBatch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
            PrimitiveType.PointList,
            points,
            0,
            1);
    }

    void update(){;}


}
