using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulletz
{
    class EarthGrass : GameObject
    {
        public EarthGrass() : base("earthGrass")
        {
            Position = new Vector2(2);
            IsActive = true;
            RigidBody = new RigidBody(this, Vector2.Zero);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.IsGravityAffected = false;
            RigidBody.Type = RigidBodyType.Earth;

            DrawMngr.Add(this);
            //DebugMngr.AddItem(RigidBody.Collider);
        }
    }
}
