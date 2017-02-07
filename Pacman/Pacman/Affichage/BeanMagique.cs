using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman
{
    class BeanMagique : Affichage
    {
        ObjetAnime bean;

        public BeanMagique(ObjetAnime bean) : base()
        {
            this.bean = bean;
        }

    }
}
