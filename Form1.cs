using System.Resources;

namespace fpdungeonCrawler
{
    public partial class Form1 : Form
    {

        TableLayoutPanelCellPosition[] PlayerView;
        TableLayoutPanelCellPosition currentPlayerposition;

        Player Map = new();
        bool[] Layer1 = new bool[3];
        bool[] Layer2 = new bool[3];
        bool[] Layer3 = new bool[3];



        public Form1()
        {
            InitializeComponent();
            PlayerView = new TableLayoutPanelCellPosition[9];
            GetPlayerView();
            DrawEnvironment();

            this.KeyDown += Playermovements;
            
            
        }


        private void Playermovements(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            currentPlayerposition = tlpMinimap.GetCellPosition(pbPlayer);

            if (e.KeyCode == Keys.W)
            {
                TableLayoutPanelCellPosition newPos = new TableLayoutPanelCellPosition(currentPlayerposition.Column, currentPlayerposition.Row - 1);
                Control nextControl = tlpMinimap.GetControlFromPosition(newPos.Column, newPos.Row);
                nextControl.Dispose();
                tlpMinimap.SetCellPosition(pbPlayer, newPos);
                PictureBox emptytile = new PictureBox();
                emptytile.Tag = "empty";
                tlpMinimap.Controls.Add(emptytile, currentPlayerposition.Column, currentPlayerposition.Row);
            }


            if (e.KeyCode == Keys.S)
            {
                TableLayoutPanelCellPosition newPos = new TableLayoutPanelCellPosition(currentPlayerposition.Column, currentPlayerposition.Row + 1);
                Control nextControl = tlpMinimap.GetControlFromPosition(newPos.Column, newPos.Row);
                nextControl.Dispose();
                tlpMinimap.SetCellPosition(pbPlayer, newPos);
                PictureBox emptytile = new PictureBox();
                emptytile.Tag = "empty";
                tlpMinimap.Controls.Add(emptytile, currentPlayerposition.Column, currentPlayerposition.Row);
            }

            DrawEnvironment();
        }


        private void GetPlayerView()
        {
            currentPlayerposition = tlpMinimap.GetCellPosition(pbPlayer);
            int k = 0;
            for (int i = currentPlayerposition.Row - 2; i <= currentPlayerposition.Row; i++)
            {
                for (int j = currentPlayerposition.Column - 1; j <= currentPlayerposition.Column + 1; j++)
                {
                    PlayerView[k] = new TableLayoutPanelCellPosition(i, j);
                    k += 1;
                }

            }
        }


        private void CheckLayer1()
        {
            GetPlayerView();
            for (int i = 0; i < 3; i++)
            {
                Control control = tlpMinimap.GetControlFromPosition(PlayerView[i].Row, PlayerView[i].Column);
                if (control.Tag == "Wall")
                {
                    Layer1[i] = true;
                }
                else
                {
                    Layer1[i] = false;
                }
            }
            if (Layer1[0] == true && Layer1[1] == false && Layer1[2] == false)
            {
                pbLayer1.BackgroundImage = Properties.Resources.Layer1_DoorRightPic;
            }
            else if(Layer1[0] == true && Layer1[1] == false && Layer1[2] == true)
            {
                pbLayer1.BackgroundImage = Properties.Resources.Layer1_HallPic;
            }
            else if (Layer1[0] == true && Layer1[1] == true && Layer1[2] == true)
            {
                pbLayer1.BackgroundImage = Properties.Resources.Layer1_WallPic1;
            }
        }

        private void CheckLayer2()
        {
            GetPlayerView();
            for (int i = 3; i < 6; i++)
            {
                Control control = tlpMinimap.GetControlFromPosition(PlayerView[i].Row, PlayerView[i].Column);
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
            else if (Layer2[0] == true && Layer2[1] == true && Layer2[2] == true)
            {
                pbLayer2.BackgroundImage = Properties.Resources.Layer2_Wall;
            }
        }

        private void CheckLayer3()
        {
            GetPlayerView();
            for (int i = 6; i < 9; i++)
            {
                Control control = tlpMinimap.GetControlFromPosition(PlayerView[i].Row, PlayerView[i].Column);
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
