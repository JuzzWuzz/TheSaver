using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


public class MosesActionIcon
{
    public enum ActionType { JUMP, RUN, STOP };
    public static float change = 0;

    public static void draw(Vector2 pos, ActionType type)
    {
        Vector2 centre = pos + 
            new Vector2(0, -20 + (float)Math.Sin((double)change * 3) * 5f);

        JewSaver.primitiveBatch.Begin(PrimitiveType.LineList);
        switch (type)
        {
            case ActionType.JUMP:
                JewSaver.primitiveBatch.AddLine(
                    centre + new Vector2(-8, 8),
                    centre + new Vector2(8, -8), Color.Blue, 3);
                JewSaver.primitiveBatch.AddLine(
                    centre + new Vector2(2, -8),
                    centre + new Vector2(8, -8), Color.Blue, 3);
                JewSaver.primitiveBatch.AddLine(
                    centre + new Vector2(8, -2),
                    centre + new Vector2(8, -8), Color.Blue, 3);
                break;
            case ActionType.RUN:
                JewSaver.primitiveBatch.AddLine(
                    centre + new Vector2(-8, -8),
                    centre + new Vector2(0, 0), Color.Green, 3);
                JewSaver.primitiveBatch.AddLine(
                    centre + new Vector2(-8, 8),
                    centre + new Vector2(0, 0), Color.Green, 3);
                JewSaver.primitiveBatch.AddLine(
                    centre + new Vector2(0, -8),
                    centre + new Vector2(8, 0), Color.Green, 3);
                JewSaver.primitiveBatch.AddLine(
                    centre + new Vector2(0, 8),
                    centre + new Vector2(8, 0), Color.Green, 3);
                break;
            case ActionType.STOP:
                JewSaver.primitiveBatch.AddLine(
                    centre + new Vector2(4, -8),
                    centre + new Vector2(4, 8), Color.Red, 3);
                JewSaver.primitiveBatch.AddLine(
                    centre + new Vector2(-4, 8),
                    centre + new Vector2(-4, -8), Color.Red, 3);
                break;
        }
        JewSaver.primitiveBatch.End();
    }
}
