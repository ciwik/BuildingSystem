using UnityEngine;
using UnityEngine.UI;

public class UiButtons : MonoBehaviour {
    [SerializeField]
    private Button _buildButton;
    [SerializeField]
    private Button _buildCancelledButton;

    public Button BuildButton => _buildButton;
    public Button BuildCancelledButton => _buildCancelledButton;
}
