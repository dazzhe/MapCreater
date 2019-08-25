using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EditMap : MonoBehaviour
{
    private float size = 0.5f;
    private BlockRotation br;
    private GameObject map;
    private PhotonView myView;
    private Text oText;
    private EditColor ecolor;
    public Material[] material = new Material[10];
    private int objectCount;
    private float maxrange = 6f;
    private GameObject[] illusionCube = new GameObject[4];
    private GameObject hitEffect;
    private GameObject lastTarget;
    private float start;
    private float requiredTime = 0.2f;
    private float cooldown = 0.2f;
    private float count;
    private float lastTime;
    private int blockType = 1;
    public bool modeUI = false;
    private EditSize es;
    private Vector3 defaultPos = new Vector3(0, -100f, 0);


    private struct Target
    {
        public Vector3 pos;
        public GameObject obj;
    }

    private void Awake()
    {
        br = GameObject.Find("Canvas/Rotation").GetComponent<BlockRotation>();
        ecolor = GameObject.Find("Canvas/Color").GetComponent<EditColor>();
        myView = GetComponent<PhotonView>();
        oText = GameObject.Find("Canvas/Counter/Objects/Text").GetComponent<Text>();
        es = GameObject.Find("Canvas/Size").GetComponent<EditSize>();
        hitEffect = GameObject.Find("HitEffect");
        lastTime = Time.time;
        count = cooldown;
        start = Time.time;
        illusionCube[0] = GameObject.Instantiate(Resources.Load("illusionCube"), defaultPos, Quaternion.identity) as GameObject;
        illusionCube[1] = GameObject.Instantiate(Resources.Load("illusionTriangle"), defaultPos, Quaternion.identity) as GameObject;
        illusionCube[2] = GameObject.Instantiate(Resources.Load("illusionSlope"), defaultPos, Quaternion.identity) as GameObject;
        illusionCube[3] = GameObject.Instantiate(Resources.Load("illusionFlat"), defaultPos, Quaternion.identity) as GameObject;
    }

    private void Start()
    {
        StartCoroutine("counter");
    }

    private IEnumerator counter()
    {
        while (true)
        {
            objectCount = GameObject.FindGameObjectsWithTag("Block").Length;
            yield return new WaitForSeconds(1f);
        }
    }

    private void Update()
    {
        oText.text = "" + objectCount;
        if (Input.GetButtonDown("Change"))
        {
            modeUI = !modeUI;
        }
        if (Input.GetButtonDown("1"))
        {
            blockType = 1;
        }
        if (Input.GetButtonDown("2"))
        {
            blockType = 2;
        }
        if (Input.GetButtonDown("3"))
        {
            blockType = 3;
        }
        if (Input.GetButtonDown("4"))
        {
            blockType = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            blockType = 5;
        }
        count -= (Time.time - lastTime);
        lastTime = Time.time;
        if (modeUI)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Target target = SetTarget();
            if (Input.GetMouseButton(0) && IsTargetValid(target.pos))
            {
                CreateCube(target.pos);
                hitEffect.transform.position = defaultPos;
            }
            else if (Input.GetMouseButton(1) && IsTargetValid(target.pos))
            {
                DeleteCube(target);
            }
            else
            {
                ShowIllusion(target.pos);
                hitEffect.transform.position = defaultPos;
            }
        }
    }

    private Target SetTarget()
    {
        Target target = new Target(); ;
        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(center);
        bool b = Physics.Raycast(ray, out RaycastHit hit, maxrange);
        Vector3 rayVecor = hit.point - ray.origin;
        if (b && ((rayVecor.x * rayVecor.x + rayVecor.z * rayVecor.z) > 0.9f
                  || rayVecor.y > 0.5f || rayVecor.y < -1.6f))
        {
            target.pos = ray.origin + (hit.point - ray.origin) * 0.99f;
            target.obj = hit.collider.gameObject;
        }
        else
        {
            target.pos = defaultPos;
        }
        return target;
    }

    private void CreateCube(Vector3 pos)
    {
        if (count <= 0)
        {
            count = cooldown;
            float[] r = { es.w, es.d, es.h };
            Vector3 rot = new Vector3(br.x * 90, br.y * 90, br.z * 90);
            myView.RPC("ApplyChangesRPC", PhotonTargets.AllBuffered, new object[] { pos, blockType, ecolor.color, r, rot });
        }
    }

    [PunRPC]
    private void ApplyChangesRPC(Vector3 pos, int type, int color, float[] r, Vector3 rot)
    {
        GameObject cube;
        switch (type)
        {
            case 1:
                cube = GameObject.Instantiate(Resources.Load("Cube"), CalcPos(pos, r, Quaternion.Euler(rot)), Quaternion.Euler(rot)) as GameObject;
                break;
            case 2:
                cube = GameObject.Instantiate(Resources.Load("Triangle"), CalcPos(pos, r, Quaternion.Euler(rot)), Quaternion.Euler(rot)) as GameObject;
                break;
            case 3:
                cube = GameObject.Instantiate(Resources.Load("Slope"), CalcPos(pos, r, Quaternion.Euler(rot)), Quaternion.Euler(rot)) as GameObject;
                break;
            case 4:
                cube = GameObject.Instantiate(Resources.Load("Flat"), CalcPos(pos, r, Quaternion.Euler(rot)), Quaternion.Euler(rot)) as GameObject;
                break;
            case 5:
                cube = GameObject.Instantiate(Resources.Load("Vanguard"), CalcPos(pos, new float[] { 1f, 1f, 1f }, Quaternion.Euler(rot)), Quaternion.Euler(rot)) as GameObject;
                break;
            default:
                cube = GameObject.Instantiate(Resources.Load("Cube"), CalcPos(pos, r, Quaternion.Euler(rot)), Quaternion.Euler(rot)) as GameObject;
                break;
        }
        if (type != 5)
        {
            cube.transform.localScale = CalcSize(r);
        }
        cube.GetComponentInChildren<Renderer>().material = material[color - 1];
        cube.transform.GetChild(0).transform.parent = GameObject.Find("Map").transform;
        Destroy(cube);
    }

    private void ShowIllusionSub(Vector3 pos, int type)
    {
        float[] r = { es.w, es.d, es.h };
        Vector3 rot = new Vector3(br.x * 90, br.y * 90, br.z * 90);
        illusionCube[type].transform.position = CalcPos(pos, r, Quaternion.Euler(rot));
        illusionCube[type].transform.rotation = Quaternion.Euler(rot);
        illusionCube[type].transform.localScale = CalcSize(r);
    }

    private void ShowIllusion(Vector3 pos)
    {
        int i;
        for (i = 1; i <= illusionCube.Length; i++)
        {
            if (blockType == i)
            {
                ShowIllusionSub(pos, i - 1);
            }
            else
            {
                ShowIllusionSub(defaultPos, i - 1);
            }
        }
    }

    private Vector3 CalcPos(Vector3 pos, float[] r, Quaternion rot)
    {
        Vector3 spawnPos;
        Vector3 pos2 = pos / size;
        spawnPos = new Vector3(Mathf.Floor(pos2.x) + 0.5f, Mathf.Floor(pos2.y) + 0.5f, Mathf.Floor(pos2.z) + 0.5f);
        spawnPos *= size;
        Vector3 offset = rot * new Vector3(r[0] / 2f - 0.5f, r[1] / 2f - 0.5f, r[2] / 2f - 0.5f) * size;
        spawnPos += offset;
        return spawnPos;
    }

    private Vector3 CalcSize(float[] r)
    {
        return new Vector3(size * r[0], size * r[1], size * r[2]);
    }

    private void DeleteCube(Target target)
    {
        hitEffect.transform.position = target.pos;
        if (target.obj == lastTarget)
        {
            if (Time.time - start >= requiredTime && target.obj.tag != "Untagged")
            {
                myView.RPC("DeleteCubeRPC", PhotonTargets.AllBuffered, new object[] { transform.position, target.pos });
            }
        }
        else
        {
            lastTarget = target.obj;
            start = Time.time;
        }
    }

    [PunRPC]
    private void DeleteCubeRPC(Vector3 from, Vector3 to)
    {
        Ray ray = new Ray(from, (to - from).normalized);
        if (Physics.Raycast(ray, out RaycastHit hit, maxrange) && hit.collider.gameObject.tag != "Untagged")
        {
            if (hit.collider.transform.parent.name == hit.collider.name + "(Clone)")
            {
                Destroy(hit.collider.transform.parent.gameObject);
            }
            Destroy(hit.collider.gameObject);
        }
    }

    private bool IsTargetValid(Vector3 targetPos)
    {
        if (targetPos == defaultPos)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}