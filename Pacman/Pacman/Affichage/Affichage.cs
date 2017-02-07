using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman
{
    public abstract class Affichage
    {
        public static int VX = 31;
        public static int VY = 28;

        public virtual void Afficher() {}


        public void AfficherMap(ObjetAnime objet, SpriteBatch spriteBatch, int i, byte[,] map)
        {
            spriteBatch.Begin();
            for (int x = 0; x < VX; x++)
            {
                for (int y = 0; y < VY; y++)
                {
                    if (map[x, y] == i)
                    {
                        int xpos, ypos;
                        xpos = x * 20;
                        ypos = y * 20;
                        Vector2 pos = new Vector2(ypos, xpos);
                        spriteBatch.Draw(objet.Texture, pos, Color.White);
                    }
                }
            }
            spriteBatch.End();
        }


    }
}
