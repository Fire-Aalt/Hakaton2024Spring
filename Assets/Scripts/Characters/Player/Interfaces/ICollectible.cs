namespace Game
{
    public interface ICollectible
    {
        public abstract CollectibleType Type { get; }

        public void Collect();
    }

    public enum CollectibleType
    {
        Coin
    }
}