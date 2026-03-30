using UnityEngine;


[CreateAssetMenu(fileName = "RewardProfile", menuName = "ScriptableObjects/RewardProfile")]
public class PostOption : ScriptableObject
{
    [SerializeField] string postName;
    [SerializeField] string postDescription;

    public string PostName {  get { return postName; } set { postName = value; } }
    public string PostDescription { get { return postDescription; } set { postDescription = value; } }
}
