using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageGame : MonoBehaviour
{
    public int refreshAfterMoved = 3, score = 0;
    List<List<GameObject>> gameObjects = new List<List<GameObject>>();
    int movedCount = 0;
    GameObject[] options;
    GameObject canvas;
    Text currentScore, highScore;
    string highscorekey = "highscore";

    // List<List<GameObject>> gamegrid = new List<List<GameObject>>();
    // Start is called before the first frame update
    void Start(){
        options = GameObject.FindGameObjectsWithTag("Player");
        canvas = GameObject.FindGameObjectWithTag("Finish");
        canvas.SetActive(false);
        // Debug.Log(canvas);
        currentScore = GameObject.FindWithTag("CurrentScore").GetComponent<Text>();
        currentScore.text = score+"";
        highScore = GameObject.FindWithTag("HighScore").GetComponent<Text>();
        highScore.text = PlayerPrefs.GetInt(highscorekey,0)+"";
    }

    // Update is called once per frame
    void Update(){

    }

    public bool checkForBreaks(){
        bool[] cols = new bool[gameObjects.Count];
        int[] ccount = new int[gameObjects.Count];
        foreach(List<GameObject> gos in gameObjects){ //rows
            List<bool> p = new List<bool>();
            bool row = true;
            int colNum = 0;
            foreach(GameObject go in gos){ // cols
                // p.Add(gameObjects[i+j].GetComponent<GridPiece>().isPlaced());
                bool isPlaced = go.GetComponent<GridPiece>().isPlaced();
                if(!isPlaced){
                    row=false;
                    cols[colNum]=true;
                    // break;
                }else
                    ccount[colNum]++;
                colNum++;
            }
            if(row){
                addPoints(10);
                foreach(GameObject go in gos)
                    breakPeice(go);
            }
        }
        for(int i = 0; i<cols.Length; i++){
            if(ccount[i]==10){ // this means col is a break // ! not is required because cols is initialized as false
                addPoints(10);
                foreach(List<GameObject> gos in gameObjects)
                    breakPeice(gos[i]);
            }
        }

        return false;
    }

    public void breakPeice(GameObject go){
        GridPiece gp = go.GetComponent<GridPiece>();
        gp.place(false);
        Destroy(gp.getPlacedGameObject());
    }
    public void checkForAllowedPieces(){
        foreach(GameObject g in options)
            if(checkIfPieceIsAllowed(g.transform.Find("piece")))
                return;
        Debug.Log("Game Over");
        canvas.SetActive(true);
    }
    // once all 3 pieces are not allowed then report game over
    public bool checkIfPieceIsAllowed(Transform piece){
        if(piece == null)
            return false;
        List<StickPieceToGrid> stickPieces = new List<StickPieceToGrid>();
        foreach(Transform child in piece)
            stickPieces.Add(child.GetComponent<StickPieceToGrid>());
        for(int i = 0; i<gameObjects.Count; i++){
            for(int j = 0; j<gameObjects[i].Count; j++){
                bool isUsedGrid = false;
                string v = piece.parent.name+": ";
                foreach(StickPieceToGrid sptg in stickPieces){
                    int y = sptg.x, x = sptg.y;
                    v+=(x+i)+","+(y+j)+" | ";
                    if(x+i >= gameObjects.Count){
                        isUsedGrid = true;
                        break;
                    }if(y+j >= gameObjects[x+i].Count){
                        isUsedGrid = true;
                        break;
                    }if(gameObjects[x+i][y+j].GetComponent<GridPiece>().isPlaced()){
                        isUsedGrid = true;
                        break;
                    }
                }
                if(!isUsedGrid){
                    // Debug.Log("NOT Used: "+v);
                    return true;
                }
                // Debug.Log("Used: "+v);
            }
        }
        // Debug.Log("Can't be placed: "+piece.parent.name);
        return false;
    }
    public void addPoints(int p){
        score+=p;
        currentScore.text = score+"";
        if(score > PlayerPrefs.GetInt(highscorekey,0)){
            PlayerPrefs.SetInt(highscorekey,score);
            highScore.text = score+"";
        }
    }

    public void setGameObjects(List<GameObject> gos){
        List<List<GameObject>> gameos = new List<List<GameObject>>();
        int k = 0;
        for(int i = 0; i<10; i++){
            List<GameObject> g = new List<GameObject>();
            for(int j = 0; j<10; j++){
                GameObject ggg=gos[k++];
                ggg.GetComponent<GridPiece>().setXY(i,j);
                g.Add(ggg);
            }
            gameos.Add(g);
        }
        gameObjects = gameos;
    }

    public List<List<GameObject>> GetGameObjects(){
        return gameObjects;
    }

    public void Moved(){
        movedCount++;
        if(movedCount >= refreshAfterMoved){
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject g in objects)
                g.GetComponent<CreateShape>().NewShape();
            movedCount = 0;
            checkForAllowedPieces();
        }
    }
}
