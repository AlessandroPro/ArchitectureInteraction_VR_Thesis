using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ActionLogger : MonoBehaviour
{
    private float time;
    private float finishTime;
    public string filename;

    public enum Actions { model_hand, model_buttons, model_move, model_hide, cp_hand, cp_buttons,
                          avatar_hand, avatar_buttons, teleport_model, teleport_local, orb_touch}

    private int[] actionsCount;

    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
        finishTime = 0;
        actionsCount = new int[System.Enum.GetNames(typeof(Actions)).Length];
    }

    // Update is called once per frame
    void Update()
    {
        time = Time.time;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            WriteStats();
        }
    }

    public void logAction(Actions action)
    {
        actionsCount[(int)action]++;
        //Debug.LogFormat("{0,-20}\t{1,-10}\t{2,-10}", action.ToString(), actionsCount[(int)action], time.ToString());

        float timeRounded = Mathf.Round(time * 100.0f) * 0.01f;
        string line = action.ToString() + "," + actionsCount[(int)action] + ","  + timeRounded.ToString();
        if(action == Actions.orb_touch)
        {
            line += ",********";
        }

        WriteString(line);

        if (actionsCount[(int)Actions.orb_touch] == 10)
        {
            finishTime = time;
            WriteStats();
        }
    }
    private void WriteStats()
    {
        WriteString("");
        WriteString("*****STATS*****");
        WriteString("");
        WriteString("FINISH_TIME," + (Mathf.Round(finishTime * 100.0f) * 0.01f).ToString());
        for (int i = 0; i < actionsCount.Length; i++)
        {
            WriteString((Actions)i + "," + actionsCount[i]);
        }
    }

    private void WriteString(string text)
    {
        string path = "Assets/Logs/" + filename;

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(text);
        writer.Close();
    }
}
