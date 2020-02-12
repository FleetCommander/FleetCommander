using System;
using System.IO;
using UnityEngine;

namespace Data_Related{
public class IDGenerator{
    private static int id = 1;
    private const string filename = "IDNumber.dat";

    public static int GetNextId(){

        try{
            using (BinaryReader binaryReader = new BinaryReader(File.Open(GetPath(), FileMode.OpenOrCreate))){
            
                id = binaryReader.ReadInt32();
            
            
            }
        }
        catch (Exception e){
            
        }
        
        using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(GetPath(), FileMode.Create))){
            binaryWriter.Write(id + 1);
        }

        return id;
    }

    private static string GetPath(){
#if UNITY_EDITOR
        return Application.dataPath + "/StudyData/" + "filename";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"Saved_data.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Saved_data.csv";
#else
        return Application.dataPath +"/"+"filename";
#endif
    }
}
}