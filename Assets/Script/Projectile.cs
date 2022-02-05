using UnityEngine;
using System.Collections;

public class Projectile
{
    private GameObject gameObject;
    public Coroutine moveCoroutine;

    public Projectile(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public IEnumerator DestoryProjectile()
    {
        var animator = gameObject.GetComponent<Animator>();
        animator.Play("Destroy");
        yield return new WaitForSeconds((float)0.5);

        UnityEngine.Object.Destroy(gameObject);
    }

    public IEnumerator MoveCoroutine(float x, float y)
    {
        float currX = gameObject.transform.position.x;
        float currY = gameObject.transform.position.y;

        if (currX == x && currY == y)
        {
            moveCoroutine = null;
            yield break;
        }

        float deltaX = (x - currX) / (PlayGround.TICK_COUNT / PlayGround.MOVE_FRAME);
        float deltaY = (y - currY) / (PlayGround.TICK_COUNT / PlayGround.MOVE_FRAME);

        for (int i = 0; i < PlayGround.TICK_COUNT / PlayGround.MOVE_FRAME; i++)
        {
            gameObject.transform.Translate(new Vector2(deltaX, deltaY));
            yield return new WaitForSeconds((float)PlayGround.MOVE_FRAME / (float)1000);
        }

        moveCoroutine = null;
    }
}
