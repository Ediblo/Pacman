using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour, IDataPersistence
{
   public int points = 10;
    public bool collected = false;

    private void Update()
    {
        if (this.gameObject.activeSelf)
        {
            collected = false;
        }
    }

   private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Pacman")){
            Eat();
        }
   }

   protected virtual void Eat(){
        FindObjectOfType<GameManager>().PelletEaten(this);
   }

   public void LoadData(GameData data)
    {
        data.pelletsCollected.TryGetValue(this.transform.position, out collected);
        if (collected)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.pelletsCollected.ContainsKey(this.transform.position))
        {
            data.pelletsCollected.Remove(this.transform.position);
        }
        data.pelletsCollected.Add(this.transform.position, collected);
    }
}
