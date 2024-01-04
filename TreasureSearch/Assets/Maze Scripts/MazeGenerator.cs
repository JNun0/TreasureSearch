using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

/*Este gerador utiliza o metodo DFS*/

public class MazeGenerator : MonoBehaviour
{
    int xDFS, zDFS, 
        xBFS, zBFS,
        xAS, zAS;

    Vector3 aStar;

    Vector3 spawnPosition;

    private bool hasSpawnedAgent1 = false;

    private bool hasSpawnedAgent2 = false;

    private bool hasSpawnedAgent3 = false;

    private bool hasSpawnedPlayer = false;

    private bool hasSpawnedTreasure = false;

    [SerializeField]
    private GameObject _agent1;

    [SerializeField]
    private GameObject _agent2;
    
    [SerializeField]
    private GameObject _agent3;

    [SerializeField]
    private GameObject _treasure;

    [SerializeField]
    private GameObject _player;

    private GameObject _agentInstance;

    [SerializeField]
    private MazeCell _mazeCellPrefab;

    [SerializeField]
    public int _mazeWidth;

    [SerializeField]
    public int _mazeDepth;

    public MazeCell[,] _mazeGrid;

    private List<MazeCell> pathList = new List<MazeCell>();


    // Start is called before the first frame update

    //Inumerator para ver o maze a ser construido
    void Start()
    {
        xDFS = Random.Range(1, _mazeWidth);
        zDFS = Random.Range(1, _mazeDepth);

        xBFS = Random.Range(1, _mazeWidth);
        zBFS = Random.Range(1, _mazeDepth);

        xAS = Random.Range(1, _mazeWidth);
        zAS = Random.Range(1, _mazeWidth);

        aStar = new Vector3(xAS, 0, zAS);

        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        //Percorre a largura
        for (int x = 0; x < _mazeWidth; x++)
        {
            //Percorre a profundidade
            for (int z = 0; z < _mazeDepth; z++)
            {
                //Cria a base do labirinto
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
                _mazeGrid[x, z].transform.localPosition = new Vector3(x, 0, z);
            }
        }

        //Não há celula anterior por isso mete-se null e a primeira celula como currentcell
        //Se for Inumerator yield return
        GenerateMaze(null, _mazeGrid[0, 0]);
        GetComponent<NavMeshSurface>().BuildNavMesh();

        List<MazeCell> path = pathList;

        if (!hasSpawnedAgent1)
        {
            SpawnAgentAStar1(_agent1);
            hasSpawnedAgent1 = true;
        }

        if (!hasSpawnedAgent2)
        {
            SpawnAgentAStar2(_agent2);
            hasSpawnedAgent2 = true;
        }

        if (!hasSpawnedAgent3)
        {
            SpawnAgentAStar3(_agent3);
            hasSpawnedAgent3 = true;
        }

        if (!hasSpawnedPlayer)
        {
            SpawnPlayer(_player);
            hasSpawnedPlayer = true;
        }

        if (!hasSpawnedTreasure)
        {
            SpawnTreasure(_treasure);
            hasSpawnedTreasure = true;
        }
    }

    //Inumerator para ver o maze a ser construido
    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        //Adiciona a celula atual à lista de caminho
        pathList.Add(currentCell);
        //Torna a primeira celula visivel
        currentCell.Visit();
        ClearWalls(previousCell,currentCell);

        //Se for Inumerator yield return
        new WaitForSeconds(0.05f);

        MazeCell nextCell;

        //Vai de celula em celula ate nao haver celulas por visitar
        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                //Se for Inumerator yield return
                GenerateMaze(currentCell, nextCell);
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
    public IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        //Os valores correspondem ao index da celula na grid
        int x = (int)currentCell.transform.localPosition.x;
        int z = (int)currentCell.transform.localPosition.z;

        //Debug.Log(x + " - " + z);

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
        if (previousCell.transform.localPosition.x < currentCell.transform.localPosition.x)
        {
            //Se sim sabemos que o algoritmo foi da esquerda para a direita
            //Apagamos a parede direita da celula anterior e a parede esquerda da celula atual
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        //Vemos se a celula anterior está à direita da celula atual
        if (previousCell.transform.localPosition.x > currentCell.transform.localPosition.x)
        {
            //Se sim sabemos que o algoritmo foi da direita para a esquerda
            //Apagamos a parede esquerda da celula anterior e a parede direita da celula atual
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        //Vemos se a celula anterior está abaixo da celula atual
        if (previousCell.transform.localPosition.z < currentCell.transform.localPosition.z)
        {
            //Se sim sabemos que o algoritmo foi de baixo para cima
            //Apagamos a parede de cima da celula anterior e a parede de baixo da celula atual
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        //Vemos se a celula anterior está acima da celula atual
        if (previousCell.transform.localPosition.z > currentCell.transform.localPosition.z)
        {
            //Se sim sabemos que o algoritmo foi de cima para baixo
            //Apagamos a parede de baixo da celula anterior e a parede de cima da celula atual
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }
    public List<MazeCell> GetPath()
    {
        return pathList;
    }

    public void SpawnAgentAStar1(GameObject agentPrefab)
    {
        int xIndex = Random.Range(1, _mazeWidth);
        int zIndex = Random.Range(1, _mazeDepth);

        spawnPosition = _mazeGrid[xIndex, zIndex].transform.position;

        _agentInstance = Instantiate(agentPrefab, spawnPosition, Quaternion.identity);

    }

    public void SpawnAgentAStar2(GameObject agentPrefab)
    {
        int xIndex = Random.Range(1, _mazeWidth);
        int zIndex = Random.Range(1, _mazeDepth);

        spawnPosition = _mazeGrid[xIndex, zIndex].transform.position;

        _agentInstance = Instantiate(agentPrefab, spawnPosition, Quaternion.identity);

    }

    public void SpawnAgentAStar3(GameObject agentPrefab)
    {
        int xIndex = Random.Range(1, _mazeWidth);
        int zIndex = Random.Range(1, _mazeDepth);

        spawnPosition = _mazeGrid[xIndex, zIndex].transform.position;

        _agentInstance = Instantiate(agentPrefab, spawnPosition, Quaternion.identity);

    }


    public void SpawnTreasure(GameObject agentPrefab)
    {
        spawnPosition = new Vector3(45, 2, 45);

        _agentInstance = Instantiate(agentPrefab, spawnPosition, Quaternion.identity);
    }

    public void SpawnPlayer(GameObject agentPrefab)
    {
        spawnPosition = new Vector3(0, 2, 0);

        _agentInstance = Instantiate(agentPrefab, spawnPosition, Quaternion.identity);
    }

    public MazeCell GetCellAtPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x);
        int z = Mathf.RoundToInt(position.z);


        //Verifica se as coordenadas estaoo dentro dos limites do labirinto
        if (x >= 0 && x < _mazeWidth && z >= 0 && z < _mazeDepth)
        {
            return _mazeGrid[x, z];
        }
        else
        {
            //Null se estiver fora dos limites do labirinto
            return null;
        }
    }

    public void ResetGame()
    {

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }

}
