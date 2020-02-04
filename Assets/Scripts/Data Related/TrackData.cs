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
        dataContainer = DataContainer.getInstance();
        dataContainer.id = IDGenerator.GetNextId();
        dataContainer.type = selectionType == SelectionType.LASER ? "Laser" : "Bubble";
        DontDestroyOnLoad(this.gameObject);
    }


    void Update(){
        if (OVRInput.GetDown(OVRInput.Button.Any)) {
            dataContainer.inputs++;
        }

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("EndScene")) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                ParseRowDataToExporter();
            }

        }
    }

    private void ParseRowDataToExporter() {
        dataContainer.time = Time.time;

        string[] rowData = new string[6];
        rowData[0] = dataContainer.id.ToString();
        rowData[1] = dataContainer.type;
        rowData[2] = dataContainer.inputs.ToString();
        rowData[3] = dataContainer.mistakes.ToString();
        rowData[4] = dataContainer.time.ToString();
        rowData[5] = dataContainer.skippedLevel.ToString();

        ExportDataUtil.SaveDataToCsv(rowData);
        Destroy(gameObject);
    }
}
