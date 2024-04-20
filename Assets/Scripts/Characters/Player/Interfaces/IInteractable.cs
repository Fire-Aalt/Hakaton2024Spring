namespace Game
{
    public interface IInteractable
    {
        public bool HoldInteract { get; }
        public float HoldDuration { get; }
        public bool CanInteract { get; }
        public string CannotInteractText { get; }

        void StartHold();
        void FailHold();
        void Interact();
    }
}