using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class Form1 : Form
    {
        
        bool goleft = false;
        bool goright = false;
        bool jumping = false;
        bool hasKey = false;

        int jumpSpeed = 10;
        int force = 8;
        int score = 0;

        int playSpeed = 18;
        int backLeft = 8;

        public Form1()
        {
            InitializeComponent();
        }

        private void UpdateScore()
        {
            txtScore.Text = "Score: " + score;
        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            player.Top += jumpSpeed;
            // refresh the player picture box consistently
            player.Refresh();
            // if jumping is true and force is less than 0
            // then change jumping to false
            if (jumping && force < 0)
            {
                jumping = false;
            }
            // if jumping is true
            // then change jump speed to -12 
            // reduce force by 1
            if (jumping)
            {
                jumpSpeed = -12;
                force -= 1;
            }
            else
            {
                // else change the jump speed to 12
                jumpSpeed = 12;
            }
            // if go left is true and players left is greater than 100 pixels
            // only then move player towards left of the 
            if (goleft && player.Left > 100)
            {
                player.Left -= playSpeed;
            }
            // by doing the if statement above, the player picture will stop on the forms left
            // if go right Boolean is true
            // player left plus players width plus 100 is less than the forms width
            // then we move the player towards the right by adding to the players left
            if (goright && player.Left + (player.Width + 100) < this.ClientSize.Width)
            {
                player.Left += playSpeed;
            }
            // by doing the if statement above, the player picture will stop on the forms right
            // if go right is true and the background picture left is greater 1352
            // then we move the background picture towards the left
            if (goright && background.Left > -1353)
            {
                background.Left -= backLeft;
                // the for loop below is checking to see the platforms and coins in the level
                // when they are found it will move them towards the left
                foreach (Control x in this.Controls)
                {
                    if (x is PictureBox && (x.Tag == "platform" || x.Tag == "coin" || x.Tag == "door" || x.Tag == "key"))
                    {
                        x.Left -= backLeft;
                    }
                }
            }
            // if go left is true and the background pictures left is less than 2
            // then we move the background picture towards the right
            if (goleft && background.Left < 2)
            {
                background.Left += backLeft;
                // below the is the for loop thats checking to see the platforms and coins in the level
                // when they are found in the level it will move them all towards the right with the background
                foreach (Control x in this.Controls)
                {
                    if (x is PictureBox && (x.Tag == "platform" || x.Tag == "coin" || x.Tag == "door" || x.Tag == "key"))
                    {
                        x.Left += backLeft;
                    }
                }
            }
            // below if the for loop thats checking for all of the controls in this form
            foreach (Control x in this.Controls)
            {
                // is X is a picture box and it has a tag of platform
                if (x is PictureBox && x.Tag == "platform")
                {
                    // then we are checking if the player is colliding with the platform
                    // and jumping is set to false
                    if (player.Bounds.IntersectsWith(x.Bounds) && !jumping)
                    {
                        // then we do the following
                        force = 8; // set the force to 8
                        player.Top = x.Top - player.Height; // also we place the player on top of the picture box
                        jumpSpeed = 0; // set the jump speed to 0
                    }
                }
                // if the picture box found has a tag of coin
                if (x is PictureBox && x.Tag == "coin")
                {
                    // now if the player collides with the coin picture box
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x); // then we are going to remove the coin image
                        score++; // add 1 to the score
                        UpdateScore(); // update the score on the screen
                    }
                }
            }
            // if the player collides with the door and has key boolean is true
            if (player.Bounds.IntersectsWith(door.Bounds) && hasKey)
            {
                // then we change the image of the door to open
                door.Image = Properties.Resources.greenDoor;
                // and we stop the timer
                gameTimer.Stop();
                MessageBox.Show("You Completed the level!!"); // show the message box
            }
            // if the player collides with the key picture box
            if (player.Bounds.IntersectsWith(key.Bounds))
            {
                // then we remove the key from the game
                this.Controls.Remove(key);
                // change the has key boolean to true
                hasKey = true;
            }
            // this is where the player dies
            // if the player goes below the forms height then we will end the game
            if (player.Top + player.Height > this.ClientSize.Height + 60)
            {
                gameTimer.Stop(); // stop the timer
                MessageBox.Show("You Died!!!"); // show the message box
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = true;
            }
            if (e.KeyCode == Keys.Space && !jumping)
            {
                jumping = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = false;
            }
            if (jumping)
            {
                jumping = false;
            }
        }

        private void CloseGame(object sender, FormClosedEventArgs e)
        {

        }
    }
}

