namespace ConstruyendoMiComunidad
{
    public interface IInteractable
    {
        string Prompt { get; }
        bool CanInteract { get; }
        void Interact(PlayerController player);
    }
}
