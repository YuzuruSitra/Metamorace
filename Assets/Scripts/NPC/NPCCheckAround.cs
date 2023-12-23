using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCheckAround : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 _upPadding = new Vector3(0f, 0.5f, 0f);
    float RightdistanceToHitObject = 0;
    float LeftdistanceToHitObject = 0;
    [SerializeField]
    float MaxBlockRayDist = 8.0f;
    [SerializeField]
    private float _npcWallReach = 1.0f;

    float _verticalRayOffset = 2.0f;
    float _vericalAvoidRay = 6.0f;

    void Update()
    {

    }

    //近いブロックを壊しに行く
    public string CheckArroundBlock()
    {
        // Debug.Log(RightdistanceToHitObject);
        // Debug.Log(LeftdistanceToHitObject);
        Ray Rightray = new Ray(transform.position + _upPadding, Vector3.right);
        Ray Leftray = new Ray(transform.position + _upPadding, Vector3.left);
        Debug.DrawRay(transform.position + _upPadding, Vector3.right * MaxBlockRayDist, Color.blue, 0.1f);
        Debug.DrawRay(transform.position + _upPadding, Vector3.left * MaxBlockRayDist, Color.yellow, 0.1f);

        if (Physics.Raycast(Rightray, out RaycastHit righthit, MaxBlockRayDist))
        {
            if (righthit.collider.CompareTag("Ambras"))
            {
                //右のブロックまでの距離
                RightdistanceToHitObject = righthit.distance;
            }
        }
        if (Physics.Raycast(Leftray, out RaycastHit lefthit, MaxBlockRayDist))
        {
            if (lefthit.collider.CompareTag("Ambras"))
            {
                //左のブロックまでの距離
                LeftdistanceToHitObject = lefthit.distance;
            }
        }
        //左右のレイが両方ブロックに当たていない場合
        if (LeftdistanceToHitObject == MaxBlockRayDist && RightdistanceToHitObject == MaxBlockRayDist)
        {
            RightdistanceToHitObject = MaxBlockRayDist;
            LeftdistanceToHitObject = MaxBlockRayDist;
            return "Null";
        }
        else if (LeftdistanceToHitObject <= RightdistanceToHitObject)
        {
            RightdistanceToHitObject = MaxBlockRayDist;
            LeftdistanceToHitObject = MaxBlockRayDist;
            return "Left";
        }

        else
        {
            RightdistanceToHitObject = MaxBlockRayDist;
            LeftdistanceToHitObject = MaxBlockRayDist;
            return "Right";
        }
    }

    public bool CheckWall()
    {
        //壁検出Ray
        Vector3 direction = transform.forward;
        direction.Normalize();
        Ray ray = new Ray(transform.position + _upPadding, direction);
        Debug.DrawRay(transform.position + _upPadding, direction, Color.green, 0.3f);
        if (Physics.Raycast(ray, out RaycastHit hit, _npcWallReach) && hit.collider.CompareTag("Wall"))
        {
            return true;
        }
        else return false;
    }

    //縦の圧死を防ぐレイ
    public bool CheckVerticalDeathBlock()
    {
        bool _overBlock = false;
        //頭上検出Ray
        //頭上にブロックがあればよける、両方がブロックで挟まれている場合はジャンプ
        Vector3 rayOrigin = transform.position + Vector3.up * _verticalRayOffset;
        Ray rayover = new Ray(rayOrigin, Vector3.up);
        Debug.DrawRay(rayover.origin, rayover.direction * _vericalAvoidRay, Color.green);

        if (Physics.Raycast(rayover, out RaycastHit hitblock, _vericalAvoidRay) )
        {
            if (hitblock.collider.CompareTag("Ambras"))
            {
                _overBlock = true;
                Debug.Log("Avoid");
            }
            else _overBlock = false;
        }
        return _overBlock;
    }
}
