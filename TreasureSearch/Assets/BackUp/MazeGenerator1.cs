using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator1 : MonoBehaviour
{
    [SerializeField]
    private GameObject _capsulePrefab;

    [SerializeField]
    private MazeCell _mazeCellPrefab;

    [SerializeField]
    private int _mazeWidth;

    [SerializeField]
    private int _mazeDepth;

    private MazeCell[,] _mazeGrid;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];
        
        //Percorre a largura
        for (int x = 0; x < _mazeWidth; x++)
        {
            //Percorre a profundidade
            for (int z = 0; z < _mazeDepth; z++)
            {
                //Cria a base do labirinto
                _mazeGrid[x,z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }

        //Não há celula anterior por isso mete-se null e a primeira celula como currentcell
        yield return GenerateMaze(null, _mazeGrid[0, 0]);
    }

    private IEnumerator GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        //Torna a primeira celula visivel
        currentCell.Visit();
        ClearWalls(previousCell,currentCell);

        yield return new WaitForSeconds(0.05f);

        MazeCell nextCell;

        //Vai de celula em celula ate nao haver celulas por visitar
        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                yield return GenerateMaze(currentCell, nextCell);
            }
        }while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var univisitedCells = GetUnvisitedCells(currentCell);

        //Escolhe um celula aleatoria das vistas
        return univisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    //Visita as celulas vizinhas todas
    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        //Os valores correspondem ao index da celula na grid
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        //Ve se a celula a direita esta dentro da grid
        if (x + 1 < _mazeWidth)
        {
            //Se sim vê se a celula já foi visitada
            var cellToRight = _mazeGrid[x + 1, z];

            if (cellToRight.IsVisited == false)
            {
                yield return cellToRight;
            }
        }

        //Ve se a celula a esquerda esta dentro da grid
        if (x - 1 >= 0)
        {
            //Se sim vê se a celula já foi visitada
            var cellToLeft = _mazeGrid[x - 1, z];

            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }

        //Ve se a celula a cima esta dentro da grid
        if (z + 1 < _mazeDepth)
        {
            //Se sim vê se a celula já foi visitada
            var cellToFront = _mazeGrid[x, z + 1];

            if (cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }

        //Ve se a celula em baixo esta dentro da grid
        if (z - 1 >= 0)
        {
            //Se sim vê se a celula já foi visitada
            var cellToBack = _mazeGrid[x, z - 1];

            if (cellToBack.IsVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        //Confirma que é a primeira celula
        if (previousCell == null)
        {
            return;
        }

        //Vemos se a celula anterior está à esquerda da celula atual
        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            //Se sim sabemos que o algoritmo foi da esquerda para a direita
            //Apagamos a parede direita da celula anterior e a parede esquerda da celula atual
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        //Vemos se a celula anterior está à direita da celula atual
        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            //Se sim sabemos que o algoritmo foi da direita para a esquerda
            //Apagamos a parede esquerda da celula anterior e a parede direita da celula atual
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        //Vemos se a celula anterior está abaixo da celula atual
        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            //Se sim sabemos que o algoritmo foi de baixo para cima
            //Apagamos a parede de cima da celula anterior e a parede de baixo da celula atual
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        //Vemos se a celula anterior está acima da celula atual
        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            //Se sim sabemos que o algoritmo foi de cima para baixo
            //Apagamos a parede de baixo da celula anterior e a parede de cima da celula atual
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
