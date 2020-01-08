using System;
using UnityEngine;

public class IntVariableEventAdapter : MonoBehaviour {
    [SerializeField] private IntVariable _intVariable;
    [SerializeField] private UnityEventInt _onValueChange;

    private void OnEnable() {
        _intVariable.OnValueChange.AddListener(OnValueChangedHandler);
        _onValueChange.Invoke(_intVariable.Value);
    }

    private void OnDisable() {
        _intVariable.OnValueChange.RemoveListener(OnValueChangedHandler);
    }

    private void OnValueChangedHandler(int value) {
        _onValueChange.Invoke(value);
    }
}
