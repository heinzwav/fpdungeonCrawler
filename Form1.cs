using Microsoft.VisualBasic.ApplicationServices;
using System.Diagnostics.Eventing.Reader;
using System.Media;
using System.Resources;
using System.Timers;

namespace fpdungeonCrawler
{
    public partial class Form1 : Form
    {
        Map map = new();
        Player player = new Player();


        Tile[] tilesLayer1 = new Tile[3];
        Tile[] tilesLayer2 = new Tile[3];
        Tile[] tilesLayer3 = new Tile[3];


        //Layer 1
        public Layer layer1 = new();
        public Size layer1Size = new Size(240, 234);
        public Point layer1Location = new Point(280, 268);

        //Layer 2
        public Layer layer2 = new();
        public Size layer2Size = new Size(480, 440);
        public Point layer2Location = new Point(175, 150);

        //Layer 3
        public Layer layer3 = new();
        public Size layer3Size = new Size(820, 690);
        public Point layer3Location = new Point(0, 0);

        //Entity Layer
        public Layer layerEntity = new();
        public Size layerEntitySize = new Size(408, 406);
        public Point layerEntityLocation = new Point(190, 150);
        public bool EntityinSight;

        Layer[] layers = new Layer[3];

        
        //Animations
        DamageAnimation dmgAnimation = new();
        Animation orcIdle = new();

        public System.Windows.Forms.Timer gameTimer = new()
        {
            Interval = 33,
            Enabled = true
        };


        public Form1()
        {
            InitializeComponent();

            //Map Generation
            map.GenerateMap();
            player = GeneratePlayer();
            map.GenerateMinimapUI();

            //Viewpoint Generation
            layer1 = layer1.InitializeLayer(layer1Size, layer1Location, Properties.Resources.Layer1_Straight);
            layer2 = layer2.InitializeLayer(layer2Size, layer2Location, Properties.Resources.Layer2_Straight);
            layer3 = layer3.InitializeLayer(layer3Size, layer3Location, Properties.Resources.Layer3_Straight);
            layerEntity = layerEntity.InitializeLayer(layerEntitySize, layerEntityLocation, Properties.Resources.Ork_Idle);


            CreateOrc();
            CreateOrcIdleAnimation();

            GetPlayerView();

            this.Paint += Form1_Paint;
            this.gameTimer.Tick += GameTimerTick;
            this.Controls.Add(map.minimap);
            this.Controls.SetChildIndex(map.minimap, 0);
            map.UpdateMinimapUI(player);


            this.KeyDown += Playermovements;

            this.pbLayer1.Visible = false;
            this.pbLayer2.Visible = false;
            this.pbLayer3.Visible = false;

            DrawEnvironment();


            this.Invalidate();
            System.Media.SoundPlayer soundPlayer = new System.Media.SoundPlayer();
            soundPlayer.SoundLocation = @"C:\Users\rinkhe\Downloads\DarkFantasyLoop.wav";
            soundPlayer.PlayLooping();
        }

