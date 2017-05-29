using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject[] tetriminos;

    public int gridWidth = 10;
    public int gridHeight = 24;

    public float fallSpeed = 1.0f;

    public Transform[,] grid;

    public int scoreZeroLine = 5;
    public int scoreOneLine = 20;
    public int scoreTwoLine = 50;
    public int scoreThreeLine = 125;
    public int scoreFourLine = 400;

    private int numberOfRowsThisTurn = 0;

    public int currentScore = 0;
    public int highScore = 0;
    public int currentLevel = 1;
    private int clearedLines = 0;

    public int levelUp = 30;

    public InterfaceManager interfaceManager;
    private MusicManager musicManager;

    private GameObject previewTetrimino;
    private GameObject nextTetrimino;

    private bool gameStarted = false;

    private Vector2 previewTetraminoPosition = new Vector2(16.75f, 10.5f);

    private void Awake()
    {
        musicManager = FindObjectOfType<MusicManager>();

        grid = new Transform[gridWidth, gridHeight];

        LoadScore();

        currentScore = 0;
    }

    private void Start()
    {
        SpawnNext();
    }

    private void Update()
    {
        UpdateScore();
    }

    public void SpawnNext()
    {
        if (!gameStarted)
        {
            gameStarted = true;

            nextTetrimino = Instantiate(tetriminos[RandomTetrimino()], new Vector2(5.0f, 20.0f), Quaternion.identity);
            nextTetrimino.transform.SetParent(transform);

            previewTetrimino = Instantiate(tetriminos[RandomTetrimino()], previewTetraminoPosition, Quaternion.identity);
            previewTetrimino.transform.SetParent(transform);
            previewTetrimino.GetComponent<Tetrimino>().enabled = false;
        }
        else
        {
            previewTetrimino.transform.localPosition = new Vector2(5.0f, 20.0f);
            nextTetrimino = previewTetrimino;
            nextTetrimino.GetComponent<Tetrimino>().enabled = true;

            previewTetrimino = Instantiate(tetriminos[RandomTetrimino()], previewTetraminoPosition, Quaternion.identity);
            previewTetrimino.transform.SetParent(transform);
            previewTetrimino.GetComponent<Tetrimino>().enabled = false;
        }
    }

    private int RandomTetrimino()
    {
        int i = Random.Range(0, tetriminos.Length);
        return i;
    }

    private void UpdateLevel()
    {
        if (clearedLines >= levelUp)
        {
            currentLevel++;
            interfaceManager.level.text = currentLevel.ToString();

            if (musicManager)
            {
                musicManager.PlaySound(musicManager.LevelUpSound);

                if (musicManager.musicPlayer.pitch < 1.05f)
                {
                    musicManager.musicPlayer.pitch += 0.01f;
                }
            }

            clearedLines = 0;

            UpdateSpeed();
        }
    }

    private void UpdateSpeed()
    {
        fallSpeed = fallSpeed - (fallSpeed / 5f);
    }

    private void UpdateScore()
    {
        if (numberOfRowsThisTurn > 0)
        {
            if (numberOfRowsThisTurn == 1)
            {
                ClearedOneLine();
            }
            else if (numberOfRowsThisTurn == 2)
            {
                ClearedTwoLines();
            }
            else if (numberOfRowsThisTurn == 3)
            {
                ClearedThreeLines();
            }
            else if (numberOfRowsThisTurn == 4)
            {
                ClearedFourLines();
            }

            if (musicManager)
                musicManager.PlaySound(musicManager.lineClearedSound);

            interfaceManager.score.text = currentScore.ToString();

            if (highScore < currentScore)
            {
                highScore = currentScore;
                interfaceManager.highScore.text = highScore.ToString();
            }

            UpdateLevel();

            numberOfRowsThisTurn = 0;
        }
    }

    private void ClearedOneLine()
    {
        currentScore += scoreOneLine * currentLevel;
    }

    private void ClearedTwoLines()
    {
        currentScore += scoreTwoLine * currentLevel;
    }

    private void ClearedThreeLines()
    {
        currentScore += scoreThreeLine * currentLevel;
    }

    private void ClearedFourLines()
    {
        currentScore += scoreFourLine * currentLevel;
    }

    public Vector2 RoundVec2(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    public bool CheckIsInsideBorders(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && pos.y >= 0);
    }

    public Transform GetTransformAtGridPos(Vector2 pos)
    {
        if (pos.y > gridHeight - 1)
        {
            return null;
        }
        else
        {
            return grid[(int)pos.x, (int)pos.y];
        }
    }

    public void DeleteRow(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public void MoveRowDown(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public void MoveAllRowsDown(int y)
    {
        for (int i = y; i < gridHeight; ++i)
        {
            MoveRowDown(i);
        }
    }

    public bool CheckIsFullRowAt(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }

        numberOfRowsThisTurn++;

        clearedLines++;

        return true;
    }

    public void DeleteFullRows()
    {
        for (int y = 0; y < gridHeight; ++y)
        {
            if (CheckIsFullRowAt(y))
            {
                DeleteRow(y);

                MoveAllRowsDown(y + 1);
                --y;
            }
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("Score", currentScore);
        PlayerPrefs.SetInt("HighScore", highScore);
    }

    private void LoadScore()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetInt("HighScore");
        }
    }
}
