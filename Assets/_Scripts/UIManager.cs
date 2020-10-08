using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;//Me ayudara a formatear el texto de la vida en tiempo real dentro del Update

//TODO cambiar la llamada a los metodos de actualizar la interfaz por medio de
//eventos y no referenciando esta clase en el GameManager
public class UIManager : MonoBehaviour
{
    public static UIManager sharedInstance;

    private void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    #region inGame

    //Texto de la cant de monedas     
    public Text coinsText;
    //public TextMeshProUGUI coinsText;
    //Cuenta regresiva para reanudar el juego    
    //public TextMeshProUGUI countDownResumeText;
    //Boton de pausa    
    //public GameObject pauseButton;
    //Contenedor de la UI en el juego      
    public GameObject hud;
    #endregion

    #region inPause

    //Buttons
    //public GameObject exitButton;
    //public GameObject resumeButton;

    //Texto de la cant de monedas
    //public Text totalCoins;
    //public TextMeshProUGUI totalCoins;

    //Menus
    public GameObject pauseMenu;

    #endregion

    //StringBuilder sb = new StringBuilder();


    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
    }

    public void ResumeGame()
    {
        HidePauseMenu();
        GameManager.SI.SetGameState(GameState.inGame);
    }

    public void ExitGame()
    {
        //Play exit timeLine 
        //TODO ¿Que variables se mantendran?
    }

    //Metodo que actualiza la puntuacion en la hud    
    public void ResfreshTextScore(int currentCoins)
    {
        //Asignamos al objeto Texto la cant de monedas formateada a un digito
        coinsText.text = currentCoins.ToString("0");
    }

    /**
    public void UpdateTextScore(float score)
    {
        //Vamos a construir el texto en tiempo real para eso usamos esta funcion de la libreria mencionada arriba
        //StringBuilder sb = new StringBuilder("SCORE: ");//Palabras por defecto     
        sb.Append("SCORE: ");
        //Con Append le agregamos texto, en este caso se ira actualizando el score actual
        sb.AppendFormat("{0:0.0}",score);

        //Asignamos el texto final al componente
        scoreText.text = sb.ToString();

        sb.Clear();
    }
    **/

}
