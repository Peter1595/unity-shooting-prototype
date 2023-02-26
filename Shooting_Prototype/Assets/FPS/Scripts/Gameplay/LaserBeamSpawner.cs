using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class LaserBeamSpawner : MonoBehaviour
    {
        public Transform Muzzle;

        public GameObject LaserBeamPrefab;

        GameObject LaserBeam;

        WeaponController weapon;

        // Start is called before the first frame update
        void Start()
        {
            weapon = GetComponent<WeaponController>();

            weapon.showingWeapon += show;

            LaserBeam = Instantiate(LaserBeamPrefab, Muzzle.position, Quaternion.identity);

            LaserBeam.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (!LaserBeam.active)
            {
                return;
            }

            LaserBeam.transform.position = Muzzle.position;
        }

        void show(bool show)
        {
            LaserBeam.SetActive(show);
        }
    }
}
