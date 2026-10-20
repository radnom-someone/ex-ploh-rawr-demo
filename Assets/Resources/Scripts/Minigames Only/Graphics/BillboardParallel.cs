
using UnityEngine;

public class BillboardParallel : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool lockElementRotation;
    [SerializeField] private bool tiltToCamera;
    [SerializeField] private bool flipped;

    private Transform camTransform;
    private Camera currentCamera;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;

        UpdateCameraReference();
    }

    private void LateUpdate()
    {
        UpdateCameraReference();

        if (camTransform == null)
        {
            return;
        }

        UpdateBillboardRotation();
        UpdateFlip();
    }

    private void UpdateCameraReference()
    {
        Camera activeCamera = PlayerInteract.PLAYER_CAMERA;

        // Fallback when no camera has been assigned by PlayerInteract.
        if (activeCamera == null || !activeCamera.isActiveAndEnabled)
        {
            activeCamera = Camera.main;
        }

        // Only update the reference when the camera changes.
        if (activeCamera != null && activeCamera != currentCamera)
        {
            currentCamera = activeCamera;
            camTransform = currentCamera.transform;
        }
    }

    private void UpdateBillboardRotation()
    {
        if (lockElementRotation)
        {
            Vector3 targetPosition = camTransform.position;

            // Keep the sprite upright by ignoring vertical differences.
            targetPosition.y = transform.position.y;

            if (tiltToCamera)
            {
                Vector3 direction = targetPosition - transform.position;

                if (direction.sqrMagnitude > 0.001f)
                {
                    transform.rotation = Quaternion.LookRotation(direction);
                }
            }
            else
            {
                Vector3 cameraForward = camTransform.forward;
                cameraForward.y = 0f;

                if (cameraForward.sqrMagnitude > 0.001f)
                {
                    transform.rotation =
                        Quaternion.LookRotation(cameraForward);
                }
            }
        }
        else
        {
            // Match the active camera's orientation.
            transform.rotation = camTransform.rotation;
        }
    }

    private void UpdateFlip()
    {
        Vector3 scale = originalScale;

        if (flipped)
        {
            scale.x = -originalScale.x;
        }
        else
        {
            scale.x = originalScale.x;
        }

        transform.localScale = scale;
    }

    public void SetFlipped(bool shouldFlip)
    {
        flipped = shouldFlip;
    }
}
