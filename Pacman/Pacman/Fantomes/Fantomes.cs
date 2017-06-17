using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Pacman
{
    public class Fantomes : Affichage
    {
        static Stopwatch tpsPouvoir = new Stopwatch();  //temps pendant lequel les fantomes seront vulnérables
        ObjetAnime fantome;
        Boolean mangeable;
        Boolean mort;
        string directionPrecedente = "";

        public Fantomes(ObjetAnime fantome, Boolean mangeable, Boolean mort) : base()
        {
            this.fantome = fantome;
            this.mangeable = mangeable;
            this.mort = mort;
        }
        
        public void Afficher(ObjetAnime fantome, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int x = 0; x < Affichage.VX; x++)
            {
                for (int y = 0; y < Affichage.VY; y++)
                {
                    if (x == fantome.coord.X && y == fantome.coord.Y)
                    {
                        int xpos, ypos;
                        xpos = fantome.coord.X * 20;
                        ypos = fantome.coord.Y * 20;
                        Vector2 pos = new Vector2(ypos, xpos);
                        spriteBatch.Draw(fantome.Texture, pos, Color.White);
                    }
                }
            }
            spriteBatch.End();
        }

        /// <summary>
        /// Tous les fantomes, excepté le rouge, ont un déplacement aléatoire
        /// A chaque Carrefour, un tirage aléatoire est effectué pour choisir la direction vers laquelle le fantome se deplacera
        /// </summary>
        #region DeplacementAleatoire
       
        public void DeplacementAleatoire(ObjetAnime fantome, byte [,] map, SpriteBatch spriteBatch)
        {
            //Liste des mouvement possibles pour le fantome
            List<string> liberte = degreDeLiberte(fantome, map); 

            Random rand = new Random();
            int proba;

            string newDirection;
            //On tire aléatoirement un chiffre
            proba = rand.Next(0, liberte.Count);
            //Ce chiffre correspondra à une des positions présente dans la liste liberte   
            newDirection = liberte[proba];              

            //On refait le tirage tant que la direction choisie est la direction inverse du fantôme
            //ça lui évitera de faire sans cesse des aller-retour
            do
            {
                proba = rand.Next(0, liberte.Count);    
                newDirection = liberte[proba];      
            } while (newDirection == sensInverse(directionPrecedente));

            //On met à jour la direction precedente
            directionPrecedente = newDirection;                         

            //Ceci va permettre au fantome de sortir d'un coté de la map, et de rentrer par un autre coté
            if (fantome.coord == new Coord(14, 2))                     
            {
                fantome.coord = new Coord(14, 26);                           
                newDirection = "Left";                                       
                choixDeplacement(fantome, newDirection, map, spriteBatch);  
            }
            else if (fantome.coord == new Coord(14, 25))                
            {
                fantome.coord = new Coord(14, 2);
                choixDeplacement(fantome, newDirection, map, spriteBatch);
                Console.WriteLine("Sortie gauche");
            }

            choixDeplacement(fantome, newDirection, map, spriteBatch);
        }

        /// <summary>
        /// Cette méthode permettra de modifier les coordonnées du fantome selon la direction qui a été tirée dans la liste
        /// </summary>
        protected void choixDeplacement(ObjetAnime objet, string key, byte[,] map, SpriteBatch spriteBatch)
        {
            if (key == "Left")
            {
                 objet.coord.Y = objet.coord.Y - 1;
            }
            else if (key == "Right")
            {    
                 objet.coord.Y = objet.coord.Y + 1;
            }
            else if (key == "Up")
            {
                  objet.coord.X = objet.coord.X - 1;
            }
            else if (key == "Down")
            {
                   objet.coord.X = objet.coord.X + 1;
            }
        }

        /// <summary>
        /// Cette fonction évalue le nombre de direction possible pour la fantome
        /// Dégré de Liberté = direction où il n'y a pas de mur
        /// </summary>
        protected List<string> degreDeLiberte(ObjetAnime Fantome, byte[,] map)
        {
            List<string> degLiberte = new List<string>();          
            if (map[Fantome.coord.X + 1, Fantome.coord.Y] != 0)        
                degLiberte.Add("Down");
            if (map[Fantome.coord.X - 1, Fantome.coord.Y] != 0)
                degLiberte.Add("Up");
            if (map[Fantome.coord.X, Fantome.coord.Y + 1] != 0)
                degLiberte.Add("Right");
            if (map[Fantome.coord.X, Fantome.coord.Y - 1] != 0)
                degLiberte.Add("Left");

            return degLiberte;
        }


        /// <summary>
        /// Cette méthode permet d'avoir l'inverse de chaque direction
        /// </summary>
        protected string sensInverse(string direction)
        {
            string mvt;
            switch (direction)
            {
                case "Up":
                    mvt = "Down";
                    break;

                case "Down":
                    mvt = "Up";
                    break;

                case "Left":
                    mvt = "Right";
                    break;

                case "Right":
                    mvt = "Left";
                    break;
                default:
                    mvt = "";
                    break;

            }
            return mvt;
        }

        #endregion

        #region Fuite
        protected void DeplacementFuite(ObjetAnime fantome, ObjetAnime pacman, byte[,] map, List<String> liberte, SpriteBatch spriteBatch)
        {
           //Tout comme précedemment, cette fonction donne les déplcament possible du fantome
            Random rand = new Random();
            int proba;
            string mvt;

            proba = rand.Next(0, liberte.Count);               
            mvt = liberte[proba];                               
            choixDeplacement(fantome, mvt, map, spriteBatch);
        }

        /// <summary>
        /// Si un fantome est "vulnérable", et qu'il se trouve sur la même ligne ou la même colonne que le Pacman,
        /// il prendra le sens inverse de sa direction afin de s'échapper
        /// </summary>
        public void fuite(ObjetAnime fantome, ObjetAnime pacman, byte [,] map, SpriteBatch spriteBatch)
        {
            List<String> direction = new List<String>() ;
            
            if (fantome.coord == pacman.coord && getEtatMangeable == true)
            {
               getEtatMort = true;
            }
            if (fantome.coord.Y == pacman.coord.Y) //S'ils ont sur la même colonne
            {
                if (fantome.coord.X < pacman.coord.X)   //Et si le fantome est en haut du pacman
                {
                    if (map[fantome.coord.X - 1, fantome.coord.Y] != 0) //Il s'éloigne de lui tant qu'il n'y a pas de mur
                        fantome.coord.X -= 1;
                    else 
                    {
                        if (map[fantome.coord.X, fantome.coord.Y - 1] != 0)
                             direction.Add("Left");                     
                        if (map[fantome.coord.X, fantome.coord.Y + 1] != 0)
                             direction.Add("Right");
                        DeplacementFuite(fantome, pacman, map, direction, spriteBatch);//S'il y'a un mur, il va à gauche ou à droite
                        direction.Clear();
                    }
                }
                else //Et si le fantome est en bas du pacman
                {
                    if (map[fantome.coord.X + 1, fantome.coord.Y] != 0) 
                        fantome.coord.X += 1;
                    else 
                    {
                        if (map[fantome.coord.X, fantome.coord.Y - 1] != 0)
                            direction.Add("Left");
                        if (map[fantome.coord.X, fantome.coord.Y + 1] != 0)
                            direction.Add("Right");
                        DeplacementFuite(fantome, pacman, map, direction, spriteBatch);
                        direction.Clear();
                    }
                }
            }
            
            else if (fantome.coord.X == pacman.coord.X) //S'ils ont sur la même ligne
            {
                if (fantome.coord.Y < pacman.coord.Y)   //Et si le fantome est à gauche du pacman
                {
                    if (map[fantome.coord.X, fantome.coord.Y - 1] != 0) //Il s'éloigne de lui
                        fantome.coord.Y -= 1;
                    else
                    {
                        if (map[fantome.coord.X - 1, fantome.coord.Y] != 0)
                            direction.Add("Up");
                        if (map[fantome.coord.X + 1, fantome.coord.Y] != 0)
                            direction.Add("Down");
                        DeplacementFuite(fantome, pacman, map, direction, spriteBatch); //S'il y'a un mur; il va en haut ou en bas
                        direction.Clear();
                    }
                }
                else if (fantome.coord.Y > pacman.coord.Y)  //Et si le fantome est à droite du pacman
                {
                    if (map[fantome.coord.X, fantome.coord.Y + 1] != 0)
                        fantome.coord.Y += 1;
                    else 
                    {
                        if (map[fantome.coord.X - 1, fantome.coord.Y] != 0)
                            direction.Add("Up"); 
                        if (map[fantome.coord.X + 1, fantome.coord.Y] != 0)
                            direction.Add("Down");
                        DeplacementFuite(fantome, pacman, map, direction, spriteBatch);
                        direction.Clear();
                    }
                }
            }
            else
            {
                DeplacementAleatoire(fantome,  map, spriteBatch);
            }

        }

        /// <summary>
        /// Cette fonction sera appelé quand un fantôme aura été mangé par le pacman
        /// En effet, il devra rentrer dans sa "maison" pour pouvoir réapparaitre
        /// Pour optimiser son déplacement vers sa maison, j'ai utilisé l'algorithme de DIJSKTRA
        /// </summary>
        public  Coord retourMaison(Coord arr, ObjetAnime fantome, Texture2D texture, byte[,] map) //Cette méthode permet à l'"ame" du fantome (quand celui ci est mangé) de retourner dans sa maison et de réapparaitre
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

            mesSommets[arr.X, arr.Y].Potentiel = 0;
            Coord courant = arr;

            while (courant != fantome.coord)
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


            if (fantome.coord == arr) //Si le fantome arrive dans sa maison, ses états sont de nouveau par défaut
            {
                getEtatMort = false;
                Game1.mangeable = false;
                fantome.Texture = texture;
                return new Coord(14,12);
            }
            else if (fantome.coord != arr)
            {
                if (suivant.Y != fantome.coord.Y)
                {
                    if (suivant.Y > fantome.coord.Y)
                        suivant.Y = fantome.coord.Y + 1; //Droite
                    else
                        suivant.Y = fantome.coord.Y - 1;  //Gauche          
                }
                else
                {
                    if (suivant.X > fantome.coord.X)
                        suivant.X = fantome.coord.X + 1;  //Bas
                    else
                        suivant.X = fantome.coord.X - 1;  //Haut
                }
            }
            return suivant;
        }

        #endregion

       

        public Boolean getEtatMangeable
        {
            get
            {
                return mangeable;
            }
            set
            {
                mangeable = value;
            }
        }

        public Boolean getEtatMort
        {
            get
            {
                return mort;
            }
            set
            {
                mort = value;
            }
        }

    }
}
