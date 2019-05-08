namespace NeedfulThings.PerformanceCounters
{
    using System;
    using System.Diagnostics;

    internal sealed class PerformanceCounterWrapper : IPerformanceCounter
    {
        private readonly IPerformanceCounter _nullPerformanceCounter;
        private IPerformanceCounter _innerCounter;

        public PerformanceCounterWrapper(IPerformanceCounter innerCounter)
        {
            _nullPerformanceCounter = new NullPerformanceCounter(innerCounter.CounterName, innerCounter.CounterType);
            _innerCounter = innerCounter;
        }

        public string CounterName => _innerCounter.CounterName;

        public PerformanceCounterType CounterType => _innerCounter.CounterType;

        public float NextValue()
        {
            try
            {
                return _innerCounter.NextValue();
            }
            catch
            {
                ChangeCounter(_nullPerformanceCounter);

                return _innerCounter.NextValue();
            }
        }

        public void Increment()
        {
            try
            {
                _innerCounter.Increment();
            }
            catch
            {
                ChangeCounter(_nullPerformanceCounter);
            }
        }

        public void IncrementBy(long value)
        {
            try
            {
                _innerCounter.IncrementBy(value);
            }
            catch
            {
                ChangeCounter(_nullPerformanceCounter);
            }
        }

        public void Decrement()
        {
            try
            {
                _innerCounter.Decrement();
            }
            catch
            {
                ChangeCounter(_nullPerformanceCounter);
            }
        }

        public void Reset()
        {
            try
            {
                _innerCounter.Reset();
            }
            catch
            {
                ChangeCounter(_nullPerformanceCounter);
            }
        }

        public void ChangeCounter(IPerformanceCounter performanceCounter)
        {
            _innerCounter = performanceCounter ?? throw new ArgumentNullException(nameof(performanceCounter));
        }

        public void Dispose()
        {
            _innerCounter.Dispose();
        }
    }
}