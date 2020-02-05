
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using Oculus.Platform;
using Application = UnityEngine.Application;

public class ExportDataUtil {
   

     public static void SaveDataToCsv(string [] data){

        if (!File.Exists(getPath())) {
            createCSVFileWithHeader();
        }
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(string.Join(",", data));
        File.AppendAllText(getPath(), sb.ToString());

    }

    private static void createCSVFileWithHeader() {
        StringBuilder sbHeader = new StringBuilder();
        string[] rowHeader = new string[5];
        rowHeader[0] = "Type";
        rowHeader[1] = "Inputs";
        rowHeader[2] = "Mistakes(wrong portal)";
        rowHeader[3] = "Time in sec";
        rowHeader[4] = "Skipped a Level?";
        sbHeader.AppendLine(string.Join(",", rowHeader));
        File.WriteAllText(getPath(), sbHeader.ToString());
    }


    // Following method is used to retrive the relative path as device platform
    private static string getPath(){
        #if UNITY_EDITOR
        return Application.dataPath +"/StudyData/"+"Daten.csv";
        #elif UNITY_ANDROID
        return Application.persistentDataPath+"Saved_data.csv";
        #elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Saved_data.csv";
        #else
        return Application.dataPath +"/"+"Saved_data.csv";
        #endif
    }
    
}
