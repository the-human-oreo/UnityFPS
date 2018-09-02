using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private Camera cam;

    public PlayerWeapon weapon;

    [SerializeField]
    private LayerMask mask;

    private Transform BulletSpawn;

    void Start()
    {
        if (cam == null)
        {
            Debug.Log("PlayerShoot: No camera");
            this.enabled = false;
        } else
        {
            BulletSpawn = cam.transform.GetChild(0).GetChild(2).GetChild(0);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    [Client]
    void Shoot ()
    {
        RaycastHit _hit;

        if (Physics.Raycast(BulletSpawn.position, BulletSpawn.forward, out _hit, weapon.range, mask))
        {
            if (_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_hit.collider.name, weapon.damage);
            }
        }
    }

    [Command]
    void CmdPlayerShot(string _playerID, int _damage)
    {
        Debug.Log(_playerID + " has been shot");

        PlayerManager _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);
    }
}
