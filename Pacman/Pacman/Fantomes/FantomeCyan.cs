using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman
{
    /// <summary>
    /// Cette classe Hérite de la classe Fantomes et de toutes ses méthodes
    /// </summary>
    class FantomeCyan : Fantomes
    {
        ObjetAnime fantome;
        public FantomeCyan(ObjetAnime fantome, Boolean mangeable, Boolean mort) : base(fantome, mangeable, mort)
        {
            this.fantome = fantome;
            mangeable = false;
            mort = false;
        }

        public void Deplacement()
        {

        }
    }
}
