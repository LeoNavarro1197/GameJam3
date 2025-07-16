using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RevolverScriptUI : MonoBehaviour
{
    public Texture[] ammoFrames;
    public RawImage ammoImage;
    public Text reloadingText;

    private int frameIndex = 0;
    private bool isAnimating = false;

    void Start()
    {
        // Inicializa estado
        frameIndex = 0;
        ammoImage.texture = ammoFrames[frameIndex];
        reloadingText.text = ""; // Oculta el texto de recarga
    }

    public void OnClickAdvanceAmmo()
    {
        if (frameIndex + 3 <= ammoFrames.Length)
        {
            StartCoroutine(PlayDisparo());
        }
        else if (!isAnimating)
        {
            reloadingText.text = "Reloading";
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
        frameIndex = 0;
        ammoImage.texture = ammoFrames[frameIndex];
        reloadingText.text = "";
    }


}
