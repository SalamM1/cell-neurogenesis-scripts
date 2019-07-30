using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class Spikes : MonoBehaviour
    {
        public int damage = 5;

        void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.tag.Equals("Cell"))
            {
                collision.gameObject.GetComponent<CellController>().TakeHit(transform, damage, DamageType.HAZARD);
            }
        }
    }
}

