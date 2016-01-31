using UnityEngine;
using System.Collections;

public class StringManager : MonoBehaviour {

    public string[] instructions;
    public string[] epitaphs;
    public string[] livedLines;
    public int curr_stage;
    public int curr_iter;
    public int max_inters_per_age;
    public string minigameName;

    // Use this for initialization
    void Start () {
        curr_stage = (int)(MiniGameMgr.Instance.GetLifeStage());
        curr_iter = MiniGameMgr.Instance.GetMinigameRepeats(minigameName);
        int stringindex = (curr_stage * max_inters_per_age) + curr_iter;
        MiniGame.Instance.tombLine = epitaphs[stringindex];
        MiniGame.Instance.livedLine = livedLines[stringindex];
        MiniGame.Instance.SetInstruction(instructions[stringindex]);
    }
}
