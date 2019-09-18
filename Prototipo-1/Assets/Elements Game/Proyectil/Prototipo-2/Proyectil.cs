﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Prototipo_2
{
    public class Proyectil : MonoBehaviour
    {
        public enum DisparadorDelProyectil
        {
            Nulo,
            Enemigo,
            Jugador,
        }
        public enum TypeShoot
        {
            Recto,
            EnParabola,
            Nulo,
        }
        public float speed;
        public float timeLife;
        public float auxTimeLife;
        public float damage;
        public Rigidbody2D rg2D;
        public Transform vectorForward;
        public Transform vectorForwardUp;
        public Transform vectorForwardDown;
        public Pool pool;
        protected bool dobleDamage;
        private PoolObject poolObject;
        protected GameManager gm;
        public DisparadorDelProyectil disparadorDelProyectil;
        private void Start()
        {
            if (GameManager.instanceGameManager != null)
            {
                gm = GameManager.instanceGameManager;
            }
        }
        private void OnEnable()
        {
            timeLife = auxTimeLife;
            
        }
        private void Update()
        {
            CheckTimeLife();
        }
        public void On()
        {
            poolObject = GetComponent<PoolObject>();
            rg2D.velocity = Vector2.zero;
            rg2D.angularVelocity = 0;
            timeLife = auxTimeLife;
        }
        public void CheckTimeLife()
        {
            if (timeLife > 0)
            {
                timeLife = timeLife - Time.deltaTime;
            }
            else if (timeLife <= 0)
            {
                if (poolObject != null)
                {
                    poolObject.Recycle();
                }
            }
        }
        public void ShootForward()
        {
            rg2D.AddForce(transform.right * speed, ForceMode2D.Force);
        }
        public void ShootForwardUp()
        {
            rg2D.AddRelativeForce(vectorForwardUp.right * speed);
        }
        public void ShootForwardDown()
        {
            rg2D.AddRelativeForce(vectorForwardDown.right * speed, ForceMode2D.Force);
        }
        public PoolObject GetPoolObject()
        {
            return poolObject;
        }
        public void SetDobleDamage(bool _dobleDamage)
        {
            dobleDamage = _dobleDamage;
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            switch (collision.tag)
            {
                case "Escudo":
                    timeLife = 0;
                    if (dobleDamage)
                    {
                        damage = damage / 2;
                        dobleDamage = false;
                    }
                    break;
                case "Cuadrilla":
                    Cuadrilla cuadrilla = collision.GetComponent<Cuadrilla>();
                    if (cuadrilla.enemy == null && cuadrilla.player == null || cuadrilla.enemy != null && cuadrilla.player != null)
                    {
                        return;
                    }
                    if (cuadrilla.enemy != null)
                    {
                        if (cuadrilla.GetStateCuadrilla() == Cuadrilla.StateCuadrilla.Ocupado && cuadrilla.enemy.GetIsDeffended())
                        {
                            cuadrilla.SetStateCuadrilla(Cuadrilla.StateCuadrilla.Defendido);
                        }
                    }
                    if (cuadrilla.GetStateCuadrilla() == Cuadrilla.StateCuadrilla.Ocupado)
                    {
                        if (dobleDamage)
                        {
                            damage = damage / 2;
                            dobleDamage = false;
                        }
                        if (cuadrilla.enemy != null)
                        {
                            if (disparadorDelProyectil == DisparadorDelProyectil.Jugador)
                            {
                                if (cuadrilla.enemy.enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.MoveToPointCombat && cuadrilla.enemy.enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.MoveToPointDeath)
                                {
                                    cuadrilla.enemy.life = cuadrilla.enemy.life - damage;
                                }
                                timeLife = 0;
                            }
                        }
                        if (cuadrilla.player != null)
                        {
                            if (disparadorDelProyectil == DisparadorDelProyectil.Enemigo)
                            {
                                cuadrilla.player.SetEnableCounterAttack(true);
                                if (cuadrilla.player.delayCounterAttack > 0)
                                {
                                    cuadrilla.player.delayCounterAttack = cuadrilla.player.delayCounterAttack - Time.deltaTime;
                                    if (Input.GetKey(cuadrilla.player.ButtonDeffence))
                                    {
                                        cuadrilla.player.Attack();
                                        timeLife = 0;
                                        cuadrilla.player.delayCounterAttack = cuadrilla.player.GetAuxDelayCounterAttack();
                                    }
                                }
                                if (cuadrilla.player.delayCounterAttack <= 0 && timeLife > 0)
                                {
                                    cuadrilla.player.delayCounterAttack = cuadrilla.player.GetAuxDelayCounterAttack();
                                    cuadrilla.player.SetEnableCounterAttack(false);
                                    cuadrilla.player.life = cuadrilla.player.life - damage;
                                    timeLife = 0;
                                }
                                else if (cuadrilla.player.delayCounterAttack <= 0)
                                {
                                    cuadrilla.player.life = cuadrilla.player.life - damage;
                                }
                            }
                            
                        }
                    }
                    if (cuadrilla.GetStateCuadrilla() == Cuadrilla.StateCuadrilla.Defendido)
                    {
                        if (cuadrilla.player != null)
                        {
                            if (disparadorDelProyectil == DisparadorDelProyectil.Enemigo)
                            {
                                float realDamage = damage - cuadrilla.player.pointsDeffence;
                                cuadrilla.player.life = cuadrilla.player.life - realDamage;
                                timeLife = 0;
                            }
                        }
                        if (cuadrilla.enemy != null)
                        {
                            if (disparadorDelProyectil == DisparadorDelProyectil.Jugador)
                            {
                                float realDamage = damage - cuadrilla.enemy.pointsDeffence;
                                cuadrilla.enemy.life = cuadrilla.enemy.life - realDamage;
                                timeLife = 0;
                            }
                        }
                    }
                    break;
            }
        }
    }
}
