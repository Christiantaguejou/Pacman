using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman
{
    /// <summary>
    /// Cette classe sera utile lors de l'utilisation de l'algorithme de Dijkstra
    /// Cf: Fantome Rouge
    /// </summary>
    class Sommet
    {
        public const int INFINI = 1000000;
        public int Potentiel;
        public bool Marque;
        public Coord Pred;

        public Sommet()
        {
            Potentiel = INFINI;
            Marque = false;
            Pred = null;
        }
    }

}
