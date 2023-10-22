using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isCubeTurn = true;
    public TextMeshProUGUI label;
    public TextMeshProUGUI cruces;
    public TextMeshProUGUI sferas;
    public GameObject cruz;
    public GameObject sfera;
    public Cell[] cells;
    public GameObject restartButton;
    public GameObject backToMenuButton;
    public AudioClip clipWin;
    public AudioClip clipDraw;
    private bool modeAI;
    private bool isAIThinking = false;
    public bool gameEnd = false;

    void Start()
    {
        int flag = PlayerPrefs.GetInt("AI", 1);
        modeAI = flag == 1;

        if (modeAI)
        {
            cruces.text = "x = Jugador local";
            sferas.text = "o = Tú";
            sfera.SetActive(true);
            cruz.SetActive(true);
            label.text = "Es tu turno";
        }
        else
        {
            sfera.SetActive(false);
            cruz.SetActive(false);
            ChangeTurn();
        }

        restartButton.SetActive(false);
        backToMenuButton.SetActive(false);

        // Inicio la IA desde el principio si está habilitada
        if (modeAI && isCubeTurn)
        {
            StartCoroutine(SimulateAITurn());
        }
    }


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
            if (cells[i].status != CellType.EMPTY && cells[i].status == cells[i + 3].status && cells[i + 3].status == cells[i + 6].status)
            {
                DeclareWinner(cells[i].status);
                return;
            }
        }

        // Revisa las diagonales
        if (cells[0].status != CellType.EMPTY && cells[0].status == cells[4].status && cells[4].status == cells[8].status)
        {
            DeclareWinner(cells[0].status);
            return;
        }

        if (cells[2].status != CellType.EMPTY && cells[2].status == cells[4].status && cells[4].status == cells[6].status)
        {
            DeclareWinner(cells[2].status);
            return;
        }

        // Si todas las celdas están llenas y no hay ganador, entonces es un empate.
        if (isDraw)
        {
            label.text = "¡Es un empate!";
            SetupGameFinished(false);
            gameEnd = true;
        }
    }

    public void ChangeTurn()
    {
        isCubeTurn = !isCubeTurn;
        if (modeAI)
        {
            if (isCubeTurn)
            {
                label.text = "Es el turno del jugador local";
            }
            else
            {
                label.text = "Es tu turno";
            }
        }
        else
        {
            if (isCubeTurn)
            {
                label.text = "Es el turno de las cruces";
            }
            else
            {
                label.text = "Es el turno de las esferas";
            }
        }

        // Verificar si es el turno de la IA y la IA no está pensando
        if (modeAI && isCubeTurn && !isAIThinking)
        {
            Debug.Log("Turno de la IA");
            StartCoroutine(SimulateAITurn());
        }
    }

    void DeclareWinner(CellType status)
    {
        if (status == CellType.SPHERE)
        {
            label.text = "Ganan las esferas!";
        }
        else
        {
            label.text = "Ganan las cruces!";
        }

        SetupGameFinished(true);
        gameEnd = true;
    }
    IEnumerator SimulateAITurn()
    {
        isAIThinking = true;

        yield return new WaitForSeconds(3f);

        List<Cell> emptyCells = new List<Cell>();

        foreach (Cell cell in cells)
        {
            if (cell.status == CellType.EMPTY)
            {
                emptyCells.Add(cell);
            }
        }

        if (emptyCells.Count > 0)
        {
            int randomIndex = Random.Range(0, emptyCells.Count);
            Cell randomCell = emptyCells[randomIndex];
            randomCell.onClick();
        }

        isAIThinking = false;
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
