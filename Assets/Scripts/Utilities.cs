using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gokyolcu
{
    public class Utilities : MonoBehaviour
    {
        #region Transform - Vectors

        public static Vector2 GetFront(Quaternion quaternion)
        {
            float degree = quaternion.eulerAngles.z % 360; // for negative/positive calculation
            float radian = degree * Mathf.Deg2Rad;
            float cotangent = 1 / Mathf.Tan(radian);

            float x;
            float y;
            switch (degree)
            {
                case 0:
                    x = 1;
                    y = 0;
                    break;
                case 90:
                    x = 0;
                    y = 1;
                    break;
                case 180:
                    x = -1;
                    y = 0;
                    break;
                case 270:
                    x = 0;
                    y = -1;
                    break;
                default:
                    if (degree < 180)
                        y = 1;
                    else
                        y = -1;

                    x = y * cotangent;
                    break;
            }

            Vector2 jumpDirection = new Vector2(x, y);
            return jumpDirection.normalized;
        }

        public static Vector2 Vector2Rotate(Vector2 vector, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = vector.x;
            float ty = vector.y;
            vector.x = (cos * tx) - (sin * ty);
            vector.y = (sin * tx) + (cos * ty);

            return vector;
        }

        #endregion

        #region Unity Misc

        public static bool DoesLayerMaskContainLayer(LayerMask layerMask, int layer)
        {
            if (((1 << layer) & layerMask) != 0) return true;

            return false;
        }

        #endregion

        #region Mouse and Screen Positions

        // Get mouse position in world with Z = 0f
        public static Vector3 GetMouseWorldPositionWithoutZ()
        {
            Vector3 mousePosition = ScreenToWorldPosition(Input.mousePosition, Camera.main);
            mousePosition.z = 0f;
            return mousePosition;
        }

        public static Vector3 GetMouseWorldPosition()
        {
            return ScreenToWorldPosition(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetMouseWorldPosition(Camera worldCamera)
        {
            return ScreenToWorldPosition(Input.mousePosition, worldCamera);
        }

        public static Vector3 ScreenToWorldPosition(Vector3 screenPosition, Camera worldCamera)
        {
            return worldCamera.ScreenToWorldPoint(screenPosition);
        }

        #endregion

        #region Debug

        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000)
        {
            if (!color.HasValue) color = Color.white;
            return CreateWorldText(parent, text, localPosition, fontSize, color.Value, textAnchor, textAlignment, sortingOrder);
        }

        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.localScale = transform.localScale * .15f;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;

            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }

        #endregion

        #region Math

        public static float GetSign(float f)
        {
            if (f > 0)
                return 1;
            else if (f < 0)
                return -1;
            else
                return 0;
        }

        public static float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        #endregion

        #region Misc

        public static Color ConvertColorFrom255To1(int r, int g, int b)
        {
            return new Color((float)r / 255, (float)g / 255, (float)b / 255);
        }

        #endregion
    }
}