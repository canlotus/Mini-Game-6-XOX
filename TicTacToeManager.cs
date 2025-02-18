using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TicTacToeManagerUI : MonoBehaviour
{
    public static TicTacToeManagerUI Instance;

    [Header("Oyuncu Sprite'ları")]
    public Sprite xSprite; 
    public Sprite oSprite; 

    [Header("UI Referansı")]
    public TextMeshProUGUI winText; 

    [Header("Win Line UI Görselleri")]
    public GameObject winLineDiagonalLeft;  
    public GameObject winLineDiagonalRight; 
    public GameObject winLineHorizontal0;   
    public GameObject winLineHorizontal1;   
    public GameObject winLineHorizontal2;  
    public GameObject winLineVertical0;    
    public GameObject winLineVertical1;  
    public GameObject winLineVertical2;  

    [Header("Ses Efektleri")]
    public AudioClip xSound;  
    public AudioClip oSound; 
    public AudioSource audioSource;

    private bool xTurn = true;     
    private int[,] board = new int[3, 3]; 
    private bool gameOver = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        ResetBoardArray();
        winText.text = "";
        HideWinLines();
    }


    public void CellClicked(int cellX, int cellY, TicTacToeCellUI cell)
    {
        if (gameOver) return;   
        if (board[cellX, cellY] != 0) return; 

        int currentPlayerValue = xTurn ? 1 : 2;
        board[cellX, cellY] = currentPlayerValue;

        Sprite spriteToUse = xTurn ? xSprite : oSprite;
        cell.SetSpriteAnimated(spriteToUse);

        AudioClip clip = xTurn ? xSound : oSound;
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }

        if (CheckWinCondition(currentPlayerValue))
        {
            winText.text = xTurn ? "X Wins!" : "O Wins!";
            gameOver = true;
            ShowWinningLine(currentPlayerValue);
        }
        else
        {
            xTurn = !xTurn; 
        }
    }

    private bool CheckWinCondition(int playerValue)
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] == playerValue && board[i, 1] == playerValue && board[i, 2] == playerValue)
                return true;
        }
        for (int j = 0; j < 3; j++)
        {
            if (board[0, j] == playerValue && board[1, j] == playerValue && board[2, j] == playerValue)
                return true;
        }
        if (board[0, 0] == playerValue && board[1, 1] == playerValue && board[2, 2] == playerValue)
            return true;
        if (board[0, 2] == playerValue && board[1, 1] == playerValue && board[2, 0] == playerValue)
            return true;

        return false;
    }


    private void ShowWinningLine(int playerValue)
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] == playerValue && board[i, 1] == playerValue && board[i, 2] == playerValue)
            {
                if (i == 0) ActivateAndAnimateLine(winLineHorizontal0);
                else if (i == 1) ActivateAndAnimateLine(winLineHorizontal1);
                else if (i == 2) ActivateAndAnimateLine(winLineHorizontal2);
                return;
            }
        }
        for (int j = 0; j < 3; j++)
        {
            if (board[0, j] == playerValue && board[1, j] == playerValue && board[2, j] == playerValue)
            {
                if (j == 0) ActivateAndAnimateLine(winLineVertical0);
                else if (j == 1) ActivateAndAnimateLine(winLineVertical1);
                else if (j == 2) ActivateAndAnimateLine(winLineVertical2);
                return;
            }
        }
        if (board[0, 0] == playerValue && board[1, 1] == playerValue && board[2, 2] == playerValue)
        {
            ActivateAndAnimateLine(winLineDiagonalLeft);
            return;
        }
        if (board[0, 2] == playerValue && board[1, 1] == playerValue && board[2, 0] == playerValue)
        {
            ActivateAndAnimateLine(winLineDiagonalRight);
            return;
        }
    }


    private void ActivateAndAnimateLine(GameObject winLine)
    {
        winLine.SetActive(true);
        Image img = winLine.GetComponent<Image>();
        if (img != null)
        {
            img.fillAmount = 0f;
            StartCoroutine(AnimateFill(img, 0.5f)); 
        }
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


    public void ResetGame()
    {
        ResetBoardArray();


        TicTacToeCellUI[] cells = FindObjectsOfType<TicTacToeCellUI>();
        foreach (TicTacToeCellUI cell in cells)
        {
            cell.ResetCell();
        }

        winText.text = "";
        xTurn = true;
        gameOver = false;
        HideWinLines();
    }


    private void ResetBoardArray()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                board[i, j] = 0;
    }


    private void HideWinLines()
    {
        if (winLineDiagonalLeft) winLineDiagonalLeft.SetActive(false);
        if (winLineDiagonalRight) winLineDiagonalRight.SetActive(false);
        if (winLineHorizontal0) winLineHorizontal0.SetActive(false);
        if (winLineHorizontal1) winLineHorizontal1.SetActive(false);
        if (winLineHorizontal2) winLineHorizontal2.SetActive(false);
        if (winLineVertical0) winLineVertical0.SetActive(false);
        if (winLineVertical1) winLineVertical1.SetActive(false);
        if (winLineVertical2) winLineVertical2.SetActive(false);
    }

    public void BackMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}