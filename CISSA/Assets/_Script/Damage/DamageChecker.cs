using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// This Class handles damage strikes and number of target allow to damage
/// i. single target single strike, where can only damage one target and one time
///     initialise number of target to 1
///     initialise allowed objects to 1 
/// ii.single target multiple strikes, where can damage one target multiple times
///     1. after dealt damage to one target, no new item can be added to the class
///     2. have to wait the timer to be reset
/// iii. multiple target single strike
///     1. in the single strike, there could be multiple target, and after that it can no longer damage more targes
/// iv. multiple target multiple strikes
///     1. constantly active
/// </summary>
public class DamageChecker
{

    private class DamagedObject
    {
        public GameObject tar;
        public float nextDamageTime;

        public DamagedObject(GameObject tar, float nextDamageTime)
        {
            this.tar = tar;
            this.nextDamageTime = nextDamageTime;
        }
    }
    private Queue<DamagedObject> damagedObjects = new();
    private int allowedDamagedObjects; public int AllowedDamagedObjects { get; private set; }
    private int numberOfStrikes; public int NumberOfStrikes { get; private set; }
    public float _damageFrequency = 0.5f;

    private float _totalAliveTime = 0f;
    private float _timeToDeleteNext = 0f;

    public bool pendingDestruction = false;


    public DamageChecker(int allowedDamagedObjects, int numberOfStrikes)
    {
        this.allowedDamagedObjects = allowedDamagedObjects;
        this.numberOfStrikes = numberOfStrikes;
        _totalAliveTime = 0f;
    }
    public DamageChecker(int allowedDamagedObjects, int numberOfStrikes, float damageFrequency)
    {
        this.allowedDamagedObjects = allowedDamagedObjects;
        this.numberOfStrikes = numberOfStrikes;
        _totalAliveTime = 0f;
        _damageFrequency = damageFrequency;
    }

    public bool AddDamagedObjects(GameObject tar)
    {

        //***********//
        //if initialise number of strikes to be less than 0, then it can keep adding more objects.
        //also initialise the frequence.
        if (damagedObjects.Count > allowedDamagedObjects) { //Debug.Log("Exceeded Allowed Objects");
                                                             return false; }
        if (numberOfStrikes == 0) { //Debug.Log("Exceeded Allowed Strikes total Object in the Queue " + damagedObjects.Count);
            return false; }
        //check if exceeded allowed number of targets damaged
        if (DoesTargetExistInQueue(tar)) { return false; }

        damagedObjects.Enqueue(new DamagedObject(tar, _damageFrequency + _totalAliveTime));
        //Debug.Log("Remaining Strikes" + numberOfStrikes + " after enqueued" + damagedObjects.Count) ;
        //Debug.Log(tar + " is enqueued at " + _totalAliveTime + "and to be deleted at" + (_damageFrequency + _totalAliveTime));
        //if there are no items before this one, then set the time to delete next
        if (damagedObjects.Count == 1)
        {
            _timeToDeleteNext = _damageFrequency + _totalAliveTime;
        }

        return true;
    }

    public void HitCounter()
    {
        numberOfStrikes -= 1;
    }


    public void Update(float deltaTime)
    {
        _totalAliveTime += deltaTime;
        UpdateRecentlyDamageObjects();
    }


    private void UpdateRecentlyDamageObjects()
    {
        float epsilon = 0.01f; // Define a small epsilon value for comparison

        while (damagedObjects.Count > 0 && _totalAliveTime >= _timeToDeleteNext)
        {
            DamagedObject tar = damagedObjects.Dequeue();
            // Process the damage logic for 'tar' here, e.g., apply damage

            if (damagedObjects.Count > 0)
            {
                // Check if the next damage time is very close to the current time
                if (_totalAliveTime - damagedObjects.Peek().nextDamageTime > epsilon)
                {
                    // If the difference is more than epsilon, set the next delete time
                    SetTimeToDeleteNext();
                    break;
                }
            }
        }
    }



    private bool DoesTargetExistInQueue(GameObject tar)
    {
        foreach(DamagedObject damagedObject in damagedObjects)
        {
            if (damagedObject.tar == tar)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// After delete the first item in the queue, and if there is more item in the queue, then peek the time to delete next;
    /// </summary>
    private void SetTimeToDeleteNext()
    {
        if (damagedObjects.Count > 0)
        {
            _timeToDeleteNext = damagedObjects.Peek().nextDamageTime;
            //Debug.Log(_timeToDeleteNext + " to delete next");
        }
    }
}
