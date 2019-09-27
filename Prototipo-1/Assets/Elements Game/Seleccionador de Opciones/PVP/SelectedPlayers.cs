﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototipo_2
{
    public class SelectedPlayers : MonoBehaviour
    {
        // ESTE SCRIPT DEBE COMUNICAR AL STRUCT DEL GAME MANAGER LAS SELECCIONES DE LOS JUGADORES (tanto player1 como player2)
        struct CursorMatriz
        {
            public int x;
            public int y;
            public bool condirmed;
        }
        public string nameNextScene;
        public List<string> namePlayersOptions;
        public Sprite spriteCursorSelectorPlayer1;
        public Sprite spriteCursorSelectorPlayer2;
        private string[,] grillaDeSeleccion;
        public int filas;
        public int columnas;
        private int idOption;
        private CursorMatriz cursorPlayer1;
        private CursorMatriz cursorPlayer2;
        private GameManager gm;
        private void Start()
        {
            if (GameManager.instanceGameManager != null)
            {
                gm = GameManager.instanceGameManager;
            }
            idOption = 0;
            cursorPlayer1.x = 0;
            cursorPlayer1.y = 0;
            if (filas > 0 && columnas > 0)
            {
                grillaDeSeleccion = new string[filas, columnas];
                if (grillaDeSeleccion != null)
                {
                    for (int i = 0; i < filas; i++)
                    {
                        for (int j = 0; j < columnas; j++)
                        {
                            if (idOption < namePlayersOptions.Count)
                            {
                                grillaDeSeleccion[i, j] = namePlayersOptions[idOption];
                            }
                            idOption++;
                        }
                    }
                }
            }
            idOption = 0;
        }
        private void Update()
        {
            MoveCursor();
            CheckSelectCursor();
        }
        public void MoveCursor()
        {
            if (cursorPlayer1.x >= 0 && cursorPlayer1.x < filas)
            {
                if (InputPlayerController.Vertical_Button_P1() > 0 && cursorPlayer1.x < filas-1)
                {
                    cursorPlayer1.x++;
                }
                else if (InputPlayerController.Vertical_Button_P1() < 0 && cursorPlayer1.x > 0)
                {
                    cursorPlayer1.x--;
                }
            }
            if (cursorPlayer1.y >= 0 && cursorPlayer1.y < columnas)
            {
                if (InputPlayerController.Horizontal_Button_P1() > 0 && cursorPlayer1.x > 0)
                {
                    cursorPlayer1.y--;
                }
                else if (InputPlayerController.Horizontal_Button_P1() < 0 && cursorPlayer1.y < columnas-1)
                {
                    cursorPlayer1.y++;
                }
            }
        }
        public void CheckSelectCursor()
        {
            if (InputPlayerController.SelectButton_P1())
            {
                switch (grillaDeSeleccion[cursorPlayer1.x, cursorPlayer1.y])
                {
                    case "Balanceado":
                        gm.structGameManager.gm_dataCombatPvP.player1_selected = DataCombatPvP.Player_Selected.Balanceado;
                        cursorPlayer1.condirmed = true;
                        break;
                    case "Agresivo":
                        gm.structGameManager.gm_dataCombatPvP.player1_selected = DataCombatPvP.Player_Selected.Agresivo;
                        cursorPlayer1.condirmed = true;
                        break;
                    case "Defensivo":
                        gm.structGameManager.gm_dataCombatPvP.player1_selected = DataCombatPvP.Player_Selected.Defensivo;
                        cursorPlayer1.condirmed = true;
                        break;
                    case "Protagonista":
                        gm.structGameManager.gm_dataCombatPvP.player1_selected = DataCombatPvP.Player_Selected.Protagonista;
                        cursorPlayer1.condirmed = true;
                        break;
                    default:
                        cursorPlayer1.condirmed = false;
                        break;
                }
            }
            cursorPlayer2.condirmed = true; // SACAR ESTO Y REMPLAZARLO POR LO MISMO QUE HICE CON cursorPlayer1 PERO UTILIZANDO cursorPlayer2
            if (cursorPlayer1.condirmed && cursorPlayer2.condirmed)
            {
                SceneManager.LoadScene(nameNextScene);
            }
        }
        // ESTE SCRIPT DEBE COMUNICAR AL STRUCT DEL GAME MANAGER LAS SELECCIONES DE LOS JUGADORES (tanto player1 como player2)
    }
}