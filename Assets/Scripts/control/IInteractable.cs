

namespace  RPG.Control
{
    public interface IInteractable 
    {
        bool Interact(PlayerController controller);
        CursorType GetCursorType();
    }
}
