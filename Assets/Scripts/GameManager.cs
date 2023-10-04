using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isCubeTurn = true;
    public TextMeshProUGUI label;
    public Cell[] cells;
    public GameObject restartButton;
    public GameObject backToMenuButton;
    // Start is called before the first frame update
    public AudioClip clipWin;
    public AudioClip clipDraw;
    private bool modeAI;
    void Start()
    {
        ChangeTurn();
        restartButton.SetActive(false);
        backToMenuButton.SetActive(false);
        int flag = PlayerPrefs.GetInt("AI", 1);
        modeAI = flag == 1;
        
    }

    // Suponiendo que las cells se disponen de la siguiente forma:
    // 0 | 1 | 2
    // 3 | 4 | 5
    // 6 | 7 | 8

    public void CheckWinner()
    {
        bool isDraw = true;

        // Revisa las filas
        for (int i = 0; i < 9; i += 3)
        {
            if (cells[i].status != CellType.EMPTY && cells[i].status == cells[i + 1].status && cells[i + 1].status == cells[i + 2].status)
            {
                DeclareWinner(cells[i].status);
                return;
            }
            if (cells[i].status == CellType.EMPTY || cells[i + 1].status == CellType.EMPTY || cells[i + 2].status == CellType.EMPTY) isDraw = false;
        }

        // Revisa las columnas
        for (int i = 0; i < 3; i++)
        {
            if (cells[i].status != 0 && cells[i].status == cells[i + 3].status && cells[i + 3].status == cells[i + 6].status)
            {
                DeclareWinner(cells[i].status);
                return;
            }
        }

        // Revisa las diagonales
        if (cells[0].status != 0 && cells[0].status == cells[4].status && cells[4].status == cells[8].status)
        {
            DeclareWinner(cells[0].status);
            return;
        }

        if (cells[2].status != 0 && cells[2].status == cells[4].status && cells[4].status == cells[6].status)
        {
            DeclareWinner(cells[2].status);
            return;
        }

        // Si todas las celdas están llenas y no hay ganador, entonces es un empate.
        if (isDraw)
        {
            label.text = "It's a draw!";
            SetupGameFinished(false);
            
        }
    }

    public void ChangeTurn()
    {
        isCubeTurn = !isCubeTurn;
        if (isCubeTurn)
        {
            label.text = "Cube's turn...";   
        }
        else
        {
            label.text = "Sphere's turn...";   
        }
    }

    void DeclareWinner(CellType status)
    {
        if (status == CellType.SPHERE)
        {
            label.text = "Sphere is the winner";   
        }
        else
        {
            label.text = "Cube is the winner";   
        }

        SetupGameFinished(true);
        
    }

    
    
    // Update is called once per frame
    void Update()
    {
        if (modeAI && isCubeTurn)
        {
            Debug.Log("toca a la IA");
            foreach (Cell cell in cells)
            {
                if (cell.status == CellType.EMPTY)
                {
                    cell.onClick();
                    break;
                }
            }
        }
    }

    public void RestartGame()
    {
        Debug.Log("RESTART");
        SceneManager.LoadScene(1);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void SetupGameFinished(bool winner)
    {
        restartButton.SetActive(true);
        backToMenuButton.SetActive(true);
        if (winner)
        {
            GetComponent<AudioSource>().PlayOneShot(clipWin);
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(clipDraw);    
        }
        
    }
}
