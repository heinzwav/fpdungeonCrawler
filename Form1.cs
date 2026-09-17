using System.Diagnostics.Eventing.Reader;
using System.Resources;

namespace fpdungeonCrawler
{
    public partial class Form1 : Form
    {
        PictureBox pbPlayer = new();

        TableLayoutPanelCellPosition[] PlayerView;
        TableLayoutPanelCellPosition currentPlayerposition;
        TableLayoutPanel tlpMinimap = new();

        Map map = new();
        Player player = new Player();
        public enum PlayerOrientation { Left, Up, Right, Down }
        PlayerOrientation pOrientation = PlayerOrientation.Up;


        bool[] Layer1 = new bool[3];
        bool[] Layer2 = new bool[3];
        bool[] Layer3 = new bool[3];




        public Form1()
        {
            InitializeComponent();

            player = map.player;
            tlpMinimap = map.Minimap;
            pbPlayer = map.player.pbPlayer;

            PlayerView = new TableLayoutPanelCellPosition[9];
            map.GenerateMap();
            map.InitializePlayer();
            GetPlayerView();
            DrawEnvironment();
            this.Controls.Add(map.Minimap);
            this.Controls.SetChildIndex(map.Minimap, 0);

            this.KeyDown += Playermovements;




        }


        private void Playermovements(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            currentPlayerposition = tlpMinimap.GetCellPosition(pbPlayer);

            if (e.KeyCode == Keys.W)
            {
                pOrientation = PlayerOrientation.Up;
                TableLayoutPanelCellPosition newPos = new TableLayoutPanelCellPosition(currentPlayerposition.Column, currentPlayerposition.Row - 1);
                Control nextControl = tlpMinimap.GetControlFromPosition(newPos.Column, newPos.Row);
                nextControl.Dispose();
                tlpMinimap.SetCellPosition(pbPlayer, newPos);
                map.GenerateEmptyTile(currentPlayerposition.Column, currentPlayerposition.Row);
            }


            if (e.KeyCode == Keys.S)
            {
                //pOrientation = PlayerOrientation.Down;
                TableLayoutPanelCellPosition newPos = new TableLayoutPanelCellPosition(currentPlayerposition.Column, currentPlayerposition.Row + 1);
                Control nextControl = tlpMinimap.GetControlFromPosition(newPos.Column, newPos.Row);
                nextControl.Dispose();
                tlpMinimap.SetCellPosition(pbPlayer, newPos);
                map.GenerateEmptyTile(currentPlayerposition.Column, currentPlayerposition.Row);
            }

            if (e.KeyCode == Keys.D)
            {
                if ((int)pOrientation < 3)
                {
                    pOrientation++;
                }
                else
                {
                    pOrientation = PlayerOrientation.Left;
                }
            }


            if (e.KeyCode == Keys.A)
            {
                if ((int)pOrientation > 0)
                {
                    pOrientation--;
                }
                else
                {
                    pOrientation = PlayerOrientation.Down;
                }
            }

            DrawEnvironment();
        }


        private void GetPlayerView()
        {
            if (pOrientation == PlayerOrientation.Up)
            {
                currentPlayerposition = tlpMinimap.GetCellPosition(pbPlayer);
                int k = 0;
                for (int i = currentPlayerposition.Row - 2; i <= currentPlayerposition.Row; i++)
                {
                    for (int j = currentPlayerposition.Column - 1; j <= currentPlayerposition.Column + 1; j++)
                    {
                        PlayerView[k] = new TableLayoutPanelCellPosition(j, i);
                        k += 1;
                    }
                }
            }

            else if (pOrientation == PlayerOrientation.Down)
            {
                currentPlayerposition = tlpMinimap.GetCellPosition(pbPlayer);
                int k = 0;
                for (int i = currentPlayerposition.Row + 2; i >= currentPlayerposition.Row; i--)
                {
                    for (int j = currentPlayerposition.Column + 1; j >= currentPlayerposition.Column - 1; j--)
                    {
                        PlayerView[k] = new TableLayoutPanelCellPosition(i, j);
                        k += 1;
                    }
                }
            }

            else if (pOrientation == PlayerOrientation.Left)
            {
                currentPlayerposition = tlpMinimap.GetCellPosition(pbPlayer);
                int k = 0;
                for (int i = currentPlayerposition.Column - 2; i <= currentPlayerposition.Column; i++)
                {
                    for (int j = currentPlayerposition.Row +1; j >= currentPlayerposition.Row -1; j--)
                    {
                        PlayerView[k] = new TableLayoutPanelCellPosition(i, j);
                        k += 1;
                    }

                }
            }


            else if (pOrientation == PlayerOrientation.Right)
            {
                currentPlayerposition = tlpMinimap.GetCellPosition(pbPlayer);
                int k = 0;
                for (int i = currentPlayerposition.Column + 2; i >= currentPlayerposition.Column; i--)
                {
                    for (int j = currentPlayerposition.Row - 1; j <= currentPlayerposition.Row + 1; j++)
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
