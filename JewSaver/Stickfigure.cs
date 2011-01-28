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

    public Stickfigure(Vector2 position)
    {
        this.position = position;
    }

    public void draw() {    
        VertexPositionColor[] points = {new VertexPositionColor(new Vector3(position, 0f), Color.White)};

        JewSaver.spriteBatch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
            PrimitiveType.PointList,
            points,
            0,
            1);
    }

    public void update(){
        if (!moving) return;
        ;
    }


}
