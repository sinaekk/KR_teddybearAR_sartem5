/*
 * DATE     : 2024.11.27
 * AUTHOR   : Kim Bum Moo
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FUTUREVISION
{
    /// <summary>
    /// <para> </para>
    /// </summary>
    /// <typeparam name="FinalClass"></typeparam>
    public class BaseSingleton<FinalClass> : MonoBehaviour
        where FinalClass : BaseSingleton<FinalClass>
    {
        private static FinalClass instance;

        public static FinalClass Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindAnyObjectByType<FinalClass>();

                    if (instance == null)
                    {
                        Debug.LogWarning("There is no instance of " + typeof(FinalClass).Name);
                    }
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as FinalClass;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Debug.LogWarning("There are multiple instances of " + typeof(FinalClass).Name);
            }
        }
    }

}