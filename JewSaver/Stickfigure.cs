﻿using System;
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

    VertexPositionColor[] points;

    public Stickfigure(Vector2 position)
    {
        this.position = position;
        roShoulder = roLHand = roRHand = roLFoot = roRFoot = 0f;

        shoulder = new Vector2(0, -10);
        lHand = rHand = new Vector2(5, 0);
        lFoot = rFoot = new Vector2(2, 7);

        setLimbs();
    }

    private void setLimbs()
    {
        crotch = position;
        Vector2 shoulderPoint = crotch + shoulder;
        Vector2 lHandPoint = shoulderPoint + lHand;
        Vector2 rHandPoint = shoulderPoint + rHand;
        Vector2 rFootPoint = crotch + rFoot;
        Vector2 lFootPoint = crotch + lFoot;

        points = new VertexPositionColor[10];
        points[0] = new VertexPositionColor(new Vector3(crotch, 0f), Color.Red);
        points[1] = new VertexPositionColor(new Vector3(shoulderPoint, 0f), Color.Yellow);
        points[2] = new VertexPositionColor(new Vector3(crotch, 0f), Color.Red);
        points[3] = new VertexPositionColor(new Vector3(lHandPoint, 0f), Color.Yellow);
        points[0] = new VertexPositionColor(new Vector3(crotch, 0f), Color.Red);
        points[1] = new VertexPositionColor(new Vector3(shoulderPoint, 0f), Color.Yellow);
        

    }

    public void draw() {

        JewSaver.spriteBatch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList,
            points,
            0,
            3);
    }

    public void update(){
        if (!moving) return;
        ;



    }


}
