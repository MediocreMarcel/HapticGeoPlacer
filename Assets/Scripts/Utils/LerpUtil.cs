using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpUtil : MonoBehaviour
{
    /// <summary>
    /// Creates a Lerp Effect on a the position of an game Object
    /// </summary>
    /// <param name="gameObjectToModify">Object that should get the effect</param>
    /// <param name="targetPosition">Target position that the object should get</param>
    /// <param name="lerpDuration">Time the lerp effect should take</param>
    public IEnumerator LerpPosition(GameObject gameObjectToModify, Vector3 targetPosition, float lerpDuration)
    {
        float lerpTime = 0.0f;
        Vector3 startPosition = gameObjectToModify.transform.position;
        while (lerpTime < lerpDuration)
        {
            gameObjectToModify.transform.position = Vector3.Lerp(startPosition, targetPosition, lerpTime / lerpDuration);
            lerpTime += Time.deltaTime;
            yield return null;
        }
        gameObjectToModify.transform.position = targetPosition;
    }

    /// <summary>
    /// Creates a Lerp Effect on a the local position of an game Object
    /// </summary>
    /// <param name="gameObjectToModify">Object that should get the effect</param>
    /// <param name="targetPosition">Target position that the object should get</param>
    /// <param name="lerpDuration">Time the lerp effect should take</param>
    public IEnumerator LerpLocalPosition(GameObject gameObjectToModify, Vector3 targetPosition, float lerpDuration)
    {
        float lerpTime = 0.0f;
        Vector3 startPosition = gameObjectToModify.transform.localPosition;
        while (lerpTime < lerpDuration)
        {
            gameObjectToModify.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, lerpTime / lerpDuration);
            lerpTime += Time.deltaTime;
            yield return null;
        }
        gameObjectToModify.transform.localPosition = targetPosition;
    }

    /// <summary>
    /// Creates a Lerp Effect on a the rotation of an game Object
    /// </summary>
    /// <param name="gameObjectToModify">Object that should get the effect</param>
    /// <param name="targetPosition">Target rotation that the object should get</param>
    /// <param name="lerpDuration">Time the lerp effect should take</param>
    public IEnumerator LerpRotation(GameObject gameObjectToModify, Quaternion targetRotation, float lerpDuration)
    {
        float lerpTime = 0.0f;
        Quaternion startPosition = gameObjectToModify.transform.rotation;
        while (lerpTime < lerpDuration)
        {
            gameObjectToModify.transform.rotation = Quaternion.Lerp(startPosition, targetRotation, lerpTime / lerpDuration);
            lerpTime += Time.deltaTime;
            yield return null;
        }
        gameObjectToModify.transform.rotation = targetRotation;
    }
}
