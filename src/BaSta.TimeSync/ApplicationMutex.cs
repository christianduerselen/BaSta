using System.Collections.Generic;
using System.Threading;

namespace BaSta.TimeSync
{
    /// <summary>
    /// Static class providing global mutex-locking for applications based on their IDs. This functionality has to be used for initialization-routines.
    /// </summary>
    public static class ApplicationMutex
    {
        private const string MutexIDFormatString = @"Global\{0}";

        private static readonly IDictionary<string, Mutex> _mutexDictionary = new Dictionary<string, Mutex>(); 
        
        /// <summary>
        /// Gets the information whether the application controller with the given ID is currently in use.
        /// </summary>
        /// <param name="uniqueID">The ID of the application mutex.</param>
        /// <returns><c>true</c> if the application controller is currently in use, otherwise <c>false</c>.</returns>
        public static bool GetIsInUse(string uniqueID)
        {
            Mutex mutex = new Mutex(true, string.Format(MutexIDFormatString, uniqueID), out bool createdNew);
            mutex.Close();

            return !createdNew;
        }

        /// <summary>
        /// Opens the mutex for the application controller with the given ID.
        /// </summary>
        /// <param name="uniqueID">The ID of the application mutex.</param>
        /// <returns><c>true</c> if the mutex has been newly created, otherwise <c>false</c>.</returns>
        public static bool Open(string uniqueID)
        {
            string mutexName = string.Format(MutexIDFormatString, uniqueID);
            lock (_mutexDictionary)
            {
                _mutexDictionary.Add(mutexName, new Mutex(true, string.Format(MutexIDFormatString, uniqueID), out bool createdNew));

                return createdNew;
            }
        }

        /// <summary>
        /// Closes the mutex.
        /// </summary>
        public static void Close(string uniqueID)
        {
            string mutexName = string.Format(MutexIDFormatString, uniqueID);

            lock (_mutexDictionary)
            {
                if (!_mutexDictionary.TryGetValue(mutexName, out Mutex mutex))
                    return;

                _mutexDictionary.Remove(mutexName);
                mutex.Close();
            }
        }
    }
}