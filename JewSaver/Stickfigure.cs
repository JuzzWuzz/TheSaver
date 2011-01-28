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
    private Vector2 crotch, shoulder, lHand, rHand, lFoot, rFoot;
    private float roShoulder, roLHand, roRHand, roLFoot, roRFoot;

    //VertexPositionColor[] points;

    public Stickfigure(Vector2 position)
    {
        this.position = position;
        roShoulder = roLHand = roRHand = roLFoot = roRFoot = 0f;

        setLimbs();
    }

    private void setLimbs()
    {
        crotch = position;
        //shoulder = position + Vector2.Dot(shoulder, Vector2
    }

    public void draw() {

        VertexPositionColor[] points = {new VertexPositionColor(new Vector3(position, 0f), Color.Red),
                                           new VertexPositionColor(new Vector3(position + new Vector2(0, 10), 0f), Color.Red)};

        JewSaver.spriteBatch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList,
            points,
            0,
            1);
    }

    public void update(){
        if (!moving) return;
        ;



    }


}
