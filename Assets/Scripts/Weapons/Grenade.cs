using Assets.Scripts.Player;
using System.Collections;
using UnityEngine;

public class Grenade : BaseWeapon
{
    [SerializeField] private GameObject _grenadePrefab;
    [SerializeField] private float _throwForce = 40f;
    [SerializeField] private float _throwDelay = 1f;

    private MeshRenderer _meshRenderer;

    private bool _canThrow = true;

    void Start()
    {
        this._meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (!PauseHandler.GamePaused)
        {
            if (Input.GetMouseButtonDown(0) && this._ammoSlot.GetCurrentAmmo(this._ammoType) > 0 && this._canThrow)
            {
                this.StartCoroutine(this.Throw());
            }

            this.EnableMesh();
            base.DisplayAmmo();
        }
    }

    private IEnumerator Throw()
    {
        this._canThrow = false;

        Vector3 throwDirection = this._fpsCamera.transform.forward.normalized;

        GameObject grenade = Instantiate(this._grenadePrefab, transform.position, transform.rotation);
        grenade.GetComponent<Rigidbody>().AddForce(throwDirection * this._throwForce);

        this._ammoSlot.ReduceCurrentAmmo(this._ammoType);
        if (this._ammoSlot.GetCurrentAmmo(this._ammoType) == 0)
        {
            this._meshRenderer.enabled = false;
        }

        yield return new WaitForSeconds(this._throwDelay);

        this._canThrow = true;
    }

    private void EnableMesh()
    {
        if (this._meshRenderer.enabled == false && this._ammoSlot.GetCurrentAmmo(this._ammoType) > 0)
        {
            this._meshRenderer.enabled = true;
        }
    }
}
