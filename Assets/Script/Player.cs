using UnityEngine;
using System.Collections;
using System;

public class Player
{
    private GameObject gameObject;
    private Animator animator;
    public Coroutine moveCoroutine;

    public Player(GameObject gameObject)
    {
        this.gameObject = gameObject;
        animator = gameObject.GetComponent<Animator>();
    }

    public void DestroyPlayer()
    {
        UnityEngine.Object.Destroy(gameObject);
    }

    public IEnumerator MoveCoroutine(float x, float y, Orc.Direction dir)
    {
        float currX = gameObject.transform.position.x;
        float currY = gameObject.transform.position.y;
        
        if(dir == Orc.Direction.East || dir == Orc.Direction.EastSouth ||
            dir == Orc.Direction.NorthEast)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(dir == Orc.Direction.West || dir == Orc.Direction.WestNorth ||
            dir == Orc.Direction.SouthWest)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

        if(currX == x && currY == y)
        {
            moveCoroutine = null;
            yield break;
        }

        animator.Play("Move");

        float deltaX = (x - currX) / (PlayGround.TICK_COUNT / PlayGround.MOVE_FRAME);
        float deltaY = (y - currY) / (PlayGround.TICK_COUNT / PlayGround.MOVE_FRAME);

        for(int i = 0; i < PlayGround.TICK_COUNT / PlayGround.MOVE_FRAME; i++)
        {
            gameObject.transform.Translate(new Vector2(deltaX, deltaY));
            yield return new WaitForSeconds((float)PlayGround.MOVE_FRAME / (float)1000);
        }

        animator.Play("Idle");
        moveCoroutine = null;
    }
}
