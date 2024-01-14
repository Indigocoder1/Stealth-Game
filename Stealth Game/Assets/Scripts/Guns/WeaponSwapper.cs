using UnityEngine;

public class WeaponSwapper : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private Gun activeGun;
    private Gun[] guns;

    private int lastScrollValue;
    private float holsterTimer;

    private int gunIndex = 0;
    private int nextIndex;
    private bool swapped;

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
    }

    private void Update()
    {
        float swapValue = inputActions.Player.WeaponSwap.ReadValue<float>();
        swapValue = Mathf.Clamp(swapValue, -1f, 1f);

        if(swapValue != lastScrollValue && swapValue != 0)
        {
            holsterTimer = activeGun.holsterTime;

            swapValue = Mathf.Clamp(swapValue, -1f, 1f);

            nextIndex = Mathf.Abs((int)(gunIndex + swapValue)) % guns.Length;
            swapped = false;
        }

        if(holsterTimer > 0)
        {
            holsterTimer -= Time.deltaTime;
        }
        else if(!swapped)
        {
            activeGun.gameObject.SetActive(false);
            gunIndex = nextIndex;

            activeGun = guns[gunIndex];
            activeGun.gameObject.SetActive(true);
            swapped = true;
        }

        lastScrollValue = (int)swapValue;
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
