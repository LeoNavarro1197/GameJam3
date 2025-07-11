using UnityEngine;

public class AmmoRaycastScript : MonoBehaviour
{
    public RevolverScriptUI ammoUI; // Referencia al script que controla la animación

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ammoUI.OnClickAdvanceAmmo();
        }
    }
}