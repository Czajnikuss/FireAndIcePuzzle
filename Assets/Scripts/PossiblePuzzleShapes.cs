﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossiblePuzzleShapes : MonoBehaviour
{
#region SINGLETON PATTERN
 public static PossiblePuzzleShapes _instance;
 public static PossiblePuzzleShapes Instance
 {
     get {
         if (_instance == null)
         {
             _instance = GameObject.FindObjectOfType<PossiblePuzzleShapes>();
             
             if (_instance == null)
             {
                 GameObject container = new GameObject("PossiblePuzzleShapes");
                 _instance = container.AddComponent<PossiblePuzzleShapes>();
             }
         }
     
         return _instance;
     }
 }
 #endregion
    
    public List<bool[,]> allShapes = new List<bool[,]>(); 
    
    public SpriteNames thisPuzzleType;
    private bool[,] one = new bool[5,5];
    private bool[,] twoVertical = new bool[5,5];
    private bool[,] twoHorizontal = new bool[5,5];
    private bool[,] threeVertical = new bool[5,5];
    private bool[,] threeHorizontal = new bool[5,5];
    private bool[,] fourHorizontal = new bool[5,5];
    private bool[,] fourVerticla = new bool[5,5];
    private bool[,] fiveHorizontal = new bool[5,5];
    private bool[,] fiveVerticla = new bool[5,5];
    private bool[,] squareSmall = new bool[5,5];
    private bool[,] squareBig = new bool[5,5];
    
    


    public void Awake()
    {
        one[2,2] = true;
        allShapes.Add(one);
        twoHorizontal[2,2] = true;
        twoHorizontal[3,2] = true;
        allShapes.Add(twoHorizontal);
        twoVertical[2,2] = true;
        twoVertical[2,3] = true;
        allShapes.Add(twoVertical);
        threeHorizontal[2,2] = true;
        threeHorizontal[1,2] = true;
        threeHorizontal[3,2] = true;
        allShapes.Add(threeHorizontal);
        threeVertical[2,2] = true;
        threeVertical[2,1] = true;
        threeVertical[2,3] = true;
        allShapes.Add(threeVertical);
        fourHorizontal[2,2] = true;
        fourHorizontal[3,2] = true;
        fourHorizontal[1,2] = true;
        fourHorizontal[0,2] = true;
        allShapes.Add(fourHorizontal);
        fourVerticla[2,2] = true;
        fourVerticla[2,3] = true;
        fourVerticla[2,1] = true;
        fourVerticla[2,0] = true;
        allShapes.Add(fourVerticla);
        fiveHorizontal[2,2] = true;
        fiveHorizontal[3,2] = true;
        fiveHorizontal[4,2] = true;
        fiveHorizontal[1,2] = true;
        fiveHorizontal[0,2] = true;
        allShapes.Add(fiveHorizontal);
        fiveVerticla[2,2] = true;
        fiveVerticla[2,3] = true;
        fiveVerticla[2,4] = true;
        fiveVerticla[2,1] = true;
        fiveVerticla[2,0] = true;
        allShapes.Add(fiveVerticla);
        squareSmall[2,2] = true;
        squareSmall[2,3] = true;
        squareSmall[3,2] = true;
        squareSmall[3,3] = true;
        allShapes.Add(squareSmall);
        allShapes.Add(squareSmall);
        squareBig[2,2] = true;
        squareBig[3,2] = true;
        squareBig[2,3] = true;
        squareBig[3,3] = true;
        squareBig[1,2] = true;
        squareBig[2,1] = true;
        squareBig[1,1] = true;
        squareBig[1,3] = true;
        squareBig[3,1] = true;
        allShapes.Add(squareBig);

    }
    public SpriteNames[,] GeneratePattern()
    {
        bool[,] randomShape = allShapes[Random.Range(0,allShapes.Count)];
        SpriteNames[,] puzzlePattern = new SpriteNames[5,5];
        if(Mathf.RoundToInt(Random.Range(0,1f)) == 1f)
        {
            thisPuzzleType = SpriteNames.fireFilled;
        }
        else
        {
            thisPuzzleType = SpriteNames.iceFilled;
        }

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                if(randomShape[x,y])
                {
                    puzzlePattern[x,y] = thisPuzzleType;
                }
            }
        }
        return puzzlePattern;
    }
}

