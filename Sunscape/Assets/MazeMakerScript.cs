using UnityEngine;
using System.Collections;

public class MazeMakerScript : MonoBehaviour
{
    public int sideLength;
    public float cubeSpacing;
    GameObject[,,] CubeMatrix;
    IntVector[,,] PathMatrix;

    int MaxPos {  get { return sideLength - 1; } }
    

    //Physical Details
    public GameObject CubeTemplate;
    float TimeBetweenDirectionChange = 10f;
    float TimeRemaining = 1f;


    void Start()
    {
        CreateCubes();
        CreatePath();
    }
    
    // Update is called once per frame
    void Update()
    {
        TimeRemaining -= Time.deltaTime;
        if (TimeRemaining <= 0)
        {
            TimeRemaining = TimeBetweenDirectionChange;
            AssignNewVelocities();
        }
    }

    void CreateCubes()
    {
        CubeMatrix = new GameObject[sideLength, sideLength, sideLength];

        


        for (int x = 0; x < sideLength; x++)
        {
            for (int y = 0; y < sideLength; y++)
            {
                for (int z = 0; z < sideLength; z++)
                {
                    GameObject newCube = Instantiate(CubeTemplate);
                    newCube.transform.position = new Vector3(x, y, z) * cubeSpacing;
                    CubeMatrix[x, y, z] = newCube;
                    newCube.name = x.ToString() + "" + y.ToString() + "" + z.ToString();
                    newCube.GetComponent<PrisonCube>().speed = cubeSpacing / TimeBetweenDirectionChange;
                }
            }
        }
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }


    int[] PullAmounts = { 0, 0, 2, 2, 4, 4, 6, 6, 8, 8, 10, 6, 6, 4, 4,2,2,1};
    void CreatePath()
    {
        PathMatrix = new IntVector[sideLength, sideLength, sideLength];
        
        IntVector currentPosition = new IntVector(0, 0, 0);
        bool positiveYDirection = true;
        bool invertedYLastTime = true;

        IntVector up = new IntVector(0, 1, 0);
        IntVector down = new IntVector(0, -1, 0);
        IntVector left = new IntVector(-1, 0, 0);
        IntVector right = new IntVector(1, 0, 0);
        IntVector forward = new IntVector(0, 0, 1);
        IntVector back = new IntVector(0, 0, -1);

        IntVector SheetEndPosition = new IntVector(9, 0, 0);

        int operations = 1000;
        int turnsMade = 0;
        Color basicColor = Color.yellow;
        Color capColor = Color.red;

        bool spiralOutJustHappened = false;
        bool spirallingOut = false;

        while (operations > 0)
        {
            operations--;
            IntVector direction = IntVector.zero;
            spiralOutJustHappened = false;

            if (currentPosition == SheetEndPosition) //Turn to new plane
            {
                if (operations < 3)
                {
                    PathMatrix[currentPosition.x, currentPosition.y, currentPosition.z] = currentPosition * -1;
                    return; //linkup the start and finish and quit
                }
                
                Vector3 yRotate = Quaternion.Euler(0, spirallingOut? 90:-90, 0) * right.ToVector3();
                right = new IntVector(yRotate);

                if (turnsMade == 10) // a bunch of shitty hacks
                {
                    basicColor = Color.blue;
                    capColor = Color.green;
                    right = new IntVector(0, 0, 1); 
                    direction = new IntVector(-1, 0, 0);
                    spiralOutJustHappened = true;
                    spirallingOut = true;
                    SheetEndPosition = new IntVector(3, 0, 6);
                    invertedYLastTime = true;
                    positiveYDirection = true;
                }
                else
                {
                    SheetEndPosition += right * (MaxPos - PullAmounts[turnsMade]);
                }

                Debug.Log(string.Format("Turn {0}: pull{1}, Position ({2},{3},{4})", turnsMade, PullAmounts[turnsMade], SheetEndPosition.x, SheetEndPosition.y, SheetEndPosition.z));
                Debug.Log(string.Format("Direction: {0},{1},{2}", direction.x, direction.y, direction.z));
                Debug.Log(string.Format("Next Capstone at: {0},{1},{2}", SheetEndPosition.x, SheetEndPosition.y, SheetEndPosition.z));
                GetCubeAtPoint(currentPosition).name += " Capstone ";
                GetCubeAtPoint(currentPosition).GetComponent<MeshRenderer>().material.color = Color.black;
               
                turnsMade++;
            }

            if (!spiralOutJustHappened)
            {

                if (!invertedYLastTime && (currentPosition.y == MaxPos || currentPosition.y == 0))
                {
                    positiveYDirection = !positiveYDirection; //move over a column
                    direction = right;
                    GetCubeAtPoint(currentPosition).GetComponent<MeshRenderer>().material.color = capColor;
                    invertedYLastTime = true;
                }
                else
                {//move up or down the column
                    direction = positiveYDirection ? up : down;
                    GetCubeAtPoint(currentPosition).GetComponent<MeshRenderer>().material.color = basicColor;
                    invertedYLastTime = false;
                }
            }

            PathMatrix[currentPosition.x, currentPosition.y, currentPosition.z] = direction;

          //  GetCubeAtPoint(currentPosition).name += " towards " + direction.x + "," + direction.y + "," + direction.z + "";
          //  GetCubeAtPoint(currentPosition).name += " Plane " + turnsMade;
            currentPosition += direction;

          //  if (SheetEndPosition.x > CubeMatrix.GetLength(0) || SheetEndPosition.y > CubeMatrix.GetLength(1) || SheetEndPosition.z > CubeMatrix.GetLength(2))
          //      Debug.Log("accessing shit dimension");
            GetCubeAtPoint(SheetEndPosition).GetComponent<MeshRenderer>().material.color = Color.cyan;
            // System.Threading.Thread.Sleep(100);
        }
    }