        public Player GeneratePlayer()
        {
            Player player = new Player();
            Tile tile = new Tile();
            tile.TilePositionX = player.StartPosition.Column;
            tile.TilePositionY = player.StartPosition.Row;
            tile.entity = player;
            map.mapTiles[player.StartPosition.Column, player.StartPosition.Row] = tile;
            player.damage = 50;
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
                if (map.mapTiles[player.PositionInFront.Column, player.PositionInFront.Row].entity is not null)
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

            if (e.KeyCode == Keys.Space)
            {
                player.PositionInFront = player.GetPositionInFront(player, 1);
                if (map.mapTiles[player.PositionInFront.Column, player.PositionInFront.Row].entity is null)
                {
                    return;
                }
                else
                {
                    System.Media.SoundPlayer OrcSound = new System.Media.SoundPlayer();
                    OrcSound.SoundLocation = @"C:\Users\rinkhe\Downloads\weaponWhoosh.wav";
                    OrcSound.Play();
                    player.DoDamage(player, map);
                    dmgAnimation.StartAnimation();
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
                        if (i < 0 || j < 0 || i > map.SizeX-1 || j > map.SizeY-1)
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
                        if (i < 0 || j < 0 || i > 9 || j > 9)
                        {
                            EntityinSight = false;
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
                        if (i < 0 || j < 0 || i > 9 || j > 9)
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
            layer1.isVisible = true;
            GetPlayerView();
            for (int i = 0; i < 3; i++)
            {
                if (player.View[i].TilePositionX < 0 || player.View[i].TilePositionY < 0)
                {
                    tilesLayer1[i].isWall = true;
                    tilesLayer1[i].entity = null;
                }
                else
                {
                    tilesLayer1[i] = player.View[i];
                    if (tilesLayer1[i].Type == Tile.TileType.Wall)
                    {
                        tilesLayer1[i].isWall = true;
                    }
                    else
                    {
                        tilesLayer1[i].isWall = false;
                    }
                }
            }

            if (tilesLayer1[0].isWall == true && tilesLayer1[1].isWall == false && tilesLayer1[2].isWall == false)
            {
                layer1.img = Properties.Resources.Layer1_R;
            }
            else if (tilesLayer1[0].isWall == true && tilesLayer1[1].isWall == false && tilesLayer1[2].isWall == true)
            {
                layer1.img = Properties.Resources.Layer1_Straight;
            }
            else if (tilesLayer1[0].isWall == false && tilesLayer1[1].isWall == false && tilesLayer1[2].isWall == true)
            {
                layer1.img = Properties.Resources.Layer1_L;
            }
            else if (tilesLayer1[0].isWall == false && tilesLayer1[1].isWall == false && tilesLayer1[2].isWall == false)
            {
                layer1.img = Properties.Resources.Layer1_T;
            }
            else if (tilesLayer1[1].isWall == true)
            {
                layer1.img = Properties.Resources.Layer1_Wall;
            }

            if (tilesLayer1[1].entity is null)
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
            layer1.isVisible = true;
            layer2.isVisible = true;
            GetPlayerView();
            for (int i = 3; i < 6; i++)
            {
                int j = i - 3;

                tilesLayer2[j] = player.View[i];
                if (tilesLayer2[j].Type == Tile.TileType.Wall)
                {
                    tilesLayer2[j].isWall = true;
                }
                else
                {
                    tilesLayer2[j].isWall = false;
                }
            }
            if (tilesLayer2[0].isWall == true && tilesLayer2[1].isWall == false && tilesLayer2[2].isWall == false)
            {
                layer2.img = Properties.Resources.Layer2_R;
            }
            else if (tilesLayer2[0].isWall == true && tilesLayer2[1].isWall == false && tilesLayer2[2].isWall == true)
            {
                layer2.img = Properties.Resources.Layer2_Straight;
            }
            else if (tilesLayer2[0].isWall == false && tilesLayer2[1].isWall == false && tilesLayer2[2].isWall == true)
            {
                layer2.img = Properties.Resources.Layer2_L;
            }
            else if (tilesLayer2[0].isWall == false && tilesLayer2[1].isWall == false && tilesLayer2[2].isWall == false)
            {
                layer2.img = Properties.Resources.Layer2_T;
            }
            else if (tilesLayer2[1].isWall == true)
            {
                layer2.img = Properties.Resources.Layer2_Wall;
                layer1.isVisible = false;
            }


            //Check if Entity is in Front

            if (tilesLayer1[1].entity is not null)
            {
                EntityinSight = true;
                SetRecEntitySize(2);
            }

            else if (tilesLayer2[1].entity is null && tilesLayer1[1].entity is null)
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
            layer3.isVisible = true;
            GetPlayerView();
            for (int i = 6; i < 9; i++)
            {
                int j = i - 6;

                tilesLayer3[j] = player.View[i];

                if (tilesLayer3[j].Type == Tile.TileType.Wall)
                {
                    tilesLayer3[j].isWall = true;
                }
                else
                {
                    tilesLayer3[j].isWall = false;
                }
            }
            if (tilesLayer3[0].isWall == true && tilesLayer3[1].isWall == false && tilesLayer3[2].isWall == true)
            {
                layer3.img = Properties.Resources.Layer3_Straight;
            }
            else if (tilesLayer3[0].isWall == true && tilesLayer3[1].isWall == false && tilesLayer3[2].isWall == false)
            {
                layer3.img = Properties.Resources.Layer3_R;
            }
            else if (tilesLayer3[0].isWall == false && tilesLayer3[1].isWall == false && tilesLayer3[2].isWall == true)
            {
                layer3.img = Properties.Resources.Layer3_L;
            }
            else if (tilesLayer3[0].isWall == false && tilesLayer3[1].isWall == false && tilesLayer3[2].isWall == false)
            {
                layer3.img = Properties.Resources.Layer3_T;
            }
        }

        private void Check3TilesInFront()
        {
            if(player.GetPositionInFront(player, 3).Column < 0 || player.GetPositionInFront(player, 3).Column > 9 ||
                player.GetPositionInFront(player, 3).Row < 0 || player.GetPositionInFront(player, 3).Row > 9)
            {
                return;
            }

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
            layers[0] = layer3;
            layers[1] = layer2;
            layers[2] = layer1;

            foreach (Layer layer in layers)
            {
                if (layer.isVisible)
                {
                    e.Graphics.DrawImage(layer.img, layer.rec);
                }
            }
            

            if (EntityinSight)
            {
                layerEntity.img = orcIdle.GetCurrentFrameSprite(orcIdle.currentFrame);
                e.Graphics.DrawImage(layerEntity.img, layerEntity.rec);
            }
            else if (EntityinSight == false)
            {
                layerEntity.rec.Size = new Size(0, 0);
                layerEntity.rec.Location = new Point(0, 0);
            }

            dmgAnimation.DoDamageAnimation(e.Graphics);
        }

        public void CreateOrc()
        {
            Entity orc = new Entity();
            orc.CurrentPosition = (1, 5);
            orc.StartPosition = orc.CurrentPosition;
            orc.healthpoints = 200;
            orc.isEnemy = true;
            map.UpdateEntityLocation(orc);
        }

        public void CreateOrcIdleAnimation()
        {
            orcIdle.frameSprites = new Image[2];
            orcIdle.frameSprites[0] = Properties.Resources.OrkIdle1;
            orcIdle.frameSprites[1] = Properties.Resources.OrkIdle2;
            orcIdle.maxFrames = 2;
            orcIdle.interval = 16;
            orcIdle.isActive = true;
        }

        public void SetRecEntitySize(int distance)
        {
            switch (distance)
            {
                case 3:
                    layerEntity.rec.Size = new Size(120, 114);
                    layerEntity.rec.Location = new Point(335, 350);
                    break;
                case 2:
                    layerEntity.rec.Size = layer1.rec.Size;
                    layerEntity.rec.Location = layer1.rec.Location;
                    break;
                case 1:
                    layerEntity.rec.Size = layer2.rec.Size;
                    layerEntity.rec.Location = layer2.rec.Location;
                    break;
            }
        }


        private void GameTimerTick(object sender, EventArgs e)
        {
            dmgAnimation.Update();
            orcIdle.Loop();
            Invalidate();
        }
    }
}
