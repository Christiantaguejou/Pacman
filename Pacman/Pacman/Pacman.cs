﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using System.Timers;
using System.Media;

namespace Pacman
{
    class Pacman : Affichage
    {
        public static int score = 0;
        public ObjetAnime pacman;
        byte[,] map;
        string direction;
        
        public Pacman ( ObjetAnime pacman, byte[,] map) : base() {
            this.pacman = pacman;
            this.map = map;
          //  this.direction = direction;
        }

        public  void Afficher(ObjetAnime pacman, SpriteBatch spriteBatch)
        {
            this.pacman = pacman;
            spriteBatch.Begin();
            for (int x = 0; x < Affichage.VX; x++)
            {
                for (int y = 0; y < Affichage.VY; y++)
                {
                    if (x == pacman.coord.X && y == pacman.coord.Y)
                    {
                        int xpos, ypos;
                        xpos = pacman.coord.X * 20;
                        ypos = pacman.coord.Y * 20;
                        Vector2 pos = new Vector2(ypos, xpos);
                        spriteBatch.Draw(pacman.Texture, pos, Color.White);
                    }
                }
            }
            spriteBatch.End();
        }
        
        public void Deplacement(GameTime gameTime, ContentManager Content, SpriteBatch spriteBatch)
        {
            //Selon la touche appuyé, les coordonnée du pacman sont modifiés, et son image change
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Right))
            {
                //Si la direction choisie est un mur, la méthode ContrainteDeplacement ne traite pas ce déplacement
                //Ainsi, le pacman continue d'avancer tout droit
                if (map[pacman.coord.X, pacman.coord.Y + 1] == 0)   
                {                                                   
                    ContrainteDeplacement(direction, spriteBatch);  
                }
                else
                {
                    direction = "Right";
                    ContrainteDeplacement(direction, spriteBatch);
                    pacman.Texture = Content.Load<Texture2D>("./Pacman/pacman");
                }
            }
            else if (keyboard.IsKeyDown(Keys.Left))
            {
                if (map[pacman.coord.X, pacman.coord.Y - 1] == 0)
                {
                    ContrainteDeplacement(direction, spriteBatch);
                }
                else
                {
                    direction = "Left";
                    ContrainteDeplacement(direction, spriteBatch);
                    pacman.Texture = Content.Load<Texture2D>("./Pacman/pacmanGauche");
                }
            }
            else if (keyboard.IsKeyDown(Keys.Up))
            {
                if (map[pacman.coord.X - 1, pacman.coord.Y] == 0)
                {
                    ContrainteDeplacement(direction, spriteBatch);
                }
                else
                {
                    direction = "Up";
                    ContrainteDeplacement(direction, spriteBatch);
                    pacman.Texture = Content.Load<Texture2D>("./Pacman/PacmanHaut");
                }
            }
            else if (keyboard.IsKeyDown(Keys.Down))
            {
                if (map[pacman.coord.X + 1, pacman.coord.Y] == 0)
                {
                    ContrainteDeplacement(direction, spriteBatch);
                }
                else
                {
                    direction = "Down";
                    ContrainteDeplacement(direction, spriteBatch);
                    pacman.Texture = Content.Load<Texture2D>("./Pacman/PacmanBas");
                }
            }
            else
            {
                ContrainteDeplacement(direction, spriteBatch);
            }
        }

        /// <summary>
        /// Permet d'éviter que le pacman se dirige vers un mur
        /// Il permet aussi au pacman de sortir d'un coté de la map pour entrer de l'autre coté
        /// </summary>
        public void ContrainteDeplacement(string key, SpriteBatch spriteBatch)
        {
            //Permet de sortir des 2 cotés de la map
            if (key == "Left" && pacman.coord == new Coord(14,1)) //Si le joueur sort par la gauche
            {
                pacman.coord = new Coord(14, 27);               //Il rentre par la droite
                ContrainteDeplacement("Left", spriteBatch);
            }
            else if (pacman.coord == new Coord(14,26) && key == "Right")
            {
                pacman.coord = new Coord(14, 1);
                ContrainteDeplacement("Right", spriteBatch);
            }


            //Modification des coordonnées du Pacman
            else if (map[pacman.coord.X, pacman.coord.Y - 1] != 0 && key == "Left") //Si le pacman veut tourner à dauche et qu'il n'y a pas de mur...
            {
                pacman.coord.Y = pacman.coord.Y - 1;                                //On modifie ses coordonnées
                Mangerbean(pacman.coord.X, pacman.coord.Y, spriteBatch);            //Et il mange les beans
            }
            else if (map[pacman.coord.X, pacman.coord.Y + 1] != 0 && key == "Right")
            {
                pacman.coord.Y = pacman.coord.Y + 1;
                Mangerbean(pacman.coord.X, pacman.coord.Y, spriteBatch);
            }
            else if (map[pacman.coord.X - 1, pacman.coord.Y] != 0 && key == "Up")
            {
                pacman.coord.X = pacman.coord.X - 1;
                Mangerbean(pacman.coord.X, pacman.coord.Y, spriteBatch);
            }
            else if (map[pacman.coord.X + 1, pacman.coord.Y] != 0 && key == "Down")
            {
                pacman.coord.X = pacman.coord.X + 1;
                Mangerbean(pacman.coord.X, pacman.coord.Y, spriteBatch);
            }
        }

        /// <summary>
        /// Lorsque le pacman se trouve sur la même case qu'un bean, il le mange
        /// </summary>
        public void Mangerbean(int a, int b, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int x = 0; x < Affichage.VX; x++)
            {
                for (int y = 0; y < Affichage.VY; y++)
                {
                    if (map[x, y] == 1 && (x == a && y == b))
                    {
                        map[x, y] = 2; 
                        score += 10;   
                      //  Game1.sonBean.Play(); //Quand pacman mange un haricot, il emet un son
                    }
                }
            }
            spriteBatch.End();

        }
    }
}
