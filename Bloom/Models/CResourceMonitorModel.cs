using System;
using System.Diagnostics;
using System.Threading;

namespace Bloom.Models
{
    class CResourceMonitorModel : AModel
    {
        /// <summary>
        /// パフォーマンスカウンタ
        /// </summary>
        private PerformanceCounter cpuPerformanceCounter = null;
        private PerformanceCounter memoryPerformanceCounter = null;
        private PerformanceCounter diskPerformanceCounter = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CResourceMonitorModel()
        {
            cpuPerformanceCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            memoryPerformanceCounter = new PerformanceCounter("Memory", "Available MBytes", "_Total");
            diskPerformanceCounter = new PerformanceCounter("LogicalDisk", "% Free Space", "_Total");
        }

        /// <summary>
        /// CPU使用率を取得する
        /// </summary>
        /// <returns>
        /// CPU使用率.
        /// </returns>
        /// <remarks>
        /// 最初の数回は0が取得される.
        /// </remarks>
        public double GetUseRate_CPU()
        {
            return this.cpuPerformanceCounter.NextValue();
        }

        /// <summary>
        /// メモリ使用率を取得する
        /// </summary>
        /// <returns>
        /// メモリ使用率.
        /// </returns>
        /// <remarks>
        /// 最初の数回は0が取得される.
        /// </remarks>
        public double GetUseRate_Memory()
        {
            return this.memoryPerformanceCounter.NextValue();
        }

        /// <summary>
        /// ディスク使用率を取得する
        /// </summary>
        /// <returns>
        /// ディスク使用率.
        /// </returns>
        /// <remarks>
        /// 最初の数回は0が取得される.
        /// </remarks>
        public double GetUseRate_Disk()
        {
            return this.diskPerformanceCounter.NextValue();
        }
    }
}
