using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class HandButton : XRBaseInteractable
{
    public UnityEvent OnPress = null;

    private float yMin = 0.0f;
    private float yMax = 0.0f;
    private bool previousPress = false;

    private float previousHandHeight = 0.0f;
    private XRBaseInteractor hoverInteractor = null;

    protected override void Awake()
    {
        base.Awake();
        onHoverEnter.AddListener(StartPress);
        onHoverExit.AddListener(EndPress);
    }

    private void OnDestroy()
    {
        onHoverEnter.RemoveListener(StartPress);
        onHoverExit.RemoveListener(EndPress);
    }

    private void StartPress(XRBaseInteractor interactor)
    {
        hoverInteractor = interactor;
        previousHandHeight = GetLocalYPos(hoverInteractor.transform.position);
    }

    private void EndPress(XRBaseInteractor interactor)
    {
        hoverInteractor = null;
        previousHandHeight = 0.0f;

        previousPress = false;
        SetYPos(yMax);
    }

    private void Start()
    {
        SetMinMax();
    }

    private void SetMinMax()
    {
        Collider collider = GetComponent<Collider>();
        yMin = transform.localPosition.y - (collider.bounds.size.y * 0.5f);
        yMax = transform.localPosition.y;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (hoverInteractor)
        {
            float newHandHeight = GetLocalYPos(hoverInteractor.transform.position);
            float handDiff = previousHandHeight - newHandHeight;
            previousHandHeight = newHandHeight;

            float newPos = transform.localPosition.y - handDiff;
            SetYPos(newPos);

            CheckPress();
        }
    }

    private float GetLocalYPos(Vector3 position)
    {
        Vector3 localPos = transform.root.InverseTransformPoint(position);
        return localPos.y;
    }

    private void SetYPos(float position)
    {
        Vector3 newPos = transform.localPosition;
        newPos.y = Mathf.Clamp(position, yMin, yMax);
        transform.localPosition = newPos;
    }

    private void CheckPress()
    {
        bool inPosition = InPosition();

        if (inPosition && inPosition != previousPress)
        {
            OnPress.Invoke();
        }
        previousPress = inPosition;
    }

    private bool InPosition()
    {
        float inRange = Mathf.Clamp(transform.localPosition.y, yMin, yMin + 0.01f);
        return transform.localPosition.y == inRange;
    }
}
