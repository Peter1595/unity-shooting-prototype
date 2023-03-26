using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{

    public class Stats<T> : MonoBehaviour
    {
        public T startingStat;
        private T stat;
        private T Stat
        {

            get
            {
                return stat;
            }
            set
            {
                onChanged?.Invoke(value, stat);

                stat = value;
            }
        }



        public UnityAction<T, T?> onChanged;
        // Start is called before the first frame update
        void Start()
        {
            Stat = startingStat;
        }




        public virtual void set(T value)
        {
            Stat = value;
        }
        public virtual T get()
        {
            return Stat;
        }

        public virtual void doWith(UnityAction<T> action)
        {
            action?.Invoke(Stat);
        }

    }
}
