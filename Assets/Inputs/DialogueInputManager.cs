using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputManager", menuName = "Data/DialogueInputManager")]
public class DialogueInputManager : ScriptableObject, PlayerInput_Actions.IDialogueActions
{
    private PlayerInput_Actions _dialogueInput;
    
    public event UnityAction InteractEvent;
    private void OnEnable()
    {
        if (_dialogueInput == null)
        {
            _dialogueInput = new PlayerInput_Actions();
            _dialogueInput.Dialogue.Enable();
            _dialogueInput.Dialogue.SetCallbacks(this);
        }
    }

    private void OnDisable()
    {
        _dialogueInput.Dialogue.Disable();
    }

    public void EnableInput(bool enable)
    {
        if (enable)
            _dialogueInput.Dialogue.Enable();
        else 
            _dialogueInput.Dialogue.Disable();
    }

    public void OnNextDialogue(InputAction.CallbackContext context)
    {
        if (context.phase is InputActionPhase.Started)
            InteractEvent?.Invoke();
    }
}
