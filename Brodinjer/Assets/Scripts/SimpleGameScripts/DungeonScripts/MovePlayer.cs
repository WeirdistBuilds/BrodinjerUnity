using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MovePlayer : MonoBehaviour
{
    public Transform ObjToMove;
    private CharacterController _cc;

    public float FadeOutTime, BlackoutTime, FadeInTime;

    public Image BlackSprite;

    private float currentTime;

    public UnityEvent EndMoveEvent, FadedEvent;

    private void Awake()
    {
        _cc = ObjToMove.GetComponent<CharacterController>();
        BlackSprite.color = Color.clear;
    }

    public void Move(Transform PlaceToMoveTo)
    {
        StartCoroutine(MoveCoroutine(PlaceToMoveTo));
    }

    private IEnumerator MoveCoroutine(Transform PlaceToMoveTo)
    {
        if (_cc != null)
            _cc.enabled = false;
        currentTime = 0;
        while (currentTime < FadeOutTime)
        {
            currentTime += Time.deltaTime;
            BlackSprite.color = Color.Lerp(Color.clear, Color.black,
                GeneralFunctions.ConvertRange(0, FadeOutTime, 0, 1, currentTime));
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForFixedUpdate();
        ObjToMove.transform.position = PlaceToMoveTo.transform.position;
        //ObjToMove.rotation = PlaceToMoveTo.rotation;
        FadedEvent.Invoke();
        if (_cc != null)
            _cc.enabled = true;
        yield return new WaitForSeconds(BlackoutTime);
        currentTime = 0;
        while (currentTime < FadeInTime)
        {
            currentTime += Time.deltaTime;
            BlackSprite.color = Color.Lerp(Color.black, Color.clear,
                GeneralFunctions.ConvertRange(0, FadeInTime, 0, 1, currentTime));
            yield return new WaitForFixedUpdate();
        }        
       
        EndMoveEvent.Invoke();
    }
    
}
