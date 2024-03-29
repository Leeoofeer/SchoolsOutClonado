﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototipo_2 { 
    public class ProyectilParabola : Proyectil
    {
        // Start is called before the first frame update
        public GameObject rutaParabola_AtaqueJugador;
        public GameObject rutaParabolaAgachado_AtaqueJugador;
        public GameObject rutaParabola_AtaqueEnemigo;
        public GameObject rutaParabolaAgachado_AtaqueEnemigo;
        [SerializeField]
        private ParabolaController parabolaController;
        private PoolObject poolObject;
        [HideInInspector]
        public int TypeRoot;
        void Start()
        {
            timeLife = auxTimeLife;
            if (GameManager.instanceGameManager != null)
            {
                gm = GameManager.instanceGameManager;
            }
        }
        private void OnEnable()
        {
            timeLife = auxTimeLife;
            OnParabola();
        }
        // Update is called once per frame
        void Update()
        {
            if (parabolaController != null)
            {
                CheckTimeLifeParabola();
            }
            CheckMovement();
        }
        public void CheckTimeLifeParabola()
        {
            if (timeLife > 0)
            {
                timeLife = timeLife - Time.deltaTime;
            }
            else if (timeLife <= 0)
            {
                //CAMBIARLO POR EL ATAQUE ESPECIAL QUE REALICE Y LUEGO LLAMAR A LA FUNCION CheckTimeLife();
                CheckTimeLife();
            }
        }
        public void CheckMovement()
        {
            if(rg2D.velocity.x <= 0 && rg2D.velocity.y <= 0)
            {
                Move(Vector3.down);
            }
        }
        public void Move(Vector3 direccion)
        {
            transform.Translate(direccion * speed * Time.deltaTime);
        }
        public void OnParabola()
        {
            // SE SELECIONA LA PARABOLA CORRESPONDIENTE DEPENDIENDO A DONDE APUNTO EL JUGADOR / ENEMIGO.
            // FALTARIA CREAR LAS PARABOLAS Y HACER EL GENERADOR DE PELOTAS CON PARABOLA Y PROBARLO.
            On(typeProyectil.Nulo);
            if (disparadorDelProyectil == DisparadorDelProyectil.Jugador1 || disparadorDelProyectil == DisparadorDelProyectil.Jugador2)
            {
                rutaParabola_AtaqueJugador.SetActive(true);
                switch (TypeRoot) {

                    case 1:
                        parabolaController.ParabolaRoot = rutaParabola_AtaqueJugador;
                        break;
                    case 2:
                        parabolaController.ParabolaRoot = rutaParabolaAgachado_AtaqueJugador;
                        break;
                }
                parabolaController.OnParabola();
            }
            else if (disparadorDelProyectil == DisparadorDelProyectil.Enemigo)
            {
                switch (TypeRoot)
                {
                    case 1:
                        rutaParabola_AtaqueEnemigo.SetActive(true);
                        parabolaController.ParabolaRoot = rutaParabola_AtaqueEnemigo;
                        break;
                    case 2:
                        rutaParabolaAgachado_AtaqueEnemigo.SetActive(true);
                        parabolaController.ParabolaRoot = rutaParabolaAgachado_AtaqueEnemigo;
                        break;
                }
                parabolaController.OnParabola();
            }       
            if (parabolaController != null)
            {
                parabolaController.Speed = speed;
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            switch (collision.tag)
            {
                case "Escudo":
                    timeLife = 0;
                    if (dobleDamage && timeLife == 0)
                    {
                        damage = damage / 2;
                        dobleDamage = false;
                    }
                    break;
            }
            
        }
    }
}