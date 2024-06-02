using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MazeCell : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftWall;
    [SerializeField]
    private GameObject _rightWall;
    [SerializeField]
    private GameObject _frontWall;
    [SerializeField]
    private GameObject _backWall;
    [SerializeField]
    private GameObject _unvisitedBlock;
    [SerializeField]
    private GameObject _collectable;
    [SerializeField]
    private GameObject _exit;
    [SerializeField]
    private GameObject _floor;

    [SerializeField]
    private GameObject _top;

    public bool IsVisited { get; private set; }

    public void Visit()
    {
        IsVisited = true;
        _unvisitedBlock.SetActive(false);
        _collectable.SetActive(false);
        _exit.SetActive(false);
    }

    public void ClearLeftWall()
    {
        _leftWall.SetActive(false);
    }

    public void ClearRightWall()
    {
        _rightWall.SetActive(false);
    }

    public void ClearFrontWall()
    {
        _frontWall.SetActive(false);
    }

    public void ClearBackWall()
    {
        _backWall.SetActive(false);
    }

    public void AddCollectable()
    {
        _collectable.SetActive(true);
    }

    public void SetExit()
    {
        _exit.SetActive(true);
        _floor.SetActive(false);
    }
}