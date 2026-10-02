using System.Diagnostics.Eventing.Reader;
using System.Resources;
using System.Timers;

namespace fpdungeonCrawler
{
    public partial class Form1 : Form
    {
        Map map = new();
        Player player = new Player();


        Tile[] Layer1 = new Tile[3];
        Tile[] Layer2 = new Tile[3];
        Tile[] Layer3 = new Tile[3];

        public Rectangle recLayer1 = new Rectangle
        {
            Size = new Size(224, 234),
            Location = new Point(288, 268)
        };

        public Rectangle recLayer2 = new Rectangle
        {
            Size = new Size(408, 406),
            Location = new Point(190, 150)
        };

        public Rectangle recLayer3 = new Rectangle
        {
            Size = new Size(820, 690),
            Location = new Point(0, 0)
        };

        public Rectangle recEntity = new Rectangle
        {
            Size = new Size(408, 406),
            Location = new Point(190, 150)
        };

        public bool EntityinSight;
        public Image imgEntity = Properties.Resources.OrkIdle1;
        public Image imgLayer1 = Properties.Resources.Layer1_Straight;
        public Image imgLayer2 = Properties.Resources.Layer2_Straight;
        public Image imgLayer3 = Properties.Resources.Layer3_Straight;


        public Form1()
        {
            InitializeComponent();


            map.GenerateMap();
            player = GeneratePlayer();
            map.GenerateMinimapUI();

            CreateOrc();

            GetPlayerView();

            this.Paint += Form1_Paint;
            this.Controls.Add(map.minimap);
            this.Controls.SetChildIndex(map.minimap, 0);
            map.UpdateMinimapUI(player);


            this.KeyDown += Playermovements;

            this.pbLayer1.Visible = false;
            this.pbLayer2.Visible = false;
            this.pbLayer3.Visible = false;

            DrawEnvironment();


            this.Invalidate();
        }

        public Player GeneratePlayer()
        {
            Player player = new Player();
            Tile tile = new Tile();
            tile.TilePositionX = player.StartPosition.Column;
            tile.TilePositionY = player.StartPosition.Row;
            tile.entity = player;
            map.mapTiles[player.StartPosition.Column, player.StartPosition.Row] = tile;
            player.orientation = Player.PlayerOrientation.Up;
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
                player.PositionInFront = player.GetPositionInFront(player, 1);
                if (map.mapTiles[player.PositionInFront.Column, player.PositionInFront.Row].Type == Tile.TileType.Wall)
                {
                    return;
                }
                player.CurrentPosition = (player.PositionInFront);
            }


