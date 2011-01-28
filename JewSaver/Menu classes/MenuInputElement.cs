using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public interface MenuInputElement
{
    bool Visible { get; set; } // should element be drawn
    bool Enabled{ get; set; } // can element receive input
    void CheckInput(); // process input & fire event if necessary
    void Draw(SpriteBatch spriteBatch); // draw all sprites that make up the menu element
}
