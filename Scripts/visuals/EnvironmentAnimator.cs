using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell 
{
	public class EnvironmentAnimator : MonoBehaviour
	{
        private void OnTriggerStay2D(Collider2D collision)
        {
            if(collision.tag.Equals("Cell") || collision.tag.Equals("Enemy"))
            {
                GetComponent<Animator>().SetBool("animate", true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag.Equals("Cell") || collision.tag.Equals("Enemy"))
            {
                GetComponent<Animator>().SetBool("animate", false);
            }
        }
    }
}