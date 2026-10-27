using fpdungeonCrawler.Map;
using fpdungeonCrawler.UI;
using Microsoft.VisualBasic.ApplicationServices;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Media;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using System.Timers;
using fpdungeonCrawler.Map;

namespace fpdungeonCrawler
{
    public partial class Form1 : Form
    {

        public Map.Map map = new();
        public Player player = new Player();

        UIManager uiManager = new UIManager();
        public DungeonView dungeonView = new DungeonView();
        CombatRunTime combatRunTime = new CombatRunTime();

        HUD hud = new HUD();


        public List<Entity> entities = new List<Entity>();
        Entity orc = new();
        Entity succubus = new();


        //GameTimer
        public System.Windows.Forms.Timer gameTimer = new()
        {
            Interval = 66,
            Enabled = true
        };




        //Audio
        public Audiomanager audiomanager = new Audiomanager();
        public Audiomanager musicmanager = new Audiomanager();

        public Form1()
        {

            InitializeComponent();

            combatRunTime.Interval = combatRunTime.CoolDowninMilliseconds / gameTimer.Interval; //3000ms

            //Map Generation
            map.GenerateMap();
            player = player.GeneratePlayer(map);
            map.GenerateMinimapUI();

            //Layerinitialisation
            dungeonView.Initialize();

            map.SpawnEnemies();
            //EntityGeneration
            ////////////entities.Add(succubus = EnemyDatabase.CreateSuccubus(map));
            ////////////entities.Add(orc = EnemyDatabase.CreateOrc(map));


            dungeonView.GetPlayerView(player, map);

            this.Paint += Form1_Paint;
            this.gameTimer.Tick += GameTimerTick;
            this.Controls.Add(map.minimap);
            this.Controls.SetChildIndex(map.minimap, 0);
            map.UpdateMinimapUI(player);


            this.KeyDown += Playermovements;

            dungeonView.DrawEnvironment(player, map);


            this.Invalidate();

            musicmanager.PlayLoop(Properties.Resources.DarkFantasyLoop);
        }


        //Draw Graphics
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            dungeonView.PaintLayers(e, player);

            if (dungeonView.entityinSight)
            {
                uiManager.enemyBar.Draw(dungeonView.entityinFront, e.Graphics, DungeonView.layerEntity);
            }

            uiManager.playerhud.Draw(player, e.Graphics, DungeonView.layerPlayer);
            combatRunTime.CombatTickIndicator(e, hud.LayerCombatTick);
        }

        public void UpdatePlayerposition()
        {
            foreach (Tile tile in map.mapTiles)
            {
                if (tile.player == player)
                {
                    tile.player = null;
                }
                map.mapTiles[player.CurrentPosition.Column, player.CurrentPosition.Row].player = player;
            }
        }


        //Player Movement || Key Actions
        private void Playermovements(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                player.PositionInFront = player.GetPositionInFront(player, 1);
                if (map.mapTiles[player.PositionInFront.Column, player.PositionInFront.Row].Type == Tile.TileType.Wall)
                {
                    return;
                }
                if (map.mapTiles[player.PositionInFront.Column, player.PositionInFront.Row].entity is not null && map.mapTiles[player.PositionInFront.Column, player.PositionInFront.Row].entity!.healthpoints > 0)
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
                    player.orientation = Player.EntityOrientation.Left;
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
                    player.orientation = Player.EntityOrientation.Down;
                }
            }

            if (e.KeyCode == Keys.Space)
            {
                player.StateHit = true;
            }


            UpdatePlayerposition();
            map.UpdateEntityLocation(player);
            map.UpdateMinimapUI(player);
            dungeonView.DrawEnvironment(player, map);
            this.Invalidate();
        }

        private void GameTimerTick(object sender, EventArgs e)
        {
            dungeonView.dmgAnimation.Update();
            player.attackAnimation.Update();

            if (dungeonView.entityinSight)
            {
                dungeonView.entityinFront.activeAnimation.Update();

            }
            combatRunTime.CombatTick(this);

            Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
