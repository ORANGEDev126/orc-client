using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayGround : MonoBehaviour
{
    private Dictionary<long, Player> playerContainer = new Dictionary<long, Player>();
    private Dictionary<long, Projectile> projectileContainer = new Dictionary<long, Projectile>();
    private long MyID;
    public GameObject defaultPlayerPrefab;
    public GameObject defaultProjectilePrefab;
    public GameObject cameraPrefab;
    static public PlayGround globalPlayGround;
    public const int TICK_COUNT = 100;
    public const int MOVE_FRAME = 20;

    void Start()
    {
        globalPlayGround = this;
    }

    public void EnterPlayer(Orc.PlayerMessage msg, bool isSelf)
    {
        GameObject playerObject = (GameObject)Instantiate(defaultPlayerPrefab,
            new Vector3((float)msg.X, (float)msg.Y, 0), Quaternion.identity);
        
        if (isSelf)
        {
            MyID = msg.Id;
            GameObject cameraObject = (GameObject)Instantiate(cameraPrefab);
            cameraObject.transform.SetParent(playerObject.transform);
            cameraObject.transform.localPosition = new Vector3(0, 0, -20);
            cameraObject.transform.localRotation = Quaternion.identity;
        }

        Player player = new Player(playerObject);
        playerContainer.Add(msg.Id, player);
    }

    public void EnterProjectile(Orc.ProjectileMessage msg)
    {
        GameObject projectileObject = (GameObject)Instantiate(defaultProjectilePrefab,
            new Vector3((float)msg.X, (float)msg.Y, 0), Quaternion.identity);

        Projectile projectile = new Projectile(projectileObject);
        projectileContainer.Add(msg.Id, projectile);
    }

    public void Leave(long id)
    {
        if(playerContainer.ContainsKey(id))
        {
            playerContainer[id].DestroyPlayer();
            playerContainer.Remove(id);
        }

        if(projectileContainer.ContainsKey(id))
        {
            projectileContainer[id].DestoryProjectile();
            projectileContainer.Remove(id);
        }
    }

    public void Move(long id, Vector3 point, Orc.Direction dir)
    {
        if(playerContainer.ContainsKey(id))
        {
            Player player = playerContainer[id];

            if(player.moveCoroutine != null)
            {
                StopCoroutine(player.moveCoroutine);
            }

            player.moveCoroutine = StartCoroutine(
                player.MoveCoroutine(point.x, point.y, dir));
        }

        if(projectileContainer.ContainsKey(id))
        {
            Projectile projectile = projectileContainer[id];

            if(projectile.moveCoroutine != null)
            {
                StopCoroutine(projectile.moveCoroutine);
            }

            projectile.moveCoroutine = StartCoroutine(
                projectile.MoveCoroutine(point.x, point.y));
        }
    }

    public void ProjectileAttacked(long playerId, long projectileId)
    {
        if(!playerContainer.ContainsKey(playerId))
        {
            Debug.Log("there is no player id when projectile attacked");
            return;
        }

        if(!projectileContainer.ContainsKey(projectileId))
        {
            Debug.Log("there is no projectile id when projectile attacked");
            return;
        }

        StartCoroutine(projectileContainer[projectileId].DestoryProjectile());
        projectileContainer.Remove(projectileId);
    }
}
