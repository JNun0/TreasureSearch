using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    public MazeGenerator _mazeGenerator;

    private void Start()
    {
        _mazeGenerator = FindObjectOfType<MazeGenerator>();

        Button button = GetComponent<Button>();

        button.onClick.AddListener(ResetGame);
    }

    private void ResetGame()
    {
        _mazeGenerator.ResetGame();
    }
}
