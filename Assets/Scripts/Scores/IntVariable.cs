using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (menuName = "Variables/Int", fileName = "IntVariable", order = 0)]

public class IntVariable : ScriptableObject {
    
    [SerializeField] private int _value;

    public UnityEventInt OnValueChange { get; } = new UnityEventInt();

    public int Value {
        get {
            return _value;
        }set {
            if (_value == value) return;
                 _value = value;
                OnValueChange.Invoke(value);
        }
    }

    public void Increment() {
        Value++;
    }

    public void Decrement() {
        if (Value > 0) {
            Value--;
        }
    }
    
    #if UNITIY_EDITOR
    private void OnValidate(){
    if(Application.isPlaying) OnValueChange.Invoke(_value);
    }
#endif
} 

