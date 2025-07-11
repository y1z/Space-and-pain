using System;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{

    [Serializable]
    public struct StateSequence
    {
        public GameStates stateToBeSet;
        public UnityEvent eventsToBeCalled;
    }

    /// <summary>
    /// Controls which sequence of states will be used when a scene is loaded
    /// </summary>
    public sealed class StateSequencer : MonoBehaviour
    {
        [Tooltip("Controls if the StateSequencer does anything")]
        public bool is_ON = true;

        [Tooltip("The sequence in which the states will be called")]
        public StateSequence[] sequence;


        private void Start()
        {
            if (!is_ON) { return; }
            if (sequence == null) { return; }

            for (int i = 0; i < sequence.Length; i++)
            {
                SingletonManager.inst.gameManager.setState(sequence[i].stateToBeSet);
            }

        }

    }


}
