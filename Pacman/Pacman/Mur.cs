using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman
{
    class Mur : Affichage //Héritage de la classe abstraie Affichage
    {
        ObjetAnime mur;

        public Mur(ObjetAnime mur)
        {
            this.mur = mur;
        }

    }
}
