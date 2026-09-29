using System.Diagnostics.Eventing.Reader;
using System.Resources;

namespace fpdungeonCrawler
{
    public partial class Form1 : Form
    {

        TableLayoutPanelCellPosition[] PlayerView;

        TableLayoutPanel tlpMinimap = new();

        Map map = new();
        Player player = new Player();
        //public enum PlayerOrientation { Left, Up, Right, Down }
        //PlayerOrientation player.orientation = PlayerOrientation.Up;


        bool[] Layer1 = new bool[3];
        bool[] Layer2 = new bool[3];
        bool[] Layer3 = new bool[3];




        public Form1()
        {
            InitializeComponent();


            PlayerView = new TableLayoutPanelCellPosition[9];
            map.GenerateMap();
            player = GeneratePlayer();
            map.GenerateMinimapUI();

            GetPlayerView();

            this.Controls.Add(map.minimap);
            this.Controls.SetChildIndex(map.minimap, 0);
            map.UpdateMinimapUI();

            this.KeyDown += Playermovements;




        }

        public Player GeneratePlayer()
        {
            Player player = new Player();
            Tile tile = new Tile();
            tile.TilePositionX = player.StartPosition.Column;
            tile.TilePositionY = player.StartPosition.Row;
            tile.entity = player;
            map.mapTiles[player.StartPosition.Column, player.StartPosition.Row] = tile;
            return player;
        }

        public void UpdatePlayerposition()
        {
            foreach (Tile tile in map.mapTiles)
            {
                if (tile.entity == player)
                {
                    tile.entity = null;
                }
                map.mapTiles[player.CurrentPosition.Column, player.CurrentPosition.Row].entity = player;
            }
        }


        private void Playermovements(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                switch (player.orientation)
                {
                    case Player.PlayerOrientation.Up:
                        player.CurrentPosition = (player.CurrentPosition.Column, player.CurrentPosition.Row - 1);
                        break;

                    case Player.PlayerOrientation.Down:
                        player.CurrentPosition = (player.CurrentPosition.Column, player.CurrentPosition.Row + 1);
                        break;
                }
            }


            if (e.KeyCode == Keys.S)
            {
                player.CurrentPosition = (player.CurrentPosition.Column, player.CurrentPosition.Row + 1);
            }

            if (e.KeyCode == Keys.D)
            {
                if ((int)player.orientation < 3)
                {
                    player.orientation++;
                }
                else
                {
                    player.orientation = Player.PlayerOrientation.Left;
                }
            }


            if (e.KeyCode == Keys.A)
            {
                if ((int)player.orientation > 0)
                {
                    player.orientation--;
                }
                else
                {
                    player.orientation = Player.PlayerOrientation.Down;
                }
            }
            UpdatePlayerposition();
            map.UpdateMinimapUI();
            //DrawEnvironment();
        }


        private void GetPlayerView()
        {
            if (player.orientation == Player.PlayerOrientation.Up)
            {
                int k = 0;
                for (int i = player.CurrentPosition.Row - 2; i <= player.CurrentPosition.Row; i++)
                {
                    for (int j = player.CurrentPosition.Column - 1; j <= player.CurrentPosition.Column + 1; j++)
                    {
                        player.View[k] = map.mapTiles[j, i];
                        k += 1;
                    }
                }
            }

            else if (player.orientation == Player.PlayerOrientation.Down)
            {
                int k = 0;
                for (int i = player.CurrentPosition.Row + 2; i >= player.CurrentPosition.Row; i--)
                {
                    for (int j = player.CurrentPosition.Column + 1; j >= player.CurrentPosition.Column - 1; j--)
                    {
                        PlayerView[k] = new TableLayoutPanelCellPosition(i, j);
                        k += 1;
                    }
                }
            }

            else if (player.orientation == Player.PlayerOrientation.Left)
            {
                int k = 0;
                for (int i = player.CurrentPosition.Column - 2; i <= player.CurrentPosition.Column; i++)
                {
                    for (int j = player.CurrentPosition.Row +1; j >= player.CurrentPosition.Row -1; j--)
                    {
                        PlayerView[k] = new TableLayoutPanelCellPosition(i, j);
                        k += 1;
                    }

                }
            }


            else if (player.orientation == Player.PlayerOrientation.Right)
            {
                int k = 0;
                for (int i = player.CurrentPosition.Column + 2; i >= player.CurrentPosition.Column; i--)
                {
                    for (int j = player.CurrentPosition.Row - 1; j <= player.CurrentPosition.Row + 1; j++)
                    {
                        PlayerView[k] = new TableLayoutPanelCellPosition(i, j);
                        k += 1;
                    }

                }
            }
        }


