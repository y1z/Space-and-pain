using IndividualLevelLogic;

using System.Collections.Generic;
using UnityEngine;
using TMPro;


public sealed class TutorialTextBox : MonoBehaviour
{
    public TextMeshProUGUI selfText;
    public UnityEngine.UI.Image background;
    public UnityEngine.UI.Image[] extraComponents;
    public TutorialLevelLogic.TutorialStates tutorialState;
    public int order = 0;

}


internal sealed class TutorialTextBoxCompare : Comparer<TutorialTextBox>
{
    public static readonly TutorialTextBoxCompare compareObj = new TutorialTextBoxCompare();

    public override int Compare(TutorialTextBox left, TutorialTextBox right)
    {
        int stateCompare = left.tutorialState.CompareTo(right.tutorialState);
        if (stateCompare != 0)
        {
            return stateCompare;
        }

        return left.order.CompareTo(right.order);
    }
}
