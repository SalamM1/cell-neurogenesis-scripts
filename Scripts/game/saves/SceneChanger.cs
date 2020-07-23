using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.egamesstudios.cell
{
    public class SceneChanger : MonoBehaviour
    {
        public string connectedScene;
        [SerializeField]
        public int ID;
        [EnumToggleButtons, SerializeField]
        private Direction direction;
        public Transform spawnHereOnEnter;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag.Equals("Cell") && !CameraManager.cameraManager.DEBUG)
            {
                StartCoroutine(LoadSceneManager.loadSceneManager.LoadLevel(ID, connectedScene));
                collision.GetComponent<CellController>().SetTransitionSpeed(direction.GetVector() * 0.5f);
            }
        }
    }
}