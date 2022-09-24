using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStone : MonoBehaviour
{
    [SerializeField] private float mouseDragPhysicsSpeed = 10;
    [SerializeField] private float mouseDragSpeed = 0.1f;
    [SerializeField] private GameObject linkTemplate;

    private GameObject objSelected = null;
    public List<HashSet<GameObject>> links = new();

    private Camera mainCamera;
    private Vector3 velocity = Vector3.zero;
    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    [HideInInspector] public static SelectStone Instance;

    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
    }
    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.attachedRigidbody != null && hit.collider.attachedRigidbody.gameObject.CompareTag("Draggable"))
            {
                objSelected = hit.collider.attachedRigidbody.gameObject;
                if (Input.GetMouseButtonDown(1) && objSelected.TryGetComponent(out ListLinks _))
                {
                    LineController line = Instantiate(linkTemplate).GetComponent<LineController>();
                    line.startObject = objSelected;
                    StartCoroutine(LineUpdate(objSelected, line));
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    hit.collider.attachedRigidbody.gameObject.transform.position = new Vector3(hit.collider.attachedRigidbody.gameObject.transform.position.x, 0.6f, hit.collider.attachedRigidbody.gameObject.transform.position.z);
                    StartCoroutine(DragUpdate(objSelected));
                }
            }
        }
        
    }

    private IEnumerator DragUpdate(GameObject clickedObject)
    {
        float initialDistance = Vector3.Distance(clickedObject.transform.position, mainCamera.transform.position);
        clickedObject.TryGetComponent<Rigidbody>(out var rb);
        if (rb != null) rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        if (clickedObject.TryGetComponent<Stone>(out Stone st)) clickedObject.transform.rotation = st.rotationInit;
        else clickedObject.transform.rotation = Quaternion.identity;


        while (Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (rb != null)
            {
                Vector3 direction = ray.GetPoint(initialDistance) - clickedObject.transform.position;
                rb.velocity = direction * mouseDragPhysicsSpeed;
                yield return waitForFixedUpdate;
            }
            else
            {
                clickedObject.transform.position = Vector3.SmoothDamp(clickedObject.transform.position,
                    ray.GetPoint(initialDistance), ref velocity, mouseDragSpeed);
                yield return null;
            }
        }

        if (rb != null) rb.velocity = Vector3.zero; rb.freezeRotation = false; rb.constraints = RigidbodyConstraints.None;
        
    }

    private IEnumerator LineUpdate(GameObject clickedObject, LineController line)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        while (Input.GetMouseButton(1))
        {
            float initialDistance = Vector3.Distance(clickedObject.transform.position, mainCamera.transform.position);
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            line.FollowLine(ray.GetPoint(initialDistance));
            yield return waitForFixedUpdate;
        }

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("this link doent' exist yet : " + LinkNotExist(hit.collider.gameObject, clickedObject));
            Debug.Log((hit.collider != null) + " has a collider ; is drag : " + hit.collider.gameObject.CompareTag("Draggable"));
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Draggable") && hit.collider.gameObject != clickedObject
                && LinkNotExist(hit.collider.gameObject, clickedObject))
            {

                HashSet<GameObject> tempLink = new();
                tempLink.Add(hit.collider.gameObject);
                tempLink.Add(clickedObject);
                links.Add(tempLink);

                line.endObject = hit.collider.gameObject;
                hit.collider.gameObject.GetComponent<ListLinks>().AddLinks(line);
                clickedObject.GetComponent<ListLinks>().AddLinks(line);
                Tableau.Instance.VerifyLink(hit.collider.gameObject, clickedObject, line);

                line.FollowLine(hit.collider.gameObject.transform.position); //replace link
            }
            else
            {
                Destroy(line.gameObject);
            }
        }
        else
        {
            Destroy(line.gameObject);
        }
    }

    private bool LinkNotExist(GameObject g1, GameObject g2)
    {
        foreach (HashSet<GameObject> allHash in links)
        {
            if (allHash.Contains(g1) && allHash.Contains(g2))
            {
                return false;
            }
        }
        return true;
    }

    public bool LinkExist(StoneData s1, StoneData s2)
    {
        foreach (HashSet<GameObject> allHash in links)
        {
            List<StoneData> tempStoneList = new();
            foreach (GameObject stone in allHash)
            {
                tempStoneList.Add(stone.GetComponent<Stone>().Data);
            }
            if ((tempStoneList[0].Equals(s1) && tempStoneList[1].Equals(s2))
                || (tempStoneList[0].Equals(s2) && tempStoneList[1].Equals(s1))) 
                return true;
        }
        return false;
    }

    public void RemoveLink(GameObject g1, GameObject g2)
    {
        if (LinkNotExist(g1, g2)) return;

        foreach (HashSet<GameObject> allHash in links)
        {
            if (allHash.Contains(g1) && allHash.Contains(g2))
            {
                links.Remove(allHash);
                return;
            }
        }
    }

    public void RemoveStone(StoneData data)
    {
        Stone[] foundStones = FindObjectsOfType<Stone>();
        foreach (Stone item in foundStones)
        {
            if (item.Data.Equals(data))
            {//item is the gameobject to remove
                item.links.RemoveAllLinks();
                Destroy(item.gameObject);
                return;
            }
        }
    }
}
