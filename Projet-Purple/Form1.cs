using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projet_Purple
{
    public partial class Form1 : Form
    {
        //initialisations des variables
        bool goLeft, goRight, jumping, IsGameOver;
        bool platformstar_move = true;
        bool Hard = false;
        bool menu = false;
        bool once = true;
        int jumpSpeed;
        int gravity;
        int playerSpeed;
        int enemySpeed = 3;
        int flySpeed = 2;
        int platformSpeed = 2;
        int characterSkin;
        int score = 0;
        int RightCond = 770;
        int LeftCond = 586;
        double time;

        public Form1()
        {
            InitializeComponent();
        }

        private void Main_Game(object sender, EventArgs e)
        {
            // calcul du temps et affichage en temps réel
            time = time + 0.03;
            label4.Text = "Temps : " + Math.Round(time, 1).ToString() + " s";

            // initialisation du perso de base
            if (characterSkin == 0)
            {
                player.BackColor = Color.Transparent;
                playerSpeed = 15;
            }
            
            // permet de sauter selon la vitesse de saut
            player.Top += jumpSpeed;

            //conditions pour les déplacments
            if (goLeft == true)
            {
                player.Left -= playerSpeed;
            }
            if (goRight == true)
            {
                player.Left += playerSpeed;
            }

            if (jumping == true && gravity < 0)
            {
                jumping = false;
            }

            if (jumping == true)
            {
                jumpSpeed = -12;
                gravity -= 1;
            }
            else
            {
                jumpSpeed = 12;
            }

            // deéplacments des ennemis terrestres
            enemy2.Left += enemySpeed;
            if (enemy2.Left < 345)
            {
                enemy2.Image = Projet_Purple.Properties.Resources.rightslime;
            }

            if (enemy2.Left < platform2.Left || enemy2.Left + enemy2.Width > platform2.Left + platform2.Width)
            {
                enemy2.Image = Projet_Purple.Properties.Resources.leftslime;
                enemySpeed = -enemySpeed;
            }

            enemy1.Left -= enemySpeed;
            enemy1.Image = Projet_Purple.Properties.Resources.rightslime;
            if (enemy1.Left < platform2.Left || enemy1.Left + enemy1.Width > platform2.Left + platform2.Width)
            {
                enemySpeed = -enemySpeed;
            }
            // déplacments des ennemis volants
            fly1.Left += flySpeed;
            fly2.Left += flySpeed;
            if (fly1.Left == LeftCond || fly1.Right == RightCond)
            {

                flySpeed = -flySpeed;
            }

            if (fly1.Right > 1000)
            {
                LeftCond = 828;
            }
            if (once == false)
            {
                if (platformstar_move == true)
                {
                    platformstar.Top -= platformSpeed;
                }
                else if (platformstar_move == false)
                {
                    platformstar.Top += platformSpeed;
                }

                if (platformstar.Top < star.Bottom + 40)
                {
                    platformstar_move = false;
                }

                if (platformstar.Top > platform2.Top + 20)
                {
                    platformstar_move = true;
                }
            }

            if (player.Top > 523)
            {
                gametimer.Stop();
                IsGameOver = true;
                label1.Text = "Tu as perdu ! Appuie sur Entrer pour rejouer...";
            }

            foreach (Control x in this.Controls)
            {
                // gestions des collisions
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "platform")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds) && !jumping)
                        {

                            gravity = 8;
                            player.Top = x.Top - player.Height;
                            jumpSpeed = 0;
                        }

                        x.BringToFront();
                    }
                    if ((string)x.Tag == "enemy")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                        {
                            if (player.Bottom - 5 == x.Top && characterSkin == 1)
                            {
                                x.Visible = false;
                            }
                            else
                            {
                                gametimer.Stop();
                                IsGameOver = true;
                                label1.Text = "Tu as perdu ! Appuie sur Entrée pour rejouer...";
                            }
                        }
                    }
                    if ((string)x.Tag == "fruits")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                        {
                            x.Visible = false;
                            score += 10;
                            label3.Text = "Score : " + score.ToString();
                        }
                    }
                    if ((string)x.Tag == "star")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            gametimer.Stop();
                            IsGameOver = true;
                            label1.Text = "Tu as Gagné ! Appuie sur Entrée pour rejouer...";
                        }
                    }
                    if ((string)x.Tag == "fire")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            if (characterSkin == 2)
                            {
                                if (once == true)
                                {
                                    once = false;
                                    RightCond = 1026;
                                    flySpeed = 4;
                                }
                            }
                            else
                            {
                                gametimer.Stop();
                                IsGameOver = true;
                                label1.Text = "Tu as perdu ! Appuie sur Entrée pour rejouer...";

                            }
                        }
                    }
                }
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            // assignements des touches
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Q)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Up && !jumping || e.KeyCode == Keys.Z && !jumping)
            {
                jumping = true;
            }
            if (e.KeyCode == Keys.Space)
            {
                characterSkin += 1;

                if (characterSkin == 1)
                {
                    player.Image = Projet_Purple.Properties.Resources.player2;
                    playerSpeed = 10;
                    enemySpeed = enemySpeed / 3;
                }
                if (characterSkin == 2)
                {
                    player.Image = Projet_Purple.Properties.Resources.player3;
                    enemySpeed = 3;
                }
                if (characterSkin == 3)
                {
                    player.Image = Projet_Purple.Properties.Resources.player1;
                    characterSkin = 0;
                }
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            // Reset des actions de certaines touches
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Q)
            {
                goLeft = false;
                if (characterSkin == 3 ||  characterSkin == 0)
                {
                    player.Image = Projet_Purple.Properties.Resources.runleft1;
                }
                if (characterSkin == 1)
                {
                    player.Image = Projet_Purple.Properties.Resources.runleft;
                }
                if (characterSkin == 2)
                {
                    player.Image = Projet_Purple.Properties.Resources.runleft3;
                }
            }
            if (e.KeyCode == Keys.Tab && menu == false)
            {
                gametimer.Stop();
                label5.Visible = true;
                label5.Text = "Changer de mode de difficulté : C\r\nChanger de personnage : Space\r\nRevenir au menu : Tab\r\nDéplacement : Flèches directionnelles ou ZQSD";
                menu = true;
            }
            else if (e.KeyCode == Keys.Tab && menu == true)
            {
                gametimer.Start();
                label5.Visible = false;
                menu = false;
            }
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                goRight = false;
                if (characterSkin == 3 || characterSkin == 0)
                {
                    player.Image = Projet_Purple.Properties.Resources.runright1;
                }
                if (characterSkin == 1)
                {
                    player.Image = Projet_Purple.Properties.Resources.run;
                }
                if (characterSkin == 2)
                {
                    player.Image = Projet_Purple.Properties.Resources.runright3;
                }
            }
            if (jumping == true)
            {
                jumping = false;
            }
            if (e.KeyCode == Keys.Enter && IsGameOver == true || e.KeyCode == Keys.Z && IsGameOver == true)
            {
                label1.Text = "";
                ResetGame();
            }
            if (e.KeyCode == Keys.C && Hard == false)
            {
                enemySpeed = enemySpeed * 2;
                enemy1.Top = enemy1.Top - 40;
                platformSpeed = platformSpeed * 3;
                enemy1.Height = enemy1.Height * 2;
                Hard = true;
            }
            else if (e.KeyCode == Keys.C && Hard == true)
            {
                enemySpeed = enemySpeed / 2;
                enemy1.Top = enemy1.Top + 40;
                platformSpeed = platformSpeed / 3;
                enemy1.Height = enemy1.Height / 2;
                Hard = false;
            }
        }

        private void ResetGame()
        {
            // reset des valeurs pour rejouer
            jumping = false;
            goLeft = false;
            goRight = false;
            IsGameOver = false;
            once = true;
            menu = false;
            platformstar_move = true;
            characterSkin = 0;
            time = 0;
            score = 0;
            label3.Text = "Score : 0";
            label4.Text = "Temps :" + time.ToString() + "s";

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Visible == false)
                {
                    x.Visible = true;
                }
            }
            player.Left = 24;
            player.Top = 372;
            enemySpeed = 3;
            enemy1.Left = 579;
            enemy2.Left = 374;
            platformstar.Top = 445;
            fly1.Left = 604;
            fly2.Left = 604;
            fly1.Top = 228;
            fly2.Top = 318;
            flySpeed = 2;
            RightCond = 800;
            LeftCond = 586;

            gametimer.Start();
        }
    }
}
