using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class Door : MonoBehaviour
    {
        [OnValueChanged("SetChildren")]
        public int size;
        [SerializeField]
        private Sprite doorSprite;
        [OnValueChanged("SetChildren"), EnumToggleButtons]
        public Direction direction;
        [Button]
        public void SetChildren()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                var temp = transform.GetChild(i);
                temp.transform.parent = null;
                DestroyImmediate(temp.gameObject);
            }
            bool vert = (direction == Direction.UP || direction == Direction.DOWN);
            GetComponent<BoxCollider2D>().size = vert ? new Vector2(1, size - 0.01f) : new Vector2(size, 0.99f);
            float rotation = vert ? 0 : 90;

            float totalSummon = size / 2;

            if (size % 2 != 0)
            {
                CreateDoorPiece(vert, rotation, 0);
                for (int i = 1; i <= totalSummon; i++)
                {
                    CreateDoorPiece(vert, rotation, i);
                    CreateDoorPiece(vert, rotation, -(i));
                }
            }
            else
            {
                for (int i = 0; i < totalSummon; i++)
                {
                    CreateDoorPiece(vert, rotation, 0.5f + i);
                    CreateDoorPiece(vert, rotation, -(0.5f + i));
                }
            }
        }


        void CreateDoorPiece(bool vert, float rotation, float distance)
        {
            GameObject tempChild = new GameObject("" + distance);
            (tempChild.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer).sprite = doorSprite;
            var spr = tempChild.GetComponent<SpriteRenderer>();
            spr.sortingLayerName = "Base";
            spr.sortingOrder = -1;
            tempChild.transform.parent = transform;
            tempChild.transform.localPosition = vert ? new Vector2(0, distance) : new Vector2(distance, 0);
            tempChild.transform.rotation = Quaternion.Euler(0, 0, rotation);
        }
    }
}
