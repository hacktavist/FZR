using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : MonoBehaviour {

  public static void Save(){
    BinaryFormatter bf = new BinaryFormatter ();
    FileStream fs;
    if(File.Exists(Application.persistentDataPath + "/stats.txt")){
      fs = File.Create(Application.persistentDataPath + "/stats.txt");
    } else{
      fs = File.Create(Application.persistentDataPath + "/stats.txt");
    }

    Data d = new Data ();
    d.win = GameManager.wins;


    bf.Serialize (fs, d);
    fs.Close ();
  }

  public static void Load(){
    BinaryFormatter bf = new BinaryFormatter ();
    FileStream fs;
    if (File.Exists (Application.persistentDataPath + "/stats.txt")) {
      fs = File.Open (Application.persistentDataPath + "/stats.txt", FileMode.Open);
      Data d = (Data)bf.Deserialize (fs);
      fs.Close ();

      GameManager.wins = d.win;

    }



  }

}
[Serializable]
class Data {
  public int win;
  public bool playAsBrain;
  public bool noDmg;
}
