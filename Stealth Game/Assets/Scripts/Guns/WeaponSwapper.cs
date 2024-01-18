using UnityEngine;

public class WeaponSwapper : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private Gun activeGun;
    private Gun[] guns;

    private int lastSwapValue;
    private float holsterTimer;

    private int gunIndex = 0;
    private int nextIndex;
    private bool swapped;
    private float swapValue;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        guns = GetComponentsInChildren<Gun>(true);

        foreach (Gun gun in guns)
        {
            gun.gameObject.SetActive(false);
        }
        activeGun = guns[0];
        activeGun.gameObject.SetActive(true);

        inputActions.Player.AlphaSwapping.performed += AlphaSwapping_Performed;
        inputActions.Player.WeaponSwap.performed += WeaponSwap_performed;
    }

    private void Update()
    {
        if(holsterTimer > 0)
        {
            holsterTimer -= Time.deltaTime;
        }
        else if(!swapped)
        {
            SwapTo(nextIndex);
            gunIndex = nextIndex;
        }
    }

    private void WeaponSwap_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        swapValue = Mathf.Clamp(obj.ReadValue<float>(), -1, 1);

        if (swapValue == 0) return;

        holsterTimer = activeGun.holsterTime;
        nextIndex = Mathf.Abs((int)(gunIndex + swapValue)) % guns.Length;
        swapped = false;
    }

    private void AlphaSwapping_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        float swapValue = obj.ReadValue<float>();

        holsterTimer = activeGun.holsterTime;
        nextIndex = (int)swapValue - 1;
        swapped = false;
    }

    private void SwapTo(int index)
    {
        activeGun.gameObject.SetActive(false);

        activeGun = guns[index];
        activeGun.gameObject.SetActive(true);
        swapped = true;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
