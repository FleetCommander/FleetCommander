using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Text))]
public class TextSetter : MonoBehaviour {
    [SerializeField] private string _prefix;

    public void ChangeText(int text) {
        GetComponent<Text>().text = _prefix + text;
    }
}
