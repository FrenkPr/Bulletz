using System.Collections.Generic;
using OpenTK;

namespace Bulletz
{
    static class EnemyMngr
    {
        private static Queue<Enemy> enemies;
        private static int numEnemies;
        //private static float timeToNextSpawn;

        public static void Init()
        {
            numEnemies = 16;
            enemies = new Queue<Enemy>();

            for (int j = 0; j < numEnemies; j++)
            {
                enemies.Enqueue(new Enemy());
            }
        }

        public static void RestoreEnemy(Enemy enemy)
        {
            enemies.Enqueue(enemy);
            enemy.IsActive = false;
            enemy.EnergyBar.IsActive = false;

            enemy.ResetEnergy();
        }

        public static void SpawnEnemy()
        {
            if (enemies.Count > 0)
            {
                Background b = ((PlayScene)Game.Scene).Background;
                Enemy e = enemies.Dequeue();
                e.IsActive = true;
                e.EnergyBar.IsActive = true;

                e.Position = new Vector2(RandomGenerator.GetRandomInt((int)(b.Position.X + e.HalfWidth), (int)(b.Position.X + b.Width - e.HalfWidth)), PhysicsMngr.GroundY);
                e.EnergyBar.Position = new Vector2(e.Position.X - e.EnergyBar.HalfWidth, e.Position.Y - e.HalfHeight - e.EnergyBar.Height);

                //timeToNextSpawn = /*200*/RandomGenerator.GetRandomInt(1, 4);
            }
        }

        public static void ClearAll()
        {
            enemies.Clear();
        }
    }
}
