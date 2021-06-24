public interface UnlockableReward
{
    bool Unlocked { get; }

    void Unlock();
}
