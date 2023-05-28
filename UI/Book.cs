using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Book : MonoBehaviour
{
    [TextArea(10, 20)]
    [SerializeField] private string content;
    [SerializeField] TMP_Text leftSide;
    [SerializeField] TMP_Text rightSide;

    [SerializeField] TMP_Text leftPagination;
    [SerializeField] TMP_Text rightPagination;

    private void OnValidate()
    {
        UpdatePagination();

        if (leftSide.text == content)
            return;

        SetupContent();
    }

    private void Awake()
    {
        SetupContent();
        UpdatePagination();
    }

    public void AddContent(string _part)
    {
        content += _part;

        if (leftSide.text == content)
            return;

        SetupContent();
        UpdatePagination();
    }

    private void SetupContent()
    {
        leftSide.text = content;
        rightSide.text = content;
    }

    private void UpdatePagination()
    {
        leftPagination.text = leftSide.pageToDisplay.ToString();
        rightPagination.text = rightSide.pageToDisplay.ToString();
    }

    public void PreviousPage()
    {
        if(leftSide.pageToDisplay < 1)
        {
            leftSide.pageToDisplay = 1;
            return;
        }

        if (leftSide.pageToDisplay - 2 > 1)
            leftSide.pageToDisplay -= 2;
        else
            leftSide.pageToDisplay = 1;

        rightSide.pageToDisplay = leftSide.pageToDisplay + 1;

        UpdatePagination();
    }

    public void NextPage()
    {
        if (rightSide.pageToDisplay >= rightSide.textInfo.pageCount)
            return;

        if(leftSide.pageToDisplay >= leftSide.textInfo.pageCount - 1)
        {
            leftSide.pageToDisplay = leftSide.textInfo.pageCount - 1;
            rightSide.pageToDisplay = leftSide.pageToDisplay + 1;
        }
        else
        {
            leftSide.pageToDisplay += 2;
            rightSide.pageToDisplay = leftSide.pageToDisplay + 1;
        }

        UpdatePagination();
    }
}
