using UnityEngine;
using System.Collections;

public class MovingCamera : MonoBehaviour
{
    [Header("Camera Movement")]
    [SerializeField] private Transform destination;
    [SerializeField] private Transform lookTarget;
    [SerializeField] private float duration = 3f;

    private Coroutine movementRoutine;

    private void Start()
    {
        
    }
    public void MoveAndLookAt()
    {
        if (destination == null || lookTarget == null)
        {
            Debug.LogWarning(
                "CameraMovement: Destination or Look Target is missing."
            );
            return;
        }

        if (movementRoutine != null)
        {
            StopCoroutine(movementRoutine);
        }

        movementRoutine = StartCoroutine(MoveCamera());
    }

    private IEnumerator MoveCamera()
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        float elapsed = 0f;

        if (duration <= 0f)
        {
            transform.position = destination.position;
            transform.LookAt(lookTarget.position);
            movementRoutine = null;
            yield break;
        }

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / duration);

            // Smooth movement.
            t = t * t * (3f - 2f * t);

            transform.position = Vector3.Lerp(
                startPosition,
                destination.position,
                t
            );

            Vector3 direction = lookTarget.position - transform.position;

            if (direction.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRotation =Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(
                    startRotation,
                    targetRotation,
                    t
                );
            }

            yield return null;
        }

        transform.position = destination.position;

        Vector3 finalDirection = lookTarget.position - transform.position;

        if (finalDirection.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(finalDirection);
        }

        movementRoutine = null;
    }
}
