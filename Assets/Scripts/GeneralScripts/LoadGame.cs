using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadGame : MonoBehaviour
{
    void Awake()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            FB.Init(() =>
            {
                FB.ActivateApp();
            });
        }
    }
    void Start()
    {
        //if (PlayerPrefs.GetInt("TutorialCompleted") == 0)
        //{
        //    StartCoroutine("LoadNextScene", 2);//Tutorial
        //}
        StartCoroutine("LoadNextScene", 1);
    }
    IEnumerator LoadNextScene(int buildIndex)
    {
        yield return new WaitForSeconds(1);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildIndex);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    //void OnApplicationPause(bool pauseStatus)
    //{
    //    print("pause "+ Time.frameCount);
    //    print("f: OnApplicationPause");
    //    print("pauseStatus "+ pauseStatus);
    //    // Check the pauseStatus to see if we are in the foreground
    //    // or background
    //    if (!pauseStatus)
    //    {
    //        //app resume
    //        if (FB.IsInitialized)
    //        {
    //        print("OnApplicationPause already IsInitialized so activating");
    //            FB.ActivateApp();
    //        }
    //        else
    //        {
    //            print("OnApplicationPause not initialized so initalising and then activating");

    //            //Handle FB.Init
    //            FB.Init(() =>
    //            {
    //                FB.ActivateApp();
    //            });
    //        }
    //    }
    //}
}
