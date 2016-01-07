﻿using System;
using System.Threading;
#if SUPPORTS_ASYNC
using System.Threading.Tasks;
#endif

namespace Polly.Utilities
{
    /// <summary>
    /// Time related delegates used to improve testability of the code
    /// </summary>
    public static class SystemClock
    {
#if !PORTABLE
        /// <summary>
        /// Allows the setting of a custom Thread.Sleep implementation for testing.
        /// By default this will be a call to <see cref="M:Thread.Sleep"/>
        /// </summary>
        public static Action<TimeSpan> Sleep = Thread.Sleep;
#endif
#if PORTABLE
        /// <summary>
        /// Allows the setting of a custom Thread.Sleep implementation for testing.
        /// By default this will be a call to <see cref="M:ManualResetEvent.WaitOne"/>
        /// </summary>
        public static Action<TimeSpan> Sleep = timespan => new ManualResetEvent(false).WaitOne(timespan);
#endif
#if SUPPORTS_ASYNC
        /// <summary>
        /// Allows the setting of a custom async Sleep implementation for testing.
        /// By default this will be a call to <see cref="M:Task.Delay"/>
        /// </summary>
        public static Func<TimeSpan, Task> SleepAsync = Task.Delay;
#endif
        /// <summary>
        /// Allows the setting of a custom DateTime.UtcNow implementation for testing.
        /// By default this will be a call to <see cref="DateTime.UtcNow"/>
        /// </summary>
        public static Func<DateTime> UtcNow = () => DateTime.UtcNow;

        /// <summary>
        /// Resets the custom implementations to their defaults. 
        /// Should be called during test teardowns.
        /// </summary>
        public static void Reset()
        {
#if !PORTABLE
            Sleep = Thread.Sleep;
#else
            Sleep = timeSpan => new ManualResetEvent(false).WaitOne(timeSpan);
#endif
#if SUPPORTS_ASYNC
            SleepAsync = Task.Delay;
#endif
            UtcNow = () => DateTime.UtcNow;
        }
    }
}