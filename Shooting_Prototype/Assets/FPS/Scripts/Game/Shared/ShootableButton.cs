using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{

    public class ShootableButton : MonoBehaviour
    {
        public UnityAction<GameObject> OnClick;

        public void OnShoot(GameObject Shooter)
        {

            OnClick?.Invoke(Shooter);
        }
    }
}
