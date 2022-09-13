using System.Diagnostics;

namespace BlockchainDataStructures.Utils
{
    public interface IMeasureTimePerformance
    {
        void Init();

        void Start();

        void Stop();

        string GetElapsedTime();

        DateTime? StartTime { get; }

        DateTime? StopTime { get; }

    }

    public class MeasureTimePerformance : IMeasureTimePerformance
    {
        private Stopwatch _stopWatch;
        private DateTime? _startTime;
        private DateTime? _stopTime;

        public DateTime? StartTime => _startTime;
        public DateTime? StopTime => _stopTime;

        public MeasureTimePerformance()
        {
        }

        public void Init()
        {
            _stopWatch = new Stopwatch();
        }

        public void Start()
        {
            if (_stopWatch == null)
                Init();

            _stopWatch?.Start();
            _startTime = DateTime.Now;
        }

        public void Stop()
        {
            if (_stopWatch == null)
                Init();

            _stopWatch?.Stop();
            _stopTime = DateTime.Now;
        }

        public string GetElapsedTime()
        {
            if (_stopWatch == null) return null;

            TimeSpan ts = _stopWatch.Elapsed;

            return $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}:{ts.Milliseconds:000}";
        }
    }
}