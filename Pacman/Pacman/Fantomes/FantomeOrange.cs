using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman
{
    /// <summary>
    /// Cette classe Hérite de la classe Fantomes et de toutes ses méthodes
    /// </summary>
    class FantomeOrange : Fantomes
    {

        ObjetAnime fantome;
        public FantomeOrange(ObjetAnime fantome, Boolean mangeable, Boolean mort) : base(fantome, mangeable, mort)
        {
            this.fantome = fantome;
            mangeable = false;
            mort = false;
        }

    }
}
