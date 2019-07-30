using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class CellParticlePlayer : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] particles;

        public void PlayParticle(CellParticleType type, Vector3 offset)
        {
            GameObject temp = null;
            switch(type)
            {
                case CellParticleType.WALK:
                    temp = particles[0];
                    break;
                case CellParticleType.JUMP:
                    temp = particles[1];
                    break;
                case CellParticleType.WALLSLIDE:
                    temp = particles[2];
                    break;
                case CellParticleType.WALLJUMP:
                    temp = particles[3];
                    break;
                case CellParticleType.LAND:
                    temp = particles[4];
                    break;
            }
            if(temp != null)
            {
                Instantiate(temp, transform.position + offset, Quaternion.identity);
            }
        }
    }

    public enum CellParticleType
    {
        WALLJUMP,
        WALLSLIDE,
        WALK,
        JUMP,
        LAND,
        SWING,
        HIT,
        CHARGE,
    }
}

