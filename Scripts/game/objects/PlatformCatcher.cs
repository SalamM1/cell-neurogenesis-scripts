using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public abstract class PlatformCatcher : MonoBehaviour
    {

        protected List<Transform> targets;

        void Awake()
        {
            targets = new List<Transform>();
        }
        public void ClearTargets()
        {
            targets.Clear();
        }
        void OnTriggerEnter2D(Collider2D col)
        {
            if (!targets.Contains(col.transform) && (col.tag.Equals("Cell") || col.tag.Equals("Enemy") || col.tag.Equals("NPC") || col.tag.Equals("Block"))) targets.Add(col.transform);
        }
        void OnTriggerExit2D(Collider2D col)
        {
            if (targets.Contains(col.transform)) targets.Remove(col.transform);
        }

    }
}
