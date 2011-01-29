using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Tree
{
    Vector2 position;
    Vector2[] leaves;
    static Random r = new Random();
    int height;
    public float scrollXValue;

    public Tree(Vector2 pos)
    {
        position = pos;
        leaves = new Vector2[120];
        height = r.Next(10, 15);
        for (int i = 0; i < leaves.Length; i++)
            leaves[i] = new Vector2(r.Next(-7, 7) + pos.X, r.Next(-7, 7) + pos.Y - height);
        
    }

    public void draw()
    {
        JewSaver.primitiveBatch.Begin(PrimitiveType.LineList);
        Vector2 currentPos = position;
        currentPos.X -= scrollXValue;
        JewSaver.primitiveBatch.AddLine(currentPos, currentPos + new Vector2(0, -height), Color.Brown, 3);
        
        JewSaver.primitiveBatch.End();
        JewSaver.primitiveBatch.Begin(PrimitiveType.PointList);
        for (int i = 0; i < leaves.Length / 2; i++)
        {
            Vector2 scrolled = leaves[i];
            scrolled.X -= scrollXValue;
            JewSaver.primitiveBatch.AddVertex(scrolled, Color.Brown);
        }
        for (int i = leaves.Length / 2; i < leaves.Length; i++)
        {
            Vector2 scrolled = leaves[i];
            scrolled.X -= scrollXValue;
            JewSaver.primitiveBatch.AddVertex(scrolled, Color.Green);
        }

        JewSaver.primitiveBatch.End();
    }

    public void update() 
    { ;}

}