            if (e.KeyCode == Keys.S)
            {
                player.PositionBehind = player.GetPositionBehind(player);
                if (map.mapTiles[player.PositionBehind.Column, player.PositionBehind.Row].Type == Tile.TileType.Wall)
                {
                    return;
                }

                player.CurrentPosition = (player.PositionBehind);
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
            map.UpdateMinimapUI(player);
            DrawEnvironment();
            this.Invalidate();
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
                        if (i < 0 || j < 0)
                        {
                            k++;
                        }
                        else
                        {
                            player.View[k] = map.mapTiles[j, i];
                            k++;
                        }
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
                        if (i < 0 || j < 0 || i > 9 || j > 9)
                        {
                            k++;
                        }
                        else
                        {
                            player.View[k] = map.mapTiles[j, i];
                            k++;
                        }
                    }
                }
            }

            else if (player.orientation == Player.PlayerOrientation.Left)
            {
                int k = 0;
                for (int i = player.CurrentPosition.Column - 2; i <= player.CurrentPosition.Column; i++)
                {
                    for (int j = player.CurrentPosition.Row + 1; j >= player.CurrentPosition.Row - 1; j--)
                    {
                        if (i < 0 || j < 0)
                        {
                            k++;
                        }
                        else
                        {
                            player.View[k] = map.mapTiles[i, j];
                            k++;
                        }
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
                        if (i < 0 || j < 0)
                        {
                            k++;
                        }
                        else
                        {
                            player.View[k] = map.mapTiles[i, j];
                            k++;
                        }
                    }

                }
            }
        }


        private void CheckLayer1()
        {
            GetPlayerView();
            for (int i = 0; i < 3; i++)
            {
                if (player.View[i].TilePositionX < 0 || player.View[i].TilePositionY < 0)
                {
                    Layer1[i].isWall = true;
                }
                else
                {
                    Layer1[i] = player.View[i];
                    if (Layer1[i].Type == Tile.TileType.Wall)
                    {
                        Layer1[i].isWall = true;
                    }
                    else
                    {
                        Layer1[i].isWall = false;
                    }
                }
            }

            if (Layer1[0].isWall == true && Layer1[1].isWall == false && Layer1[2].isWall == false)
            {
                imgLayer1 = Properties.Resources.Layer1_R;
            }
            else if (Layer1[0].isWall == true && Layer1[1].isWall == false && Layer1[2].isWall == true)
            {
                imgLayer1 = Properties.Resources.Layer1_Straight;
            }
            else if (Layer1[0].isWall == false && Layer1[1].isWall == false && Layer1[2].isWall == true)
            {
                imgLayer1 = Properties.Resources.Layer1_L;
            }
            else if (Layer1[0].isWall == false && Layer1[1].isWall == false && Layer1[2].isWall == false)
            {
                imgLayer1 = Properties.Resources.Layer1_T;
            }
            else if (Layer1[1].isWall == true)
            {
                imgLayer1 = Properties.Resources.Layer1_Wall;
            }

            if (Layer1[1].entity is null)
            {
                EntityinSight = false;
            }
            else
            {
                EntityinSight = true;
                SetRecEntitySize(2);
            }
        }

        private void CheckLayer2()
        {
            GetPlayerView();
            for (int i = 3; i < 6; i++)
            {
                int j = i - 3;

                Layer2[j] = player.View[i];
                if (Layer2[j].Type == Tile.TileType.Wall)
                {
                    Layer2[j].isWall = true;
                }
                else
                {
                    Layer2[j].isWall = false;
                }
            }
            if (Layer2[0].isWall == true && Layer2[1].isWall == false && Layer2[2].isWall == false)
            {
                imgLayer2 = Properties.Resources.Layer2_R;
            }
            else if (Layer2[0].isWall == true && Layer2[1].isWall == false && Layer2[2].isWall == true)
            {
                imgLayer2 = Properties.Resources.Layer2_Straight;
            }
            else if (Layer2[0].isWall == false && Layer2[1].isWall == false && Layer2[2].isWall == true)
            {
                imgLayer2 = Properties.Resources.Layer2_L;
            }
            else if (Layer2[0].isWall == false && Layer2[1].isWall == false && Layer2[2].isWall == false)
            {
                imgLayer2 = Properties.Resources.Layer2_T;
            }
            else if (Layer2[1].isWall == true)
            {
                imgLayer2 = Properties.Resources.Layer2_Wall;
            }


            //Check if Entity is in Front

            if (Layer1[1].entity is not null)
            {
                EntityinSight = true;
                SetRecEntitySize(2);
            }

            else if (Layer2[1].entity is null && Layer1[1].entity is null)
            {
                EntityinSight = false;
            }

            else
            {
                EntityinSight = true;
                SetRecEntitySize(1);
            }
        }

        private void CheckLayer3()
        {
            GetPlayerView();
            for (int i = 6; i < 9; i++)
            {
                int j = i - 6;

                Layer3[j] = player.View[i];

                if (Layer3[j].Type == Tile.TileType.Wall)
                {
                    Layer3[j].isWall = true;
                }
                else
                {
                    Layer3[j].isWall = false;
                }
            }
            if (Layer3[0].isWall == true && Layer3[1].isWall == false && Layer3[2].isWall == true)
            {
                imgLayer3 = Properties.Resources.Layer3_Straight;
            }
            else if (Layer3[0].isWall == true && Layer3[1].isWall == false && Layer3[2].isWall == false)
            {
                imgLayer3 = Properties.Resources.Layer3_R;
            }
            else if (Layer3[0].isWall == false && Layer3[1].isWall == false && Layer3[2].isWall == true)
            {
                imgLayer3 = Properties.Resources.Layer3_L;
            }
            else if (Layer3[0].isWall == false && Layer3[1].isWall == false && Layer3[2].isWall == false)
            {
                imgLayer3 = Properties.Resources.Layer3_T;
            }
        }

        private void Check3TilesInFront()
        {
            Tile tile = map.mapTiles[player.GetPositionInFront(player, 3).Column, player.GetPositionInFront(player, 3).Row];

            switch (tile.entity)
            {
                case null:
                    break;
                case not null:
                    SetRecEntitySize(3);
                    EntityinSight = true;
                    break;
            }


        }


        private void DrawEnvironment()
        {
            CheckLayer1();
            CheckLayer2();
            CheckLayer3();
            Check3TilesInFront();
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(imgLayer3, recLayer3);
            e.Graphics.DrawImage(imgLayer2, recLayer2);
            e.Graphics.DrawImage(imgLayer1, recLayer1);

            if (EntityinSight)
            {
                e.Graphics.DrawImage(imgEntity, recEntity);
            }

        }

        public void CreateOrc()
        {
            Entity orc = new Entity();
            orc.CurrentPosition = (1, 5);
            orc.StartPosition = orc.CurrentPosition;
            orc.isEnemy = true;
            map.UpdateEntityLocation(orc);
        }

        public void SetRecEntitySize(int distance)
        {
            switch (distance)
            {
                case 3:
                    recEntity.Size = new Size(120, 114);
                    recEntity.Location = new Point(335, 350);
                    break;
                case 2:
                    recEntity.Size = recLayer1.Size;
                    recEntity.Location = recLayer1.Location;
                    break;
                case 1:
                    recEntity.Size = recLayer2.Size;
                    recEntity.Location = recLayer2.Location;
                    break;
            }
        }
    }
}
