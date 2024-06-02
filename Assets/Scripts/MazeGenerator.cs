using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell _mazeCellPrefab;
    [SerializeField]
    private int _mazeWidth;
    [SerializeField]
    private int _mazeDepth;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private Camera _camera;

    private MazeCell[,] _mazeGrid;
    private GameObject _mazeContainer;

    void Start()
    {
        // Crear un GameObject vac?o para contener el laberinto
        _mazeContainer = new GameObject("MazeContainer");
        // A?adir el script BoardController al MazeContainer
        _mazeContainer.AddComponent<BoardController>();

        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
                _mazeGrid[x, z].transform.parent = _mazeContainer.transform; // Asignar "MazeContainer" como padre
            }
        }

        GenerateMaze(null, _mazeGrid[0, 0]);

        // Mover al jugador a la posici?n inicial
        _player.transform.position = new Vector3(0, 0.5f, 0);

        // Agregar coleccionables y salida
        AddSpecialItems();

        // Configurar la c?mara
        ConfigureCamera();

        InitializePlayerPhysics();
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        if (x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];
            if (cellToRight.IsVisited == false)
            {
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];
            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if (z + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, z + 1];
            if (cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }

        if (z - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];
            if (cellToBack.IsVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

    private void AddSpecialItems()
    {
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                if (Random.value < 0.2f)
                {
                    _mazeGrid[x, z].AddCollectable();
                }
            }
        }

        // Agregar la salida en la ?ltima celda
        _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].SetExit();
    }

    private void ConfigureCamera()
    {
        // Calcula la posici?n central del laberinto
        Vector3 centerPosition = new Vector3(_mazeWidth / 2f, 0, _mazeDepth / 2f);

        if (_camera != null)
        {
            // Ajusta la posici?n de la c?mara inicial
            float cameraDistance = Mathf.Max(_mazeWidth, _mazeDepth);
            _camera.transform.position = new Vector3(centerPosition.x - cameraDistance / 2, cameraDistance, centerPosition.z - cameraDistance / 2);
            _camera.transform.LookAt(centerPosition);

            // Asignar el objetivo y el desplazamiento al script SmoothCameraFollow
            SmoothCameraFollow followScript = _camera.GetComponent<SmoothCameraFollow>();
            if (followScript != null)
            {
                followScript.target = _player.transform;
                followScript.offset = new Vector3(0, 10, 0); // Ajustar seg?n sea necesario
            }
        }
    }

    private void InitializePlayerPhysics()
    {
        Rigidbody rb = _player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.WakeUp(); // Asegura que el Rigidbody no est? dormido al inicio
        }
    }
}
