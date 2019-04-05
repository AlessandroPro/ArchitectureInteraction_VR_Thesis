using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLogger : MonoBehaviour
{
    private float time;

    public enum Actions { model_hand, model_buttons, model_move, cp_hand, cp_buttons,
                          avatar_hand, avatar_buttons, teleport_model, teleport_local, orb_touch}

    private int[] actionsCount;

    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
        actionsCount = new int[System.Enum.GetNames(typeof(Actions)).Length];
    }

    // Update is called once per frame
    void Update()
    {
        time = Time.time;
    }

    public void logAction(Actions action)
    {
        actionsCount[(int)action]++;
        Debug.LogFormat("{0,-20}\t{1,-10}\t{2,-10}", action.ToString(), actionsCount[(int)action], time.ToString());
    }
    private void printStats()
    {

    }
}
