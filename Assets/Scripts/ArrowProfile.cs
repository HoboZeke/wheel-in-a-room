using UnityEngine;

[CreateAssetMenu(fileName = "ArrowProfile", menuName = "ScriptableObjects/ArrowProfile")]
public class ArrowProfile : ScriptableObject
{
    [SerializeField] string arrowName;
    [SerializeField] string arrowDescription;
    [SerializeField] Mesh arrowMesh;
    [SerializeField] Material arrowMaterial;
    [SerializeField] Arrow.ArrowTag[] tags;
    [SerializeField] WheelSegment.SegmentColour segmentColour;
    [SerializeField] int startHP;


    public string ArrowName { get { return arrowName; } set { arrowName = value; }}
    public string ArrowDescription { get { return arrowDescription; } set { arrowDescription = value; }}
    public Mesh ArrowMesh { get { return arrowMesh; }  set { arrowMesh = value; } }
    public Material ArrowMaterial { get { return arrowMaterial; } set { arrowMaterial = value; } }
    public WheelSegment.SegmentColour SegmentColour { get { return segmentColour; } set { segmentColour = value; } }
    public bool RewardsSegmentUnderArrow {
        get
        {
            foreach (Arrow.ArrowTag tag in tags) 
            {
                if (tag == Arrow.ArrowTag.Scorer) return true;
                else if (tag == Arrow.ArrowTag.PickyScorer) return true;
            }

            return false;
        }
    }
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

    public bool HasTag(Arrow.ArrowTag tag)
    {
        foreach(Arrow.ArrowTag t in tags)
        {
            if(t == tag) return true;
        }
        return false;
    }
}
