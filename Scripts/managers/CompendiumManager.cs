using UnityEngine;
using System.Collections;

namespace com.egamesstudios.cell
{
    public class CompendiumManager : MonoBehaviour
    {
        
        private void Awake()
        {

        }

        public string[] LoadCompendium()
        {
            string content = ((TextAsset)Resources.Load("compendium/", typeof(TextAsset))).text;
            return JsonHelper.FromJson<string>(content);
        }
    }
    public enum EnemyType
    {
        GERM,
        VIRUS,
        BACTERIA,
    }
}
