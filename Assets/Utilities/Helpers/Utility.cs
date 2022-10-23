using System.Collections.Generic;
using System.Linq;
using Random = System.Random;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Racer.Utilities
{
    public static class Utility
    {
        private static Camera _cameraMain;
        /// <summary>
        /// Gets a one time reference to the Camera.Main Method. 
        /// </summary>
        public static Camera CameraMain
        {
            get
            {
                if (_cameraMain == null)
                    _cameraMain = Camera.main;

                return _cameraMain;
            }
        }
        /// <summary>
        /// Finds and returns a gameobject's component by a specified tag.
        /// </summary>
        /// <typeparam name="T">Type of component to return</typeparam>
        /// <param name="tag">Tag specified in the Inspector.</param>
        public static T FindByTag<T>(string tag) where T : MonoBehaviour
        {
            return GameObject.FindGameObjectWithTag(tag).GetComponent<T>();
        }

        private static readonly Dictionary<float, WaitForSeconds> WaitDelay = new Dictionary<float, WaitForSeconds>();
        /// <summary>
        /// Container that stores/reuses newly created WaitForSeconds.
        /// </summary>
        /// <param name="time">time(s) to wait</param>
        /// <returns>new WaitForSeconds</returns>
        public static WaitForSeconds GetWaitForSeconds(float time)
        {
            if (WaitDelay.TryGetValue(time, out var waitForSeconds)) return waitForSeconds;

            WaitDelay[time] = new WaitForSeconds(time);

            return WaitDelay[time];
        }


        private static PointerEventData _eventDataCurrentPosition;

        private static readonly List<RaycastResult> RaycastResults = new List<RaycastResult>();
        /// <summary>
        /// Checks if the mouse/pointer is over a UI element.
        /// </summary>
        /// <returns>true if the pointer is over a UI element</returns>
        public static bool IsPointerOverUI()
        {
            _eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };

            EventSystem.current.RaycastAll(_eventDataCurrentPosition, RaycastResults);

            return RaycastResults.Count > 0;
        }

        /// <summary>
        /// Gets the world position of a canvas element, can be used to spawn a 3d element in the 2d canvas
        /// </summary>
        /// <param name="rectTransform"> Canvas element(ui elements) </param>
        /// <returns>The position of the canvas element in world space</returns>
        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform rectTransform)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, rectTransform.position, CameraMain, out var output);

            return output;
        }

        /// <summary>
        /// Checks if an object with collider is present in the specified <paramref name="spawnPoint"/>.
        /// </summary>
        /// <param name="spawnPoint">Position to check for object's presence.</param>
        /// <param name="objects">Objects(with collider) to detect or include during check.</param>
        /// <param name="radius">Objects within this range would be accounted for.</param>
        /// <returns>True if an object with collider is present otherwise false.</returns>
        public static bool CheckColliderPresent(Vector3 spawnPoint, LayerMask objects, float radius = 1.5f)
        {
            var hitColliders = Physics.OverlapSphere(spawnPoint, radius, objects);

            return hitColliders.Length > 0;
        }


        /// <summary>
        /// Shuffles an array of strings
        /// </summary>
        /// <param name="texts">String Array to Shuffle</param>
        /// <returns>Shuffled string[]</returns>
        public static string[] RandomizeTexts(string[] texts)
        {
            return texts.OrderBy(x => new Random().Next())
                              .ToArray();
        }

        /// <summary>
        /// Converts a millisecond value to seconds
        /// </summary>
        /// <param name="value">Millisecond value</param>
        public static int ConvertMsToSeconds(float value)
        {
            int timeMultiplier = 1000;

            return (int)(value * timeMultiplier);
        }

        public static int GetAnimId(string id)
        {
            return Animator.StringToHash(id);
        }
    }
}