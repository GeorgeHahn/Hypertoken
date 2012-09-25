using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sniffer
{
    public interface DataPane
    {
        void AddData(long[] timeStamp, byte[] addr, byte[][] data);
    }
}
