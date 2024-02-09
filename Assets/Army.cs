using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Army : MonoBehaviour
{
    [SerializeField] private CrowdControl crowdControl;

    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private List<GameObject> spawnedUnits = new List<GameObject>();
    [SerializeField] private List<Vector3> points = new List<Vector3>();
    [SerializeField] private Transform parent;



    private void Start()
    {
        SetArmyPositions();

    }
    // Update is called once per frame
    void Update()
    {
        SetArmyPositions();

    }

    void SetArmyPositions(){
        points = crowdControl.PlaceUnits().ToList();
        
        // if our shape needs more soldier
        if(points.Count > spawnedUnits.Count)
        {
            var remaining = points.Skip(spawnedUnits.Count);
            SpawnUnit(remaining);
            
        }
        else if(points.Count < spawnedUnits.Count)
        {
            KillUnit(spawnedUnits.Count - points.Count);
        }

        for (int i = 0; i < spawnedUnits.Count; i++)
        {
            //spawnedUnits[i].transform.position = transform.position + points[i];
            spawnedUnits[i].transform.position = Vector3.MoveTowards(spawnedUnits[i].transform.position, transform.position + points[i], 4 * Time.deltaTime);
        }
    }
    void SpawnUnit(IEnumerable<Vector3> points)
    {
        foreach (var pos in points)
        {
            var unit = Instantiate(unitPrefab, transform.position + pos, Quaternion.identity, parent);
            spawnedUnits.Add(unit);
        }
    }

    void KillUnit(int num)
    {
        var unit = spawnedUnits.Last();
        spawnedUnits.Remove(unit);
        Destroy(unit.gameObject);
    }
}
