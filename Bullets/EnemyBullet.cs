using OpenTK;

namespace Bulletz
{
    class EnemyBullet : Bullet
    {
        public EnemyBullet() : base("bullet")
        {
            Dmg = 25;

            Type = BulletType.EnemyBullet;
            RigidBody.Type = RigidBodyType.EnemyBullet;
        }
    }
}
