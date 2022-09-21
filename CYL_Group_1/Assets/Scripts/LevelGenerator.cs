using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] int FloorNumber = 5;
    [SerializeField] DifficultySetting Difficulty = DifficultySetting.INTERMEDIATE;
    [SerializeField] GameObject StartingRoom;
    [SerializeField] GameObject EndingRoom;
    [SerializeField] GameObject[] EasyFloorModels;
    [SerializeField] GameObject[] IntermediateFloorModels;
    [SerializeField] GameObject[] HardFloorModels;

    int CurrentFloor = -1;
    GameObject[] CurrentRunFloors;

    void Start()
    {
        ResetLevelVisibility();
        GenerateFloorSelection();
        StartingRoom.SetActive(true);
    }

    public void ResetProgress()
    {
        CurrentFloor = -1;
        ResetCurrentRunLevelVisibility();
    }

    public void ResetSession()
    {
        CurrentFloor = -1;
        GenerateFloorSelection();
        ResetCurrentRunLevelVisibility();
    }

    public int GetCurrentFloorNumber() => CurrentFloor;

    public void GetNextFloor()
    {
        CurrentRunFloors[CurrentFloor].SetActive(false);
        if (CurrentFloor + 1 < CurrentRunFloors.Length)
            CurrentRunFloors[++CurrentFloor].SetActive(true);
        else if (CurrentFloor + 1 >= CurrentRunFloors.Length)
            EndingRoom.SetActive(true);
        else
        {
            CurrentFloor = -1;
            StartingRoom.SetActive(true);
        }
    }

    private void GenerateFloorSelection()
    {
        CurrentRunFloors = new GameObject[FloorNumber];
        GameObject[] FloorSelection = GetFloorSelection();

        if (FloorSelection == null)
            return;
        else if (FloorSelection.Length <= 0)
            return;

        int Index = 0;
        while (Index < CurrentRunFloors.Length)
        {
            System.Random Random = new System.Random();
            FloorSelection = FloorSelection.OrderBy(x => Random.Next()).ToArray();
            Array.Copy(FloorSelection, 0, CurrentRunFloors, Index, Math.Min(FloorSelection.Length, CurrentRunFloors.Length - Index));
            Index += Math.Min(FloorSelection.Length, CurrentRunFloors.Length - Index);
        }
    }

    private GameObject[] GetFloorSelection()
    {
        switch (Difficulty)
        {
            case DifficultySetting.EASY:
                return EasyFloorModels;
            case DifficultySetting.INTERMEDIATE:
                return IntermediateFloorModels;
            case DifficultySetting.HARD:
                return HardFloorModels;
            default:
                return null;
        }    
    }

    private void ResetLevelVisibility()
    {
        StartingRoom.SetActive(true);
        foreach (GameObject Floor in EasyFloorModels)
            Floor.SetActive(false);
        foreach (GameObject Floor in IntermediateFloorModels)
            Floor.SetActive(false);
        foreach (GameObject Floor in HardFloorModels)
            Floor.SetActive(false);
        EndingRoom.SetActive(false);
    }

    private void ResetCurrentRunLevelVisibility()
    {
        StartingRoom.SetActive(true);
        foreach (GameObject Floor in CurrentRunFloors)
            Floor.SetActive(false);
        EndingRoom.SetActive(false);
    }
}

public enum DifficultySetting
{
    EASY = 0,
    INTERMEDIATE = 1,
    HARD = 2
}