    IEnumerator Example()
    {
        print(Time.time);
        yield return new WaitForSeconds(0.1f);
        print(Time.time);
    }

    GameObject GetCubeAtPoint(IntVector position)
    {
        if (position.x > CubeMatrix.GetLength(0) || position.y > CubeMatrix.GetLength(1) || position.z > CubeMatrix.GetLength(2))
            Debug.Log("fucking what");
        return CubeMatrix[position.x, position.y, position.z];
    }

    IntVector GetPathAtPoint(IntVector position)
    {
        return PathMatrix[position.x, position.y, position.z];
    }


    void AssignNewVelocities()
    {
        GameObject[,,] newCubeMatrix = new GameObject[sideLength, sideLength, sideLength];
        
        for (int x = 0; x < sideLength; x++)
        {
            for (int y = 0; y < sideLength; y++)
            {
                for (int z = 0; z < sideLength; z++)
                {
                    GameObject currentCube = CubeMatrix[x, y, z];
                    if (currentCube != null)
                    {
                        
                            
                     //       .velocity = PathMatrix[x, y, z].ToVector3() * cubeSpacing / TimeBetweenDirectionChange;
                        IntVector newPosition = new IntVector(x, y, z) + PathMatrix[x, y, z];
                        currentCube.GetComponent<PrisonCube>().TargetPosition = newPosition.ToVector3() * cubeSpacing;
                        newCubeMatrix[newPosition.x, newPosition.y, newPosition.z] = currentCube;
                    }
                    
                }
            }
        }
        CubeMatrix = newCubeMatrix;
    }
}

public struct IntVector
{
    public int x;
    public int y;
    public int z;


    public IntVector(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public IntVector(Vector3 vector)
    {
        x = Mathf.RoundToInt(vector.x);
        y = Mathf.RoundToInt(vector.y);
        z = Mathf.RoundToInt(vector.z);
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public static IntVector operator +(IntVector A, IntVector B)
    {
        return new IntVector(A.x + B.x, A.y + B.y, A.z + B.z);
    }

    public static IntVector operator *(IntVector A, int coefficient)
    {
        return new IntVector(A.x * coefficient, A.y * coefficient, A.z * coefficient);
    }

    public static bool operator ==(IntVector A, IntVector B)
    {
        return A.x == B.x && A.y == B.y && A.z == B.z;
    }
    public static bool operator !=(IntVector A, IntVector B)
    {
        return A.x != B.x || A.y != B.y || A.z != B.z;
    }

    public static IntVector back { get { return new IntVector(0, 0, -1); } }
    public static IntVector down { get { return new IntVector(0, -1, 0); } }
    public static IntVector forward { get { return new IntVector(0, 0, 1); } }
    public static IntVector left { get { return new IntVector(-1, 0, 1); } }
    public static IntVector one { get { return new IntVector(1, 1, 1); } }
    public static IntVector right { get { return new IntVector(1, 0, 0); } }
    public static IntVector up { get { return new IntVector(0, 1, 0); } }
    public static IntVector zero { get { return new IntVector(0, 0, 0); } }
}