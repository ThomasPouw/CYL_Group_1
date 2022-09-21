using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] int FloorNumber = 5;
    [SerializeField] DifficultySetting Difficulty = DifficultySetting.INTERMEDIATE;
    [SerializeField] GameObject[] EasyFloorModels;
    [SerializeField] GameObject[] IntermediateFloorModels;
    [SerializeField] GameObject[] HardFloorModels;

    int CurrentFloor = 0;
    GameObject[] CurrentRunFloors;

    void Start()
    {
        ResetLevelVisibility();
        GenerateFloorSelection();
        CurrentRunFloors[0].SetActive(true);
    }

    public void ResetProgress()
    {
        CurrentFloor = 0;
        ResetCurrentRunLevelVisibility();
    }

    public void ResetSession()
    {
        CurrentFloor = 0;
        GenerateFloorSelection();
        ResetCurrentRunLevelVisibility();
    }

    public int GetCurrentFloorNumber() => CurrentFloor;

    public void GetNextFloor()
    {
        CurrentRunFloors[CurrentFloor].SetActive(false);
        if (CurrentFloor + 1 < CurrentRunFloors.Length)
            CurrentRunFloors[++CurrentFloor].SetActive(true);
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
        while (Index <= CurrentRunFloors.Length)
        {
            System.Random Random = new System.Random();
            FloorSelection = FloorSelection.OrderBy(x => Random.Next()).ToArray();
            Array.Copy(FloorSelection, 0, CurrentRunFloors, Index, Math.Min(FloorSelection.Length, CurrentRunFloors.Length - Index));
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
        foreach (GameObject Floor in EasyFloorModels)
            Floor.SetActive(false);
        foreach (GameObject Floor in IntermediateFloorModels)
            Floor.SetActive(false);
        foreach (GameObject Floor in HardFloorModels)
            Floor.SetActive(false);
    }

    private void ResetCurrentRunLevelVisibility()
    {
        foreach (GameObject Floor in CurrentRunFloors)
            Floor.SetActive(false);
        CurrentRunFloors[0].SetActive(true);
    }
}

public enum DifficultySetting
{
    EASY = 0,
    INTERMEDIATE = 1,
    HARD = 2
}
