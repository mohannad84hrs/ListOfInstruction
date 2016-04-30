using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MistaksInHaramin.HelperClasses
{
    /// <summary>
    /// INetworkConncetion: check whether internet connection is available or not.
    /// Interface to be implemented for each platform
    /// </summary>
    public interface INetworkConnection
    {
        /// <summary>
        /// Return true if interent connection was availbe and false if wasn't.
        /// </summary>
        bool IsConnected { get; }
        
    }
}
