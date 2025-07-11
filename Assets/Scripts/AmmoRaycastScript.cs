using UnityEngine;

public class AmmoRaycastScript : MonoBehaviour
{
    public RevolverScriptUI ammoUI; // Referencia al script que controla la animaci�n

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ammoUI.OnClickAdvanceAmmo();
        }
    }
}