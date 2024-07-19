using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float MovementSpeed = 10f;
    private const float MovementSprintSpeed = 20f;

    private const float minHeight = 3f;
    private const float maxHeight = 20f;
    private const float zoomSpeed = 1f;

    [SerializeField] private Transform target;

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        var inputVector = InputManager.Instance.GetNormalizedMoveVector();
        var isSpriting = InputManager.Instance.IsSprinting();
        var zoom = InputManager.Instance.GetZoom();

        var moveDir = Vector3Extensions.WithXZ(inputVector);
        var zoomAmount = zoom * zoomSpeed * Time.deltaTime;

        if (moveDir == Vector3.zero && zoomAmount == 0)
            return;

        var speed = isSpriting ? MovementSprintSpeed : MovementSpeed; // TODO: speed should be based on height
        var moveOffset = moveDir * (speed * Time.deltaTime);
        var newHeight = Mathf.Clamp(target.position.y - zoomAmount, minHeight, maxHeight);

        // Apply movement
        var newPosition = new Vector3(target.position.x, newHeight, target.position.z) + moveOffset;
        target.position = newPosition;
    }
}
