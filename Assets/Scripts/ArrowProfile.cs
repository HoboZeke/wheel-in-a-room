using UnityEngine;

[CreateAssetMenu(fileName = "ArrowProfile", menuName = "ScriptableObjects/ArrowProfile")]
public class ArrowProfile : ScriptableObject
{
    [SerializeField] string arrowName;
    [SerializeField] string arrowDescription;
    [SerializeField] Mesh arrowMesh;
    [SerializeField] Material arrowMaterial;
    [SerializeField] Arrow.ArrowTag[] tags;
    [SerializeField] bool rewardsSegmentUnderArrow;
    [SerializeField] int startHP;


    public string ArrowName { get { return arrowName; } set { arrowName = value; }}
    public string ArrowDescription { get { return arrowDescription; } set { arrowDescription = value; }}
    public Mesh ArrowMesh { get { return arrowMesh; }  set { arrowMesh = value; } }
    public Material ArrowMaterial { get { return arrowMaterial; } set { arrowMaterial = value; } }
    public bool RewardsSegmentUnderArrow {  get { return rewardsSegmentUnderArrow; } set { rewardsSegmentUnderArrow = value; }    }
    public bool IsBrittle 
    {
        get
        {
            foreach(Arrow.ArrowTag tag in tags) { if (tag == Arrow.ArrowTag.Brittle) return true; }

            return false;
        }
    }
    public int StartHP { get { return startHP; } set { startHP = value; }}

    public string ArrowTags
    {
        get
        {
            string s = "";

            foreach (Arrow.ArrowTag tag in tags) { s += tag.ToString() + " "; }

            return s;
        }
    }
}
