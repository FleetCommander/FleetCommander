using System.Collections;
using System.Collections.Generic;
using Data_Related;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackData : MonoBehaviour{

    private const string ENDSCENE = "EndScene";
    
    enum SelectionType {
        LASER, BUBBLE
    }

    [SerializeField]
    private SelectionType selectionType;

    private DataContainer dataContainer;
    

    void Awake(){
        dataContainer = DataContainer.GetInstance();
        dataContainer.type = selectionType == SelectionType.LASER ? "Laser" : "Bubble";
        DontDestroyOnLoad(gameObject);
    }


    void Update(){
        if (OVRInput.GetDown(OVRInput.Button.Any)) {
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1)) {
                dataContainer.level1Inputs++;
            }
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(2)) {
                dataContainer.level2Inputs++;
            }
        }

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("EndScene")) {
            ParseRowDataToExporter();
        }
    }

    private void ParseRowDataToExporter() {
        dataContainer.id = Resources.Load<IDGenerator>("IDGenerator").GetNextId();

        string[] rowData = new string[10];
        rowData[0] = dataContainer.id.ToString();
        rowData[1] = dataContainer.type;
        rowData[2] = dataContainer.level1Inputs.ToString();
        rowData[3] = dataContainer.level1Mistakes.ToString();
        rowData[4] = dataContainer.level1Time.ToString();
        rowData[5] = dataContainer.level1SkippedLevel.ToString();
        rowData[6] = dataContainer.level2Inputs.ToString();
        rowData[7] = dataContainer.level2Mistakes.ToString();
        rowData[8] = dataContainer.level2Time.ToString();
        rowData[9] = dataContainer.level2SkippedLevel.ToString();
        
        ExportDataUtil.SaveDataToCsv(rowData);
        Destroy(gameObject);
    }
}
