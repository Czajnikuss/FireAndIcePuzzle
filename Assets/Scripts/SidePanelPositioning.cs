using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SidePanelLocation
{
    top, 
    bottom, 
    left, 
    right
}
public class SidePanelPositioning : MonoBehaviour
{
    [SerializeField] private MainBoard mainBoard;
    private float panelWidth, panelHight;
    public int numberOfPossiblePuzzles;
    public GameObject possiblePuzzleShowPanelPrefab;
    public float offsetFromMainBoard;
    public SidePanelLocation panelLocation;
    public float borderThicknes;
    
    
    void Start()
    {
        DrawPanel(); 
           
    }

    private void DrawPanel()
    {
        if(panelLocation == SidePanelLocation.top || panelLocation == SidePanelLocation.bottom)
        {
            //top/bottom loctaion, calculate width and hight acordingly
            panelWidth = mainBoard.cellSize*mainBoard.width;
            panelHight = panelWidth/3f;
            transform.localScale = new Vector3(panelWidth, panelHight, 1f);
          //  DrawHorizontal();
        }
        else
        {
            //left/right loctaion, calculate width and hight acordingly
            panelHight = mainBoard.cellSize * mainBoard.hight;
            panelWidth = panelHight /3f;
            transform.localScale = new Vector3(panelWidth, panelHight, 1f);
          //  DrawVertical();
        }
        //scale set, now position
        if(panelLocation == SidePanelLocation.top)
        {
            transform.position = new Vector3(mainBoard.transform.position.x, mainBoard.transform.position.y + ((mainBoard.cellSize*mainBoard.hight)/2f ) + (panelHight /2f)+ offsetFromMainBoard, mainBoard.transform.position.z); 
        }
        else if(panelLocation == SidePanelLocation.bottom)
        {
            transform.position = new Vector3(mainBoard.transform.position.x, -(mainBoard.transform.position.y + ((mainBoard.cellSize*mainBoard.hight)/2f )+(panelHight /2f)+ offsetFromMainBoard), mainBoard.transform.position.z); 
        }
        else if(panelLocation == SidePanelLocation.right)
        {
            transform.position = new Vector3(mainBoard.transform.position.x + ((mainBoard.cellSize*mainBoard.width)/2f )+(panelWidth/2f)+ offsetFromMainBoard, mainBoard.transform.position.y , mainBoard.transform.position.z); 
        }
        else if(panelLocation == SidePanelLocation.left)
        {
            transform.position = new Vector3(-(mainBoard.transform.position.x + ((mainBoard.cellSize*mainBoard.width)/2f )+(panelWidth/2f)+ offsetFromMainBoard), mainBoard.transform.position.y , mainBoard.transform.position.z); 
        }

    }
    private void DrawHorizontal()
    {

        for (int i = 0; i < numberOfPossiblePuzzles; i++)
        {
            GameObject tempPanel = Instantiate(possiblePuzzleShowPanelPrefab, new Vector3( Mathf.Lerp(-(panelWidth/2f),panelWidth/2, i), 0, 0), Quaternion.identity, this.transform);
            
        }
    }
    private void DrawVertical()
    {
        
        for (int i = 0; i < numberOfPossiblePuzzles; i++)
        {
            GameObject tempPanel = Instantiate(possiblePuzzleShowPanelPrefab, this.transform, false);
            tempPanel.transform.localPosition = new Vector3( 0,Mathf.Lerp(-(panelHight/2f), panelHight/2f, (float)i/(float)numberOfPossiblePuzzles-1), 0);
            //tempPanel.transform.localScale = new Vector3(1f/transform.localScale.x, 1f/ transform.localScale.y, 1f/ transform.localScale.z);
        }
    }
}
