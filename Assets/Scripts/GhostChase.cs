using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostChase : GhostBehavior
{
    private void OnDisable(){
        this.ghost.scatter.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other){
        Node node = other.GetComponent<Node>();

        if(node != null && !this.enabled && !this.ghost.frightened.enabled){
            Vector2 direction = Vector2.zero;

            float minDistance = float.MaxValue;

            foreach(Vector2 availableDirections in node.availableDirections){
                Vector3 newPosition = this.transform.position + new Vector3(availableDirections.x, availableDirections.y, 0.0f);

                float distance = (this.ghost.target.position - newPosition).sqrMagnitude;

                if(distance < minDistance){
                    direction = availableDirections;
                    minDistance = distance;
                }
            }

            this.ghost.movement.SetDirection(direction);
        }
    }
}
