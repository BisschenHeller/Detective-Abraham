using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class CompromiseHandler : MonoBehaviour
{
    private static Queue<CompromiseID> newCompromisesQueue = new Queue<CompromiseID>();

    private List<Compromise> compromises = new List<Compromise>();

    public GameObject compromisePrefab = null;

    public static void EnqueueCompromise(CompromiseID newCompromise)
    {
        newCompromisesQueue.Enqueue(newCompromise);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CompromiseHandler.EnqueueCompromise(CompromiseID.acknowledge_dirty_kitchen);
        }
        while (newCompromisesQueue.Count != 0)
        {
            CompromiseID newCompromise = newCompromisesQueue.Dequeue();
            //if (compromises.Exists(n => n.id == newCompromise)) continue;
            compromises.Add(new Compromise(newCompromise, true));
            GameObject compPrefab = Instantiate(compromisePrefab, this.transform);
            compPrefab.GetComponent<CompromiseMono>().SetCompromiseID(newCompromise);
            compPrefab.transform.localPosition = Vector2.zero + new Vector2(0, - (compromises.Count - 1) * 20);
        }
    }
}

public class Compromise
{
    public Compromise(CompromiseID eidi, bool act)
    {
        active = act;
        id = eidi;
    }
    public bool active = false;
    public CompromiseID id = CompromiseID.NONE;
}
public enum CompromiseID
{
    NONE = 0,

    acknowledge_dirty_kitchen = 1,
    acknowledge_joshuas_death = 2,
    acknowledge_wrong_clock = 3,
}