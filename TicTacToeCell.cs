using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class TicTacToeCellUI : MonoBehaviour, IPointerClickHandler
{
    [Header("Hücre Koordinatları (0-2)")]
    public int cellX; 
    public int cellY; 

    [Header("Hücre Image Bileşeni")]
    public Image cellImage; 

    private bool isFilled = false;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isFilled)
        {
            isFilled = true;
            TicTacToeManagerUI.Instance.CellClicked(cellX, cellY, this);
        }
    }


    public void SetSpriteAnimated(Sprite newSprite)
    {
        cellImage.sprite = newSprite;
        cellImage.color = Color.white;  
        cellImage.fillAmount = 0f;       
        StartCoroutine(AnimateFill(cellImage, 0.5f)); 
    }


    private IEnumerator AnimateFill(Image img, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            img.fillAmount = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
        img.fillAmount = 1f;
    }


    public void ResetCell()
    {
        isFilled = false;
        cellImage.sprite = null;
        cellImage.color = new Color(1, 1, 1, 0); // Tekrar şeffaf yap
    }
}