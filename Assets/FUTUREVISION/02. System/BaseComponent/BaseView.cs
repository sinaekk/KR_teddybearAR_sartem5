/*
 * DATE     : 2024.11.27
 * AUTHOR   : Kim Bum Moo
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FUTUREVISION
{
    public class BaseView : MonoBehaviour
    {
        public virtual void Initialize()
        {
        }

        public virtual void Show()
        {
            transform.gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            transform.gameObject.SetActive(false);
        }
    }
}