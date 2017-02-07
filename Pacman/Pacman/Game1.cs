using System;
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
using static System.Net.Mime.MediaTypeNames;

namespace Pacman
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Variables Globales
        private Pacman _pacman;
        private FantomeRouge _fantomeRouge;
        private FantomeRose _fantomeRose;
        private FantomeOrange _fantomeOrange;
        private FantomeCyan _fantomeCyan;
        private Fantomes[] tabFantomes = new Fantomes[4];

        private Mur _mur;
        private Bean _bean;
        private BeanMagique _beanMagique;
        private SpriteFont policeScore;
        private SpriteFont policePlay;

        ObjetAnime mur;
        ObjetAnime bean;
        ObjetAnime pacman;
        ObjetAnime beanMagique;
        ObjetAnime fantomeCyan;
        ObjetAnime fantomeRouge;
        ObjetAnime fantomeOrange;
        ObjetAnime fantomeRose;
        ObjetAnime fantomeMangeable;
        ObjetAnime[] tabObjetFantomes = new ObjetAnime[4];

        //Etat des fantomes
        Boolean rougeMangeable = false;
        Boolean cyanMangeable = false;
        Boolean roseMangeable = false;
        Boolean orangeMangeable = false;

        Boolean rougeMort = false;
        Boolean cyanMort = false;
        Boolean roseMort = false;
        Boolean orangeMort = false;

        Stopwatch tpsPouvoir = new Stopwatch();

        public static byte[,] map;
        public static int viePacman = 3;
        int VX, VY;

        int compteurVitessePacman = 0;
        int compteurVitesseFantome = 0;
        public static Boolean mangeable;
        public static SoundEffect sonBean;
        public static Boolean play;

        public Game1() //Dessin de la Map
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            map = new byte[,]{
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
            {0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
            {0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
            {0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
            {0, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 0},
            {0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0},
            {0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0},
            {0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 2, 2, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 2, 2, 2, 2, 2, 2, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 2, 2, 2, 2, 2, 2, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
            {0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
            {0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
            {0, 3, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 3, 0},
            {0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0},
            {0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0},
            {0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0},
            {0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
            {0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
            {0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        };
            VX = map.GetLength(0); //Donne les lignes
            VY = map.GetLength(1); //Donne les colonnes

        } 

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Initialisation des objets du jeu
            _pacman = new Pacman(pacman, map);
            _fantomeRouge = new FantomeRouge(fantomeRouge, rougeMangeable, rougeMort);
            _fantomeRose = new FantomeRose(fantomeRose, roseMangeable, roseMort);
            _fantomeOrange = new FantomeOrange(fantomeOrange, orangeMangeable, orangeMort);
            _fantomeCyan = new FantomeCyan(fantomeCyan, cyanMangeable, cyanMort);

            tabFantomes[0] = _fantomeRouge;
            tabFantomes[1] = _fantomeCyan;
            tabFantomes[2] = _fantomeOrange;
            tabFantomes[3] = _fantomeRose;

            _mur = new Mur(mur);
            _bean = new Bean(bean);
            _beanMagique = new BeanMagique(beanMagique);
          
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //  changing the back buffer size changes the window size (when in windowed mode)
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 660;
            graphics.ApplyChanges();

            //Chargement des objets: Textures dans le dossier 'Content' du projet et coordonnées de départ     
            policeScore = Content.Load<SpriteFont>("PoliceScore");
            policePlay = Content.Load<SpriteFont>("policePlay");

            mur = new ObjetAnime(Content.Load<Texture2D>("./Map/mur"), new Vector2(0f, 0f), new Vector2(20f, 20f));
            bean = new ObjetAnime(Content.Load<Texture2D>("./Map/bean"), new Vector2(0f, 0f), new Vector2(20f, 20f));
            pacman = new ObjetAnime(Content.Load<Texture2D>("./Pacman/pacman"), new Vector2(0f, 0f), new Vector2(20f, 20f), new Coord(23,13));
            beanMagique = new ObjetAnime(Content.Load<Texture2D>("./Map/beanMagique"), new Vector2(0f, 0f), new Vector2(20f, 20f));
            fantomeRose = new ObjetAnime(Content.Load<Texture2D>("./Fantomes/fantome_Rose"), new Vector2(0f, 0f), new Vector2(20f, 20f),new Coord(14,15));
            fantomeCyan = new ObjetAnime(Content.Load<Texture2D>("./Fantomes/fantome_Cyan"), new Vector2(0f, 0f), new Vector2(20f, 20f), new Coord(14,14));
            fantomeRouge = new ObjetAnime(Content.Load<Texture2D>("./Fantomes/fantome_Rouge"), new Vector2(0f, 0f), new Vector2(20f, 20f), new Coord(14,13));
            fantomeOrange = new ObjetAnime(Content.Load<Texture2D>("./Fantomes/fantome_Orange"), new Vector2(0f, 0f), new Vector2(20f, 20f), new Coord(14,12));
            fantomeMangeable = new ObjetAnime(Content.Load<Texture2D>("./Fantomes/fan_mangeable"), new Vector2(0f, 0f), new Vector2(20f, 20f));

            tabObjetFantomes[0] = fantomeRouge;
            tabObjetFantomes[1] = fantomeCyan;
            tabObjetFantomes[2] = fantomeOrange;
            tabObjetFantomes[3] = fantomeRose;

            sonBean = Content.Load<SoundEffect>("Invincible");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            
                // Allows the game to exit
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();

            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Enter)) //Si le joueur appuies sur ENTRER, le jeu se lance
                play = true;

            if (play)
            {
                //La fonction Update met à jour régulièrement le jeu, donc à partir d'un compteur on peut gérer les vitesses de déplacement

                //Vitesse et déplacement du Pacman
                if (compteurVitessePacman > 5)
                {
                    _pacman.Deplacement(gameTime, Content, spriteBatch); //Chaque fois que le compteur sera à 5, le pacman bougera d'un "case"
                    compteurVitessePacman = 0; //On remet le compteur à zéro
                }
                compteurVitessePacman++;

                //Pacman meurt
                for (int i = 0; i < tabObjetFantomes.Length; i++)
                {
                    if (tabObjetFantomes[i].coord == pacman.coord && !mangeable)
                    {
                        LoadContent(); //Reinitialisation des positions des objets
                        play = false;
                        viePacman--;
                    }
                }

                //Vitesse et déplacement des fantomes
                if (compteurVitesseFantome > 6)
                {
                    compteurVitesseFantome = 0;
                   
                    //Rouge
                    if (_fantomeRouge.getEtatMangeable == false && _fantomeRouge.getEtatMort == false) //Si le fantome est mangeable et s'il est vivant...
                        fantomeRouge.coord = _fantomeRouge.Dijkstra(pacman, fantomeRouge, map, new Coord(14, 13));//Sa position est reinitialisé grâce à Dijkstra
                    fuiteFantome(_fantomeRouge, fantomeRouge, Content.Load<Texture2D>("./Fantomes/fantome_rouge"));//Sinon, s'il est mangeable, le fantome fuit le pacman
                
                    //Orange
                    if (_fantomeOrange.getEtatMangeable == false && _fantomeOrange.getEtatMort == false)
                         _fantomeOrange.DeplacementAleatoire(fantomeOrange,  map, spriteBatch);
                    fuiteFantome(_fantomeOrange, fantomeOrange, Content.Load<Texture2D>("./Fantomes/fantome_orange"));

                    //Cyan
                    if (_fantomeCyan.getEtatMangeable == false && _fantomeCyan.getEtatMort == false)
                        _fantomeCyan.DeplacementAleatoire(fantomeCyan,  map, spriteBatch);
                    fuiteFantome(_fantomeCyan, fantomeCyan, Content.Load<Texture2D>("./Fantomes/fantome_cyan"));

                    //Rose
                    if (_fantomeRose.getEtatMangeable == false && _fantomeRose.getEtatMort == false)
                         _fantomeRose.DeplacementAleatoire(fantomeRose,  map, spriteBatch);
                    fuiteFantome(_fantomeRose, fantomeRose, Content.Load<Texture2D>("./Fantomes/fantome_rose"));
                        
                }
                compteurVitesseFantome++;

                //Pouvoir
                MangerPouvoir(pacman.coord.X, pacman.coord.Y);        //On vérifie à chaqu'instant si le pacman à mangé un pouvoir, si c'est le cas
                                                                      //Le compteur tpsPouvoir est déclenché
                if (tpsPouvoir.ElapsedMilliseconds > 6000)            //6000 = temps pendant lequel les fantomes seront vulnérables
                {
                    RegenFantome();                                   //Les fantomes retrouvent leurs textures. Ils vont de nouveau chasser le pacman
                    tpsPouvoir.Reset();                               //On remet le compteur à zero

                    for (int j = 0; j < tabFantomes.Length; j++)
                        tabFantomes[j].getEtatMangeable = false;      //On remet leur état par défaut

                    mangeable = false;  
                }


                //Pause
                if (keyboard.IsKeyDown(Keys.P))         //Si le joueur appuie sue P, le jeu se met en pause
                    play = false;

                base.Update(gameTime);
            }

            if(viePacman == 0)
            {
                KeyboardState keyboard2 = Keyboard.GetState();
                if (keyboard2.IsKeyDown(Keys.Enter)) 
                    Exit();     //Permet de fermer l'application
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {   //Draw permet de "dessiner" les objets qu'on utilisera
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Importe la police d'écriture que j'ai crée (Dossier Content)
            spriteBatch.Begin();
            spriteBatch.DrawString(policeScore, "SCORE: ", new Vector2(600, 100), Color.White);
            spriteBatch.DrawString(policeScore, "VIES: ", new Vector2(600, 150), Color.White);
            spriteBatch.DrawString(policeScore, Pacman.score.ToString(), new Vector2(800, 100), Color.White);
            spriteBatch.DrawString(policePlay, "ENTRER: Lancer le jeu ! ", new Vector2(600, 250), Color.White);
            spriteBatch.DrawString(policePlay, "P: Mettre en Pause ! ", new Vector2(600, 275), Color.White);
            switch (viePacman) //Selon le nombre de vie qu'il reste , on affiche plus ou moins de pacman sur l'écran de droite
            {
                case 1:
                    spriteBatch.Draw(Content.Load<Texture2D>("./Pacman/viePacman"), new Vector2(750, 165), Color.White);
                    break;
                case 2:
                    spriteBatch.Draw(Content.Load<Texture2D>("./Pacman/viePacman"), new Vector2(750, 165), Color.White);
                    spriteBatch.Draw(Content.Load<Texture2D>("./Pacman/viePacman"), new Vector2(800, 165), Color.White);
                    break;
                case 3:
                    spriteBatch.Draw(Content.Load<Texture2D>("./Pacman/viePacman"), new Vector2(750, 165), Color.White);
                    spriteBatch.Draw(Content.Load<Texture2D>("./Pacman/viePacman"), new Vector2(800, 165), Color.White);
                    spriteBatch.Draw(Content.Load<Texture2D>("./Pacman/viePacman"), new Vector2(850, 165), Color.White); break;
                case 0:
                    spriteBatch.DrawString(policeScore, "GAME OVER", new Vector2(600, 350), Color.Red);
                    spriteBatch.DrawString(policePlay, "Appuyer sur ENTRER pour QUITTER", new Vector2(600, 400), Color.Red);
                    break;
            }
            spriteBatch.End();
            
            //Mise à jour de la map
            _bean.AfficherMap(bean, spriteBatch, 1, map);
            _mur.AfficherMap(mur, spriteBatch,0, map);
            _beanMagique.AfficherMap(beanMagique, spriteBatch, 3, map);
            _pacman.Afficher(pacman, spriteBatch);

            //Affichage des fantomes
            for (int i = 0; i < tabFantomes.Length; i++)
                tabFantomes[i].Afficher(tabObjetFantomes[i], spriteBatch);

            base.Draw(gameTime);

        }       

        protected void MangerPouvoir(int a, int b) 
        {
            for (int x = 0; x < VX; x++)
            {
                for (int y = 0; y < VY; y++)
                {
                    if (map[x, y] == 3 && (x == a && y == b))
                    {
                        map[x, y] = 2;
                        
                        //Modification des textures des fantomes
                        for (int j = 0; j < tabObjetFantomes.Length; j++)
                            tabObjetFantomes[j].Texture = fantomeMangeable.Texture;

                        //Changement de leur état
                        for (int j = 0; j < tabFantomes.Length; j++)
                            tabFantomes[j].getEtatMangeable = true;

                        tpsPouvoir.Start(); //Lanchement du timer
                    }
                }
            }
        }

        protected void RegenFantome() //Les fantomes retrouvent leurs textures d'origine
        {
             fantomeCyan.Texture = Content.Load<Texture2D>("./Fantomes/fantome_cyan");
             fantomeOrange.Texture = Content.Load<Texture2D>("./Fantomes/fantome_orange");
             fantomeRose.Texture = Content.Load<Texture2D>("./Fantomes/fantome_rose");
             fantomeRouge.Texture = Content.Load<Texture2D>("./Fantomes/fantome_rouge");             
        }

        protected void MangerFantome(ObjetAnime pacman, ObjetAnime fantome) //Changement de la texture des fantomes quand ils sont "mangeable"
        {
            if (mangeable)
            {
                if(pacman.coord == fantome.coord)
                {
                    fantome.Texture = Content.Load<Texture2D>("./Fantomes/FantomePeur1");
                }
            }
        }

        protected void fuiteFantome(Fantomes _fantome, ObjetAnime fantome, Texture2D texture)
        {
            //Si les fantomes sont "mangeables" et vivant...
            if (_fantome.getEtatMangeable == true && _fantome.getEtatMort == false)
            {
                _fantome.Fuite(fantome, pacman, map, spriteBatch); //...ils fuient le pacman
                mangeable = true;
            }
            //S'ils sont mort
            if (_fantome.getEtatMort == true)
            {
                fantome.Texture = Content.Load<Texture2D>("./Fantomes/fantome_mort");            //ils changent de textures
                                                                                                 //Ils ne sont plus mangeables
                fantome.coord = _fantome.retourMaison(new Coord(14, 12), fantome, texture, map); //Ils rentrent dans leur maison pour mettre leur texture par defaut et chasser le pacman
            }
            
        }
    }

}
