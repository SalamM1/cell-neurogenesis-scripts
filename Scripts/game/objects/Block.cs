using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class Block : PlatformCatcher
    {
        // Start is called before the first frame update

        private Vector3 previousPos;

        void Start()
        {
            previousPos = transform.position;
        }

        private void Update()
        {

        }
        // Update is called once per frame
        void FixedUpdate()
        {
            Vector3 offset = transform.position - previousPos;
            previousPos = transform.position;
            foreach (Transform target in targets)
            {
                target.position += offset;
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if(collision.contactCount > 0)
            {
                ContactPoint2D contact = collision.GetContact(0);
                if (Vector2.Dot(contact.normal, Vector2.up) > 0.5f)
                {
                    if (collision.gameObject.GetComponent<CellController>() && collision.gameObject.GetComponent<CellController>().vars.grounded)
                    {
                        collision.gameObject.GetComponent<CellController>().TakeHit(transform, 20, DamageType.HAZARD);
                    }
                    if (collision.gameObject.GetComponent<AEnemyAI>() && collision.gameObject.GetComponent<AEnemyAI>().IsGrounded())
                    {
                        collision.gameObject.GetComponent<AEnemyAI>().Damage(9999);
                    }
                }
            }
        }
    }
}