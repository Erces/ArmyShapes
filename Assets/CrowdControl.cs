using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrowdControl : MonoBehaviour
{
    public enum ArmyType {RECTANGLE,TRIANGLE,CIRCLE,ARROW}
    public ArmyType selectedType;

    public List<Unit> units;
    [SerializeField,Range(0,10)] private int unitVertical;
    [SerializeField,Range(0,10)] private int unitHorizontal;
    [SerializeField] private float rangeBetween;

    [SerializeField] private bool isOdd = false;

    [Header("UI")]
    [SerializeField] private TMP_InputField verticalInput;
    [SerializeField] private TMP_InputField horizontalInput;
    [SerializeField] private TMP_InputField rangeBetweenInput;
    [Header("Circle Settings")]
    [SerializeField] private int amount;
    [SerializeField] private int ringOffset;
    [SerializeField] private int radius;
    [SerializeField] private float radiusGrowth;
    [SerializeField] private int rotations;
    [SerializeField] private int rings;




    public IEnumerable<Vector3> PlaceUnits(){

        switch (selectedType)
        {
            case ArmyType.CIRCLE:

                var amountPerRing = amount/rings;
                var _ringOffset = 0f;
                for (int i = 0; i < rings; i++)
                {
                    for (int j = 0; j < amountPerRing; j++)
                    {
                        var angle = j * Mathf.PI * (2 * rotations) / amountPerRing;
                        var _radius = radius + _ringOffset + j * radiusGrowth;
                        var x = Mathf.Cos(angle) * _radius;
                        var z = Mathf.Sin(angle) * _radius;

                        var position = new Vector3(x, 0, z);
                        yield return position;
                    }
                    _ringOffset += ringOffset;
                }
                

                break;
            case ArmyType.RECTANGLE:
                var centerPosRectangle = new Vector3(unitVertical * 0.5f, 0, unitHorizontal * 0.5f);
                Debug.Log("RECTANGLE");

                for (int x = 0; x < unitHorizontal; x++)
                {
                    for (int z = 0; z < unitVertical; z++)
                    {
                        var pos = new Vector3(x, 0, z);

                        pos -= centerPosRectangle;
                        pos *= rangeBetween;
                        yield return pos;

                    }
                }
                break;
            case ArmyType.ARROW:
                Debug.Log("TRIANGLE");
                int rectangleCount;
                int arrowTipCount;
                var centerPos = new Vector3(0, 0, 0);
                if (unitHorizontal % 2 == 0)
                {
                    Debug.Log("Even");
                    isOdd = false;
                    arrowTipCount = (unitHorizontal) / 2;
                    rectangleCount = unitVertical - arrowTipCount;

                }
                else
                {
                    Debug.Log("Odd");
                    isOdd = true;
                    arrowTipCount = (unitHorizontal + 1) / 2;
                    rectangleCount = unitVertical - arrowTipCount;
                }
                for (int x = 0; x < unitHorizontal; x++)
                {
                    for (int z = 0; z < rectangleCount; z++)
                    {
                        var pos = new Vector3(x, 0, z);

                        pos -= centerPos;
                        pos *= rangeBetween;
                        yield return pos;

                    }
                }

                for (int k = -1,x = 0; x < unitHorizontal; x++)
                {
                    if (isOdd)
                    {
                        k++;
                        if (x < unitHorizontal / 2)
                        {
                            
                            for (int z = arrowTipCount; z >= arrowTipCount - k && arrowTipCount - k > 0; z--)
                            {
                                Debug.Log("Arrow");
                                var pos = new Vector3(x, 0, Mathf.Abs(z-arrowTipCount) + rectangleCount);

                                pos -= centerPos;
                                pos *= rangeBetween;

                                yield return pos;
                            }
                        }
                        if(x == (unitHorizontal + 1) / 2)
                        {
                            for (int z = 0; z < arrowTipCount; z++)
                            {
                                Debug.Log("Arrow");
                                var pos = new Vector3(x-1, 0, z+rectangleCount);

                                pos -= centerPos;
                                pos *= rangeBetween;
                                k = -1;
                                yield return pos;
                            }
                           
                        }
                        if(x > unitHorizontal / 2)
                        {
                            for (int z = 0; z < arrowTipCount-2-k ; z++)
                            {
                                Debug.Log("K: " + k);
                                Debug.Log("ArrowLast");
                                var pos = new Vector3(x, 0, z + rectangleCount);

                                pos -= centerPos;
                                pos *= rangeBetween;

                                yield return pos;
                            }
                            
                        }
                    }
                    else
                    {
                        k++;
                        if (x < (unitHorizontal / 2)-1)
                        {

                            for (int z = arrowTipCount; z >= arrowTipCount - k && arrowTipCount - k > 0; z--)
                            {
                                Debug.Log("Arrow");
                                var pos = new Vector3(x, 0, Mathf.Abs(z - arrowTipCount) + rectangleCount);

                                pos -= centerPos;
                                pos *= rangeBetween;

                                yield return pos;
                            }
                        }
                        if (x == (unitHorizontal) / 2 || x == (unitHorizontal / 2) + 1)
                        {
                            for (int z = 0; z < arrowTipCount; z++)
                            {
                                Debug.Log("ArrowMiddle");
                                var pos = new Vector3(x-1, 0, z + rectangleCount);

                                pos -= centerPos;
                                pos *= rangeBetween;
                                k = -1;
                                yield return pos;
                            }

                        }
                        if (x > unitHorizontal / 2)
                        {
                            for (int z = 0; z < arrowTipCount - 2 - k; z++)
                            {
                                Debug.Log("K: " + k);
                                Debug.Log("ArrowLast");
                                var pos = new Vector3(x, 0, z + rectangleCount);

                                pos -= centerPos;
                                pos *= rangeBetween;

                                yield return pos;
                            }

                        }
                    }
                    
                    

                    
                    
                }
                break;

            case ArmyType.TRIANGLE:
                break;
 
        }
        
    }

    public void SetRectangle()
    {
        selectedType = ArmyType.RECTANGLE;
    }
    public void SetArrow()
    {
        selectedType = ArmyType.ARROW;
    }

    public void SetVertical()
    {
        unitVertical = int.Parse(verticalInput.text);
    }
    public void SetHorizontal()
    {
        unitHorizontal = int.Parse(horizontalInput.text);

    }
    public void SetRangeBetween()
    {
        rangeBetween = int.Parse(rangeBetweenInput.text);
    }

}
