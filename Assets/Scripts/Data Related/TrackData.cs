using System.Collections;
using System.Collections.Generic;
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
        IDGenerator.GetNextId();
        dataContainer.type = selectionType == SelectionType.LASER ? "Laser" : "Bubble";
        DontDestroyOnLoad(this.gameObject);
    }


    void Update(){
        if (OVRInput.GetDown(OVRInput.Button.Any)) {
            dataContainer.inputs++;
        }

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level2 Nav1")) {
            ParseRowDataToExporter();
        }
    }

    private void ParseRowDataToExporter() {
        dataContainer.time = (int) Time.time;

        string[] rowData = new string[5];
        rowData[0] = dataContainer.type;
        rowData[1] = dataContainer.inputs.ToString();
        rowData[2] = dataContainer.mistakes.ToString();
        rowData[3] = dataContainer.time.ToString();
        rowData[4] = dataContainer.skippedLevel.ToString();

        ExportDataUtil.SaveDataToCsv(rowData);
        Destroy(gameObject);
    }
}
