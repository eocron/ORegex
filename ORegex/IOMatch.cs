namespace Eocron
{
    public interface IOMatch<TValue> : IOCapture<TValue>
    {
        IOCaptureTable<TValue> Captures { get; }
    }
}