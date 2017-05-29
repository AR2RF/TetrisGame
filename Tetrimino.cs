using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetrimino : MonoBehaviour
{
    private float lastFall = 0;

    private GameController gameController;
    private MusicManager musicManager;
    private InterfaceManager interfaceManager;

    private float continuousVerticalSpeed = 0.05f;
    private float continuousHoriontalSpeed = 0.1f;
    private float buttonDownWaitMax = 0.2f;

    private float verticalTimer = 0;
    private float horizontalTimer = 0;
    private float buttonDownWaitTimerHorizontal = 0;
    private float buttonDownWaitTimerVertical = 0;

    private bool movedImmediateHorizontal = false;
    private bool movedImmediateVertical = false;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        musicManager = FindObjectOfType<MusicManager>();
        interfaceManager = GameObject.FindGameObjectWithTag("InterfaceManager").GetComponent<InterfaceManager>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            movedImmediateHorizontal = false;
            horizontalTimer = 0;           
            buttonDownWaitTimerHorizontal = 0;
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            movedImmediateVertical = false;
            verticalTimer = 0;
            buttonDownWaitTimerVertical = 0;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateRight();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RotateLeft();
        }

        if (Input.GetKey(KeyCode.DownArrow) || Time.time - lastFall >= gameController.fallSpeed)
        {
            MoveDown();
        }
    }

    public void MoveLeft()
    {
        if (movedImmediateHorizontal)
        {
            if (buttonDownWaitTimerHorizontal < buttonDownWaitMax)
            {
                buttonDownWaitTimerHorizontal += Time.deltaTime;
                return;
            }

            if (horizontalTimer < continuousHoriontalSpeed)
            {
                horizontalTimer += Time.deltaTime;
                return;
            }
        }

        if (!movedImmediateHorizontal)
        {
            movedImmediateHorizontal = true;
        }

        horizontalTimer = 0;

        transform.position += new Vector3(-1, 0, 0);

        if (CheckIsValidGridPosition())
        {
            UpdateGrid();

            if (musicManager)
                musicManager.PlaySound(musicManager.moveSound);
        }
        else
            transform.position += new Vector3(1, 0, 0);
    }

    public void MoveRight()
    {
        if (movedImmediateHorizontal)
        {
            if (buttonDownWaitTimerHorizontal < buttonDownWaitMax)
            {
                buttonDownWaitTimerHorizontal += Time.deltaTime;
                return;
            }

            if (horizontalTimer < continuousHoriontalSpeed)
            {
                horizontalTimer += Time.deltaTime;
                return;
            }
        }

        if (!movedImmediateHorizontal)
        {
            movedImmediateHorizontal = true;
        }

        horizontalTimer = 0;

        transform.position += new Vector3(1, 0, 0);

        if (CheckIsValidGridPosition())
        {
            UpdateGrid();

            if (musicManager)
                musicManager.PlaySound(musicManager.moveSound);
        }
        else
            transform.position += new Vector3(-1, 0, 0);
    }

    public void MoveDown()
    {
        if (movedImmediateVertical)
        {
            if (buttonDownWaitTimerVertical < buttonDownWaitMax)
            {
                buttonDownWaitTimerVertical += Time.deltaTime;
                return;
            }

            if (verticalTimer < continuousVerticalSpeed)
            {
                verticalTimer += Time.deltaTime;
                return;
            }
        }

        if (!movedImmediateVertical)
        {
            movedImmediateVertical = true;
        }

        verticalTimer = 0;

        transform.position += new Vector3(0, -1, 0);

        if (CheckIsValidGridPosition())
        {
            UpdateGrid();

            if (musicManager)
                musicManager.PlaySound(musicManager.moveSound);
        }
        else
        {
            transform.position += new Vector3(0, 1, 0);

            gameController.DeleteFullRows();

            if (CheckIsAboveGrid())
            {
                gameController.GameOver();
            }

            enabled = false;

            gameController.SpawnNext();

            ScoreZeroLine();
        }

        lastFall = Time.time;
    }

    public void RotateLeft()
    {
        transform.Rotate(0, 0, 90);

        if (CheckIsValidGridPosition())
        {
            UpdateGrid();

            if (musicManager)
                musicManager.PlaySound(musicManager.moveSound);
        }
        else
            transform.Rotate(0, 0, -90);
    }

    public void RotateRight()
    {
        transform.Rotate(0, 0, -90);

        if (CheckIsValidGridPosition())
        {
            UpdateGrid();

            if (musicManager)
                musicManager.PlaySound(musicManager.moveSound);

        }
        else
            transform.Rotate(0, 0, 90);
    }

    public bool CheckIsValidGridPosition()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = gameController.RoundVec2(child.position);

            if (!gameController.CheckIsInsideBorders(v))
                return false;

            if (gameController.GetTransformAtGridPos(v) != null && gameController.GetTransformAtGridPos(v).parent != transform)
                return false;
        }
        return true;
    }

    public void UpdateGrid()
    {
        for (int y = 0; y < gameController.gridHeight; ++y)
        {
            for (int x = 0; x < gameController.gridWidth; ++x)
            {
                if (gameController.grid[x, y] != null)
                {
                    if (gameController.grid[x, y].parent == transform)
                    {
                        gameController.grid[x, y] = null;
                    }
                }
            }
        }

        foreach (Transform child in transform)
        {
            Vector2 v = gameController.RoundVec2(child.position);

            if (v.y < gameController.gridHeight)
                gameController.grid[(int)v.x, (int)v.y] = child;
        }
    }

    public bool CheckIsAboveGrid()
    {
        for (int x = 0; x < gameController.gridWidth; ++x)
        {
            foreach (Transform child in transform)
            {
                Vector2 v = gameController.RoundVec2(child.position);

                if (v.y > gameController.gridHeight - 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void ScoreZeroLine()
    {
        gameController.currentScore += (gameController.scoreZeroLine * gameController.currentLevel);
        interfaceManager.score.text = gameController.currentScore.ToString();

        if (gameController.highScore < gameController.currentScore)
        {
            gameController.highScore = gameController.currentScore;
            interfaceManager.highScore.text = gameController.highScore.ToString();
        }
    }
}