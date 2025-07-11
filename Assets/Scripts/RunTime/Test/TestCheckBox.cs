using System;
using UnityEngine;

namespace Test
{
    using System.Collections;
    using UI;

    public sealed class TestCheckBox : MonoBehaviour
    {
        public SelectableCheckBox checkBox;
        public SelectableCheckBox checkBox2;
        public bool isCheck = true;
        public bool isCheck2 = true;

        private void Start()
        {
            StartCoroutine(setUpCheckBox());
        }

        private IEnumerator setUpCheckBox()
        {
            yield return new WaitForEndOfFrame();
            checkBox.setup(setUpFunction);
            checkBox2.setup(setUpFunction2);
        }

        private void setUpFunction(SelectableBase thingToBeSetUp)
        {
            if (thingToBeSetUp is SelectableCheckBox)
            {
                SelectableCheckBox box = thingToBeSetUp as SelectableCheckBox;
                box.isChecked = isCheck;
            }
        }


        private void setUpFunction2(SelectableBase thingToBeSetUp)
        {
            if (thingToBeSetUp is SelectableCheckBox)
            {
                SelectableCheckBox box = thingToBeSetUp as SelectableCheckBox;
                box.isChecked = isCheck2;
            }
        }

    }

}
