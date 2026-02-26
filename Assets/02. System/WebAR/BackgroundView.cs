/*
 * 작성자: Kim Bummoo
 * 작성일: 2025.03.03
 *
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FUTUREVISION.WebAR
{
    public class BackgroundView : BaseView
    {
        [Space(10)]
        [Header("Text")]
        public TextUIItem Message;

        public override void Initialize() 
        {
        }
    }
}
