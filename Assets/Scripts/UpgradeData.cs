﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class UpgradeData
    {
        public string key;
        public string name;
        public Sprite icon;
        public int basePrice;
        public int currentLevel;
    }
}
