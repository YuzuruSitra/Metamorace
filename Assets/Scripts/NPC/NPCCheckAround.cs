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
     [SerializeField] 
    private float _npcReach = 1.0f;

    float _verticalRayOffset = 2.0f;
    float _horizontalRayOffset = 1.0f;
    float _vericalAvoidRayLengh = 6.0f;
     public bool _hitBlock = false;
     public bool _behindBlock = false;
    void Update()
    {

    }
    //目の前のブロックを破壊する
    public bool CheckBreakeBlock()
    {
        Vector3 direction = transform.forward;
        direction.Normalize();
        Ray ray = new Ray(transform.position + _upPadding, direction);

        Debug.DrawRay(transform.position + _upPadding, direction, Color.green, 0.3f);
        if(Physics.Raycast(ray, out RaycastHit hit, _npcReach))
        {
             if(hit.collider.CompareTag("Ambras")||hit.collider.CompareTag("Heros")) 
             {
                _hitBlock = true;
             }
        }
        else 
        {
            _hitBlock = false;
        }
        return _hitBlock;
    }
    //背後のブロックを確認する
    public bool CheckBehindBlock()
    {
         Vector3 direction = -transform.forward;
        direction.Normalize();
        Ray ray = new Ray(transform.position + _upPadding, direction);

        Debug.DrawRay(transform.position + _upPadding, direction, Color.green, 0.3f);
        if(Physics.Raycast(ray, out RaycastHit hit, _npcReach))
        {
             if(hit.collider.CompareTag("Ambras")||hit.collider.CompareTag("Heros")) 
             {
                _behindBlock = true;
             }
        }
        else 
        {
            _behindBlock = false;
        }
        return _behindBlock;
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
            if (righthit.collider.CompareTag("Ambras")||righthit.collider.CompareTag("Heros"))
            {
                //右のブロックまでの距離
                RightdistanceToHitObject = righthit.distance;
            }
        }
        if (Physics.Raycast(Leftray, out RaycastHit lefthit, MaxBlockRayDist))
        {
            if (lefthit.collider.CompareTag("Ambras")||lefthit.collider.CompareTag("Heros"))
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

    //壁を検知したら反転する用
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
        Debug.DrawRay(rayover.origin, rayover.direction * _vericalAvoidRayLengh, Color.green);

        if (Physics.Raycast(rayover, out RaycastHit hitblock, _vericalAvoidRayLengh) )
        {
            if (hitblock.collider.CompareTag("Ambras")||hitblock.collider.CompareTag("Heros"))
            {
                _overBlock = true;
            }
            else _overBlock = false;
        }
        return _overBlock;
    }

    //奥行きの圧死を回避するレイ
    public bool CheckHorizontalDeathBlock()
    {
        bool _depthBlock = false;
       
        Vector3 rayOrigin = transform.position + Vector3.up * _horizontalRayOffset;
        Ray raydepth = new Ray(rayOrigin, Vector3.forward);
        Debug.DrawRay(raydepth.origin, raydepth.direction * _vericalAvoidRayLengh, Color.yellow);
    
        if (Physics.Raycast(raydepth, out RaycastHit hitblock, _vericalAvoidRayLengh) )
        {
            if (hitblock.collider.CompareTag("Ambras")||hitblock.collider.CompareTag("Heros"))
            {
                _depthBlock = true;
            }
            else _depthBlock = false;
        }
        return _depthBlock;
    }

    // //敵プレイヤーを検出する
    public bool CheckPlayer()
    {
        bool _checkPlayer = false;
       
        Vector3 rayOrigin = transform.position + Vector3.up * _horizontalRayOffset;
        Ray raydepth = new Ray(rayOrigin, Vector3.forward);
        Debug.DrawRay(raydepth.origin, raydepth.direction * _vericalAvoidRayLengh, Color.red);
   
        if (Physics.Raycast(raydepth, out RaycastHit hitplayer, _vericalAvoidRayLengh) )
        {
            if (hitplayer.collider.CompareTag("Player"))
            {
                _checkPlayer = true;
            }
            else _checkPlayer = false;
        }
        return _checkPlayer;
    }
}
