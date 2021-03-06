using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SceneState
{
    idle,
    pass,
    fail
}

public class SlaveTrader : MonoBehaviour
{
    [Header("Open Character Image")]
    private bool isOpend;
    public GameObject SlaveTraderPanel;
    public GameObject SlaveTraderImage;

    [Header("NPC status TextList")]
    public Text Slave_price;
    public Text MasterCurrentMoney;
    public Text WarningText;

    [Header("Master Resource List")]
    public IntValue MasterMoney;

    [Header("Slave List")]
    public GameObject SlaveList; //? this design ok??? hmmmmm

    public Transform SlaveTraderPos;

    private float CheckDistance_ = 0.5f;
    private SceneState sceneState;
    private int SlavePriceRandom = 100; // Temporary
    private string[] WarningList = {"Not enough Money"};

    // Start is called before the first frame update
    void Start()
    {
        isOpend = false;
        Debug.Log("SlaveTrader Start()\n");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = transform.position.z;

            var pos1 = transform.position.x - CheckDistance_;
            var pos2 = transform.position.x + CheckDistance_;
            var pos3 = transform.position.y - CheckDistance_;
            var pos4 = transform.position.y + CheckDistance_;

            if (pos.x >= pos1 && pos.x <= pos2 && pos.y <= pos4 && pos.y >= pos3)
            {
                Debug.Log("pos1 " + pos1 + "pos2 " + pos2 + "pos3 " + pos3 + "pos4 " + pos4);
                if (sceneState == SceneState.idle) //How to SceneState state share??? make class?
                {
                    OpenSlaveTraderPanel();
                }
            }
        }
    }
    private void OpenSlaveTraderPanel()
    {
        isOpend = !isOpend;

        if (isOpend)
        {
            SlaveTraderPanel.SetActive(true);
            SlaveTraderImage.SetActive(true);
            OpenTextSlaveTraderStat();
            //Time.timeScale = 1f;
        }
        else
        {
            SlaveTraderPanel.SetActive(false);
            SlaveTraderImage.SetActive(false);
            //Time.timeScale = 1f;
        }
    }

    public void CloseSlaveTraderPanel()
    {
        SlaveTraderPanel.SetActive(false);
        SlaveTraderImage.SetActive(false);
        //Time.timeScale = 1f;
        isOpend = false;
    }

    private void OpenTextSlaveTraderStat()
    {
        // Random Slave List and Slave Price
        
        // Master Current Resource List..
        MasterCurrentMoney.text = MasterMoney.RuntimeValue;
    }

    public void SlaveBuyBtn()
    {
        // Master Gold >= Slave Price?
        if(MasterMoney.RuntimeValue >= SlavePriceRandom) 
        {
            MasterMoney.RuntimeValue -= SlavePriceRandom;
            CreateNewSlave();
        }
        else
        {
            WarningText.text = WarningList[0];
        }
    }

    private void CreateNewSlave()
    {
        Vector3 SlavePos = transform.position;
        SlavePos.x = SlavePos.x + 5;
        Instantiate(SlaveList, SlavePos.position, Quaternion.identity);
        //How to Slave Stat Resource management??
    }
}