        private void CheckLayer1()
        {
            GetPlayerView();
            for (int i = 0; i < 3; i++)
            {
                if (PlayerView[i].Column < 0 || PlayerView[i].Row < 0)
                {
                    Layer1[i] = true;
                }
                else
                {
                    Control control = tlpMinimap.GetControlFromPosition(PlayerView[i].Column, PlayerView[i].Row);
                    if (control.Tag == "Wall")
                    {
                        Layer1[i] = true;
                    }
                    else
                    {
                        Layer1[i] = false;
                    }
                }

            }
            if (Layer1[0] == true && Layer1[1] == false && Layer1[2] == false)
            {
                pbLayer1.BackgroundImage = Properties.Resources.Layer1_DoorRightPic;
            }
            else if (Layer1[0] == true && Layer1[1] == false && Layer1[2] == true)
            {
                pbLayer1.BackgroundImage = Properties.Resources.Layer1_HallPic;
            }
            else if (Layer1[0] == false && Layer1[1] == false && Layer1[2] == true)
            {
                pbLayer1.BackgroundImage = Properties.Resources.Layer1_DoorLeftPic;
            }
            else if (Layer1[0] == false && Layer1[1] == false && Layer1[2] == false)
            {
                pbLayer1.BackgroundImage = Properties.Resources.Layer1_DoorRightaLeftPic;
            }
            else if (Layer1[1] == true)
            {
                pbLayer1.BackgroundImage = Properties.Resources.Layer1_WallPic1;
            }
        }

        private void CheckLayer2()
        {
            GetPlayerView();
            for (int i = 3; i < 6; i++)
            {
                Control control = tlpMinimap.GetControlFromPosition(PlayerView[i].Column, PlayerView[i].Row);
                if (control.Tag == "Wall")
                {
                    Layer2[i - 3] = true;
                }
                else
                {
                    Layer2[i - 3] = false;
                }
            }
            if (Layer2[0] == true && Layer2[1] == false && Layer2[2] == false)
            {
                pbLayer2.BackgroundImage = Properties.Resources.Layer2_DoorRightPic;
            }
            else if (Layer2[0] == true && Layer2[1] == false && Layer2[2] == true)
            {
                pbLayer2.BackgroundImage = Properties.Resources.Layer2_HallPic;
            }
            else if (Layer2[0] == false && Layer2[1] == false && Layer2[2] == true)
            {
                pbLayer2.BackgroundImage = Properties.Resources.Layer2_DoorLeftPic;
            }
            else if (Layer2[0] == false && Layer2[1] == false && Layer2[2] == false)
            {
                pbLayer2.BackgroundImage = Properties.Resources.Layer2_DoorRightaLeftPic;
            }
            else if (Layer2[1] == true)
            {
                pbLayer2.BackgroundImage = Properties.Resources.Layer2_Wall2;
            }
        }

        private void CheckLayer3()
        {
            GetPlayerView();
            for (int i = 6; i < 9; i++)
            {
                Control control = tlpMinimap.GetControlFromPosition(PlayerView[i].Column, PlayerView[i].Row);
                if (control.Tag == "Wall")
                {
                    Layer3[i - 6] = true;
                }
                else
                {
                    Layer3[i - 6] = false;
                }
            }
            if (Layer3[0] == true && Layer3[1] == false && Layer3[2] == true)
            {
                pbLayer3.BackgroundImage = Properties.Resources.Layer3_Hall;
            }
            else if (Layer3[0] == true && Layer3[1] == false && Layer3[2] == false)
            {
                pbLayer3.BackgroundImage = Properties.Resources.Layer3_DoorRightPic;
            }
            else if (Layer3[0] == false && Layer3[1] == false && Layer3[2] == true)
            {
                pbLayer3.BackgroundImage = Properties.Resources.Layer3_DoorLeftPic;
            }
            else if (Layer3[0] == false && Layer3[1] == false && Layer3[2] == false)
            {
                pbLayer3.BackgroundImage = Properties.Resources.Layer3_DoorLeftaRightPic;
            }
        }


        private void DrawEnvironment()
        {
            CheckLayer1();
            CheckLayer2();
            CheckLayer3();
        }



        private void EnvironmentTest()
        {
            for (int i = 0; i < PlayerView.Length; i++)
            {
                Control control = tlpMinimap.GetControlFromPosition(PlayerView[i].Row, PlayerView[i].Column);
                MessageBox.Show("Das hier ist" + control.Name);
            }
        }
    }
}
