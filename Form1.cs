using fpdungeonCrawler.Map;
using fpdungeonCrawler.UI;
using Microsoft.VisualBasic.ApplicationServices;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Media;
using System.Resources;
using System.Timers;

namespace fpdungeonCrawler
{
    public partial class Form1 : Form
    {
        public fpdungeonCrawler.Map.Map map = new();
        public Player player = new Player();

        UIManager uiManager = new UIManager();
        DungeonView dungeonView = new DungeonView();
        CombatRunTime combatRunTime = new CombatRunTime();

        HUD hud = new HUD();


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

            combatRunTime.Interval = combatRunTime.CoolDowninMilliseconds/gameTimer.Interval; //3000ms

            //Map Generation
            map.GenerateMap();
            player = GeneratePlayer();
            map.GenerateMinimapUI();

            //Layerinitialisation
            dungeonView.Initialize();

            //EntityGeneration
            orc = EnemyDatabase.CreateOrc(map);
            succubus = EnemyDatabase.CreateSuccubus(map);


            dungeonView.CreateswordHitAnimation();
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
            dungeonView.PaintLayers(e);
            uiManager.enemyBar.Draw(dungeonView.entityinFront, e.Graphics, DungeonView.layerEntity);
            combatRunTime.CombatTickIndicator(e, hud.LayerCombatTick);
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
                player.StateHit = true;
            }


            UpdatePlayerposition();
            map.UpdateMinimapUI(player);
            dungeonView.DrawEnvironment(player, map);
            this.Invalidate();
        }

        public void PlayerAttack()
        {
            player.PositionInFront = player.GetPositionInFront(player, 1);
            if (map.mapTiles[player.PositionInFront.Column, player.PositionInFront.Row].entity is null)
            {
                return;
            }
            else
            {
                audiomanager.PlayAudio(Properties.Resources.weaponWhoosh);
                player.DoDamage(player, map, audiomanager);
                dungeonView.dmgAnimation.StartAnimation();
                dungeonView.swordHit.StartAnimation();
            }
        }

        private void GameTimerTick(object sender, EventArgs e)
        {
            Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff"));
            dungeonView.dmgAnimation.Update();
            dungeonView.swordHit.Update();
            orc.idleAnimation.Loop();
            succubus.idleAnimation.Loop();
            combatRunTime.CombatTick(this);

            Invalidate();
        }


        //CombatRoundTimer
        private void CombatTimerTick()
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
