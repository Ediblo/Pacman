using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int liveS;
    public int scoreS;
    public int highestscoreS;
    public Vector3 playerPos;
    public Vector2 playerDirect;
    public SerializableDictionary<Vector3, bool> pelletsCollected;


    public GameData(){
        this.liveS = 3;
        this.scoreS = 0;
        this.highestscoreS = 0;
        this.playerPos = new Vector3(0, -3.5f, -5f);
        this.playerDirect = Vector2.right;
        pelletsCollected = new SerializableDictionary<Vector3, bool>();
    }
}
