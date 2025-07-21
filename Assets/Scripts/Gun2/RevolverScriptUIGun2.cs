using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RevolverScriptUIGun2 : MonoBehaviour
{
    public Texture[] ammoFrames;
    public RawImage ammoImage;
    public Text reloadingText;

    public int frameIndex = 0;
    private bool isAnimating = false;

    public bool isEmptyGun = true;

    void Start()
    {
        frameIndex = 0;
        ammoImage.texture = ammoFrames[frameIndex];
        reloadingText.text = "";
    }

    public void OnClickAdvanceAmmo()
    {
        if (frameIndex + 3 <= ammoFrames.Length)
        {
            StartCoroutine(PlayDisparo());

        }
        else if (!isAnimating)
        {
            isEmptyGun = false;
            reloadingText.text = "Recargando...";
            Invoke("ResetAmmoUI", 2f);
        }
    }

    IEnumerator PlayDisparo()
    {
        isAnimating = true;

        for (int i = 0; i < 3 && frameIndex < ammoFrames.Length; i++)
        {
            ammoImage.texture = ammoFrames[frameIndex];
            frameIndex++;
            yield return new WaitForSeconds(0.1f);
        }

        isAnimating = false;
    }

    public void ResetAmmoUI()
    {
        isEmptyGun = true;
        frameIndex = 0;
        ammoImage.texture = ammoFrames[frameIndex];
        reloadingText.text = "";
    }


}
