﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototipo_2
{
    public class StructsPlayer : MonoBehaviour
    {
        // Start is called before the first frame update
        public struct DataPlayer
        {
            public int CantCasillasOcupadas_X;
            public int CantCasillasOcupadas_Y;
            public int columnaActual;
        }

        public GameObject ruta1;
        public GameObject ruta2;
        public GameObject ruta3;
        public DataPlayer dataPlayer;
    }
}
