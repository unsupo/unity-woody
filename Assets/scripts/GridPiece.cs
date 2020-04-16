using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPiece : MonoBehaviour
{

    public bool _isPlaced = false;
    public int x,y;
    public GameObject placedGameObject;
    // Start is called before the first frame update
    void Start(){
        
    }
    public void setXY(int x, int y){
        this.x = x;
        this.y = y;
    }
    public void setPlacedGameObject(GameObject go){
        placedGameObject = go;
    }
    public GameObject getPlacedGameObject(){
        return placedGameObject;
    }
    public bool isPlaced(){
        return _isPlaced;
    }

    public void place(bool p){
        _isPlaced = p;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
