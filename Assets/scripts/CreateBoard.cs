using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBoard : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 scale = new Vector3(.001f,.001f,.001f);
    public Vector2 offset = new Vector2(0.26f,0.26f);
    GameObject gg;
    List<GameObject> gameObjects = new List<GameObject>();
    Vector2 min = new Vector2(9999f,9999f), max = new Vector2(-9999f,-9999f);
    void Start(){
        gg = new GameObject("GameBoard");
        Sprite[] sprites = Resources.LoadAll<Sprite>("sprites");
        // Sprite[] sprites = new Sprite[10*10];
        System.Array.Reverse(sprites);
        int x = 0, y = 0;
        Vector3 center = new Vector3();
        foreach(Sprite s in sprites){
            center+=SpawnTile(x++,y,s).transform.position;
            // Debug.Log("created sprite at: "+(x-1)+","+y+",,"+s);
            if(x>=10){
                x=0;
                y++;
            }
        }
        center = new Vector3(center.x/(float)sprites.Length,center.y/(float)sprites.Length,center.z/(float)sprites.Length);
        gg.transform.parent = transform;
        gg.transform.position = transform.position-center;
        BoxCollider2D collider = gg.AddComponent<BoxCollider2D>();
        collider.size = new Vector3(max.x-min.x+offset.x*2f,max.y-min.y+offset.y*2f,0);
        collider.offset = center;
        collider.isTrigger = true;

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ManageGame>().setGameObjects(gameObjects);
    }

    // Update is called once per frame
    void Update(){
        
    }


    GameObject SpawnTile(int x, int y, Sprite sprite){
        GameObject g = new GameObject(x + ":" + y);
        // Debug.Log(name+" "+transform.position);
        g.transform.localScale = scale;
        g.transform.parent = gg.transform;
        SpriteRenderer tile = g.AddComponent<SpriteRenderer>();
        tile.sprite = sprite;
        // Debug.Log(rect);
        g.transform.position = transform.position+new Vector3(x*offset.x,y*offset.y,0);

        BoxCollider2D collider = g.AddComponent<BoxCollider2D>();
        collider.size = new Vector3(offset.x*2f,offset.y*2f,0);
        // collider.offset = new Vector3(x*offset.x,y*offset.y,0);
        collider.isTrigger = true;
        // g.AddComponent<StickPieceToGrid>();
        g.AddComponent<GridPiece>();
        Rigidbody2D rb = g.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        // size+=new Vector3(x*offset.x,y*offset.y,0);
        min.x = min.x > g.transform.position.x ? g.transform.position.x : min.x;
        min.y = min.y > g.transform.position.y ? g.transform.position.y : min.y;
        max.x = max.x < g.transform.position.x ? g.transform.position.x : max.x;
        max.y = max.y < g.transform.position.y ? g.transform.position.y : max.y;
        gameObjects.Add(g);
        return g;
        // Debug.Log("TILE:" + g.transform.position);
        // tile.color = new Color(value, value, value);
    }
    
}
