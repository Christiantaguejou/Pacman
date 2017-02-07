using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman
{
    class FantomeRose : Fantomes
    {
        ObjetAnime fantome;
        public FantomeRose(ObjetAnime fantome, Boolean mangeable, Boolean mort) : base(fantome, mangeable, mort)
        {
            this.fantome = fantome;
            mangeable = false;
            mort = false;
        }

    }
}
