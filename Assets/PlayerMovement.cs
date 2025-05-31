using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Look Settings")]
    [Range(0.1f, 100f)]
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;
    public TextMeshProUGUI sensitivityDisplay;

    [Header("Gun")]
    public Gun equippedGun;

    private PlayerInput input;
    private InputAction lookAction, aimAction, fireAction, moveAction;

    private float verticalLookRotation;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
        lookAction = input.actions["Look"];
        aimAction = input.actions["Aim"];
        fireAction = input.actions["Attack"];
        moveAction = input.actions["Move"];

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLook();
        HandleGunInput();
        HandleSensitivityAdjust();
        UpdateSensitivityDisplay();
    }

    void HandleLook()
    {
        Vector2 mouseDelta = lookAction.ReadValue<Vector2>();
        transform.Rotate(Vector3.up * (mouseDelta.x * mouseSensitivity * Time.deltaTime));

        verticalLookRotation -= mouseDelta.y * mouseSensitivity * Time.deltaTime;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);

        if (equippedGun != null)
            equippedGun.UpdateLookInput(mouseDelta);
    }

    void HandleGunInput()
    {
        if (equippedGun == null) return;

        equippedGun.SetAiming(aimAction.IsPressed());
        equippedGun.SetSprinting(false);

        if (fireAction.IsPressed())
            equippedGun.TryShoot();
    }

    void HandleSensitivityAdjust()
    {
        float verticalInput = moveAction.ReadValue<Vector2>().y;

        if (Mathf.Abs(verticalInput) > 0.1f)
        {
            mouseSensitivity += verticalInput * 2f * Time.deltaTime;
            mouseSensitivity = Mathf.Clamp(mouseSensitivity, 0.1f, 100f);
        }
    }

    void UpdateSensitivityDisplay()
    {
        if (sensitivityDisplay != null)
            sensitivityDisplay.text = $"Sensitivity: {mouseSensitivity:F2}";
    }

    public void SetMouseSensitivity(float newSensitivity)
    {
        mouseSensitivity = newSensitivity;
    }
}
