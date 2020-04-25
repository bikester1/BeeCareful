using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code
{
    public interface CollisionCallable
    {
        void OnCollisionEnter(Collision collision);

        void OnCollisionStay(Collision collision);

        void OnCollisionExit(Collision collision);

        void OnTriggerEnter(Collider collider);
    }
}
