using UnityEngine;
using System.Collections;

namespace com.egamesstudios.cell
{
    public static class DirectionExtension
    {
        /// <summary>
        /// Obtains the Vector2 of the direction of the operated enum
        /// </summary>
        public static Vector2 GetVector(this Direction direction)
        {
            Vector2 toRet = Vector2.zero;
            switch (direction)
            {
                case Direction.UP:
                    toRet = Vector2.up;
                    break;
                case Direction.DOWN:
                    toRet = Vector2.down;
                    break;
                case Direction.LEFT:
                    toRet = Vector2.left;
                    break;
                case Direction.RIGHT:
                    toRet = Vector2.right;
                    break;

            }
            return toRet;
        }
    }
    /// <summary>
    /// A general 4-directional enumerator; used when needing a direction mapped (eg. a door's opening direction)
    /// </summary>
    public enum Direction
    {

        UP,
        DOWN,
        LEFT,
        RIGHT,

    }

}
