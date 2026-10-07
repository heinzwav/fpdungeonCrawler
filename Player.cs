using System;
using System.Collections.Generic;
using System.Media;
using System.Text;

namespace fpdungeonCrawler
{

    public class Player : Entity
    {
        public enum PlayerOrientation { Left, Up, Right, Down }
        public PlayerOrientation orientation = PlayerOrientation.Up;

        public new (int Column, int Row) CurrentPosition { get; set; } = new(1, 7);

        public new (int Column, int Row) StartPosition { get; set; } = new(1, 7);

        public (int Column, int Row) PositionInFront { get; set; }

        public (int Column, int Row) PositionBehind { get; set; }

        public Tile[] View = new Tile[9];


        public (int Column, int Row) GetPositionInFront(Player player, int distance)
        {
            switch (player.orientation)
            {
                case PlayerOrientation.Up:
                    player.PositionBehind = (CurrentPosition.Column, CurrentPosition.Row - distance);
                    break;

                case PlayerOrientation.Down:
                    player.PositionBehind = (CurrentPosition.Column, CurrentPosition.Row + distance);
                    break;

                case PlayerOrientation.Left:
                    player.PositionBehind = (CurrentPosition.Column - distance, CurrentPosition.Row);
                    break;

                case PlayerOrientation.Right:
                    player.PositionBehind = (CurrentPosition.Column + distance, CurrentPosition.Row);
                    break;
            }
            return player.PositionBehind;
        }

        public (int Column, int Row) GetPositionBehind(Player player)
        {
            switch (player.orientation)
            {
                case PlayerOrientation.Up:
                    player.PositionBehind = (CurrentPosition.Column, CurrentPosition.Row + 1);
                    break;

                case PlayerOrientation.Down:
                    player.PositionBehind = (CurrentPosition.Column, CurrentPosition.Row - 1);
                    break;

                case PlayerOrientation.Left:
                    player.PositionBehind = (CurrentPosition.Column + 1, CurrentPosition.Row);
                    break;

                case PlayerOrientation.Right:
                    player.PositionBehind = (CurrentPosition.Column - 1, CurrentPosition.Row);
                    break;
            }
            return player.PositionBehind;
        }

        public void DoDamage(Player player, Map map)
        {
            Tile tile = map.mapTiles[player.PositionInFront.Column, player.PositionInFront.Row];
            if (tile?.entity != null)
            {
                Entity entity;
                entity = tile.entity;


                entity.healthpoints -= player.damage;

                if (entity.healthpoints <= 0)
                {
                    System.Media.SoundPlayer OrcSound = new System.Media.SoundPlayer();
                    OrcSound.SoundLocation = @"C:\Users\rinkhe\Downloads\SchattenFlieht.wav";
                    OrcSound.Play();
                    tile.entity = null;
                }
            }
        }

    }


}
