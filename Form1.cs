namespace fpdungeonCrawler
{
    public partial class Form1 : Form
    {

        TableLayoutPanelCellPosition[] PlayerView;
        TableLayoutPanelCellPosition currentPlayerposition;

        public Form1()
        {
            InitializeComponent();
            PlayerView = new TableLayoutPanelCellPosition[9];

            // 1. Wichtig: Form fängt Tasteneingaben ab, bevor Control-Fokus greift
            //this.KeyPreview = true;

            // 2. Event-Handler an das KeyDown-Event hängen
            this.KeyDown += Playermovements;
            DrawEnvironment();
            EnvironmentTest();
        }


        private void Playermovements(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            currentPlayerposition = tlpMinimap.GetCellPosition(pbPlayer);

            if (e.KeyCode == Keys.W)
            {
                TableLayoutPanelCellPosition newPos = new TableLayoutPanelCellPosition(currentPlayerposition.Column, currentPlayerposition.Row - 1);
                tlpMinimap.SetCellPosition(pbPlayer, newPos);
            }


            if (e.KeyCode == Keys.S)
            {
                TableLayoutPanelCellPosition newPos = new TableLayoutPanelCellPosition(currentPlayerposition.Column, currentPlayerposition.Row + 1);
                tlpMinimap.SetCellPosition(pbPlayer, newPos);
            }

        }


        private void DrawEnvironment()
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
