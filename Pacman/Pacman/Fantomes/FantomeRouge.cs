using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    class FantomeRouge : Fantomes //Héritage de la classe Fantomes
    {
        ObjetAnime fantome;
        public FantomeRouge(ObjetAnime fantome, Boolean mangeable, Boolean mort) : base(fantome, mangeable, mort)
        {
            this.fantome = fantome;
            mangeable = false;
            mort = false;
        }
        
        public Coord Dijkstra(ObjetAnime arr, ObjetAnime dep, byte[,] map, Coord positionInitial)
        {

            Sommet[,] mesSommets = new Sommet[Affichage.VX, Affichage.VY];

            for (int i = 0; i < Affichage.VX; i++)
            {
                for (int j = 0; j < Affichage.VY; j++)
                {
                    if (map[i, j] != 0)
                    {
                        mesSommets[i, j] = new Sommet();
                    }
                }
            }

            mesSommets[arr.coord.X, arr.coord.Y].Potentiel = 0;
            Coord courant = arr.coord;
            //Honnetement, j'ai pas tout compris à ce while donc bon...
            while (courant != dep.coord)
            {
                Sommet z = mesSommets[courant.X, courant.Y];
                z.Marque = true;

                //Haut
                if (courant.X > 0)
                {
                    if (mesSommets[courant.X - 1, courant.Y] != null)
                    {
                        Sommet s = mesSommets[courant.X - 1, courant.Y];
                        if (s.Potentiel > z.Potentiel + 1)
                        {
                            s.Potentiel = z.Potentiel + 1;
                            s.Pred = courant;
                        }
                    }
                }

                //Bas
                if (courant.X + 1 < Affichage.VX)
                {
                    if (mesSommets[courant.X + 1, courant.Y] != null)
                    {
                        Sommet s = new Sommet(); s = mesSommets[courant.X + 1, courant.Y];
                        if (s.Potentiel > z.Potentiel + 1)
                        {
                            s.Potentiel = z.Potentiel + 1;
                            s.Pred = courant;
                        }
                    }
                }

                //Gauche
                if (courant.Y > 0)
                {
                    if (mesSommets[courant.X, courant.Y - 1] != null)
                    {
                        Sommet s = mesSommets[courant.X, courant.Y - 1];
                        if (s.Potentiel > z.Potentiel + 1)
                        {
                            s.Potentiel = z.Potentiel + 1;
                            s.Pred = courant;
                        }
                    }
                }

                //Droite
                if (courant.Y + 1 < Affichage.VY)
                {
                    if (mesSommets[courant.X, courant.Y + 1] != null)
                    {
                        Sommet s = mesSommets[courant.X, courant.Y + 1];
                        if (s.Potentiel > z.Potentiel + 1)
                        {
                            s.Potentiel = z.Potentiel + 1;
                            s.Pred = courant;
                        }
                    }
                }

                int min = Sommet.INFINI;

                for (int i = 0; i < Affichage.VX; i++)
                {
                    for (int j = 0; j < Affichage.VY; j++)
                    {
                        if (mesSommets[i, j] != null)
                        {
                            if (!mesSommets[i, j].Marque && mesSommets[i, j].Potentiel < min)
                            {
                                min = mesSommets[i, j].Potentiel;
                                courant = new Coord(i, j);
                            }
                        }
                    }
                }
            }
            Coord suivant = mesSommets[courant.X, courant.Y].Pred;

           if(dep.coord != arr.coord)
            {
                if (suivant.Y != dep.coord.Y)//Si leur coord sont différents...
                {
                    if (suivant.Y > dep.coord.Y)
                        suivant.Y = dep.coord.Y + 1; //Droite
                    else
                        suivant.Y = dep.coord.Y - 1;  //Gauche          
                }
                else
                {
                    if (suivant.X > dep.coord.X)
                        suivant.X = dep.coord.X + 1;  //Bas
                    else
                        suivant.X = dep.coord.X - 1;  //Haut
                }
            }
            return suivant; //Renvoie les nouvelles coordonnées du pacman
        }
    }
}
