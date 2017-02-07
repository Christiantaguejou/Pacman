using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    class Bean : Affichage
    {
        ObjetAnime bean;

        public Bean(ObjetAnime bean) : base()
        {
            this.bean = bean;
        }

    }
}
