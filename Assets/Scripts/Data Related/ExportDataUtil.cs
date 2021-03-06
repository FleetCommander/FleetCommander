﻿
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
        string[] rowHeader = new string[10];
        rowHeader[0] = "ID";
        rowHeader[1] = "Type";
        rowHeader[2] = "Level1 Inputs";
        rowHeader[3] = "Level1 Mistakes";
        rowHeader[4] = "Level1 Time in sec";
        rowHeader[5] = "Level1 Skipped a Level?";
        rowHeader[6] = "Level2 Inputs";
        rowHeader[7] = "Level2 Mistakes";
        rowHeader[8] = "Level2 Time in sec";
        rowHeader[9] = "Level2 Skipped a Level?";
        
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
        return Application.dataPath +"/"+"StudienDaten.csv";
        #endif
    }
    
}
