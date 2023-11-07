using UnityEngine;
using UnityEngine.UI;

public class BillboardDisplay : MonoBehaviour
{
    [SerializeField] private RawImage billboardImage;
    [SerializeField] private RawImage overlay;

    float alpha;
    float duration = 2;

    private void Start()
    {
        overlay.color = Color.clear;
    }

    public void TransitionBillboardTexture(Texture2D texture)
    {
        billboardImage.texture = texture;
        overlay.color = Color.white;
        alpha = 1.0f;
    }

    private void Update()
    {
        alpha -= Time.deltaTime / duration;

        overlay.color = new Color(1, 1, 1, alpha);
    }
}