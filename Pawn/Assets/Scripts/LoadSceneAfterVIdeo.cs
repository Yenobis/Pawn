 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.Video;
 using UnityEngine.SceneManagement;
 
 public class LoadSceneAfterVIdeo : MonoBehaviour
 { 
      public VideoPlayer VideoPlayer; // Drag & Drop the GameObject holding the VideoPlayer component
      public string Escena;     
     void Start() 
     {
          VideoPlayer.loopPointReached += LoadScene;
     }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)){
            SceneManager.LoadScene(Escena);
        }

        if (Input.GetKeyDown("b")){
            SceneManager.LoadScene(Escena);
        }



    }
    void LoadScene(VideoPlayer vp)
     {
          SceneManager.LoadScene( Escena );
      }
  }
