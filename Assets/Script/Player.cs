using UnityEngine;
using System.Collections;
using System;

public class Player
{
    private GameObject gameObject;
    private Animator animator;
    public Coroutine moveCoroutine;
    public Coroutine hitCoroutine;

    public Player(GameObject gameObject)
    {
        this.gameObject = gameObject;
        animator = gameObject.GetComponent<Animator>();
    }

    public void DestroyPlayer()
    {
        UnityEngine.Object.Destroy(gameObject);
    }

    private float GetRotator(Orc.Direction dir)
    {
        return 360.0f / 8 * ((int)dir - 1);
    }

    public IEnumerator MoveCoroutine(float x, float z, Orc.Direction dir)
    {
        float currX = gameObject.transform.position.x;
        float currZ = gameObject.transform.position.z;

        gameObject.transform.eulerAngles = new Vector3(0, GetRotator(dir), 0);

        if(currX == x && currZ == z)
        {
            moveCoroutine = null;
            yield break;
        }

        animator.SetBool("Walk", true);

        float deltaX = (x - currX) / (PlayGround.TICK_COUNT / PlayGround.MOVE_FRAME);
        float deltaZ = (z - currZ) / (PlayGround.TICK_COUNT / PlayGround.MOVE_FRAME);

        for(int i = 0; i < PlayGround.TICK_COUNT / PlayGround.MOVE_FRAME; i++)
        {
            gameObject.transform.Translate(new Vector3(deltaX, 0, deltaZ), Space.World);
           
            yield return new WaitForSeconds((float)PlayGround.MOVE_FRAME / (float)1000);
        }

        animator.SetBool("Walk", false);
        moveCoroutine = null;
    }

    public IEnumerator KnockBack(float x, float z)
    {
        animator.SetBool("Hit", true);

        float currX = gameObject.transform.position.x;
        float currZ = gameObject.transform.position.z;
        float currSpeedX = 0.0f;
        float currSpeedZ = 0.0f;
        int tickCount = PlayGround.TICK_COUNT / PlayGround.MOVE_FRAME;
        for (int i = 0; i < tickCount; ++i) 
        {
            var newX = Mathf.SmoothDamp(currX, x, ref currSpeedX, (float)PlayGround.TICK_COUNT / 1000 * tickCount);
            var newZ = Mathf.SmoothDamp(currZ, z, ref currSpeedZ, (float)PlayGround.TICK_COUNT / 1000 * tickCount);
            gameObject.transform.Translate(new Vector3( newX, 0, newZ));

            Debug.Log(string.Format("{0},{1} : {4},{5} -> {2},{3} (speed : {6},{7})", x, z, newX, newZ, currX, currZ, currSpeedX, currSpeedZ));
            yield return new WaitForSeconds((float)PlayGround.TICK_COUNT / 1000);
        }
        animator.SetBool("Hit", false);
        moveCoroutine = null;
    }

    public void Attack()
    {
        animator.SetBool("Attack", true);
    }

}
