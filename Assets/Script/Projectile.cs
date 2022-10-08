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
        if (animator != null)
        {
            animator.Play("Destroy");
        }
        yield return new WaitForSeconds((float)0.5);
        UnityEngine.Object.Destroy(gameObject);
    }

    public IEnumerator MoveCoroutine(float x, float z)
    {
        float currX = gameObject.transform.position.x;
        float currZ = gameObject.transform.position.z;

        if (currX == x && currZ == z)
        {
            moveCoroutine = null;
            yield break;
        }

        float deltaX = (x - currX) / (PlayGround.TICK_COUNT / PlayGround.MOVE_FRAME);
        float deltaZ = (z - currZ) / (PlayGround.TICK_COUNT / PlayGround.MOVE_FRAME);

        for (int i = 0; i < PlayGround.TICK_COUNT / PlayGround.MOVE_FRAME; i++)
        {
            gameObject.transform.Translate(new Vector3(deltaX, 0, deltaZ));
            yield return new WaitForSeconds((float)PlayGround.MOVE_FRAME / (float)1000);
        }

        moveCoroutine = null;
    }
}
