using System.Collections.Generic;
using System.Linq;

namespace HrtzSysInfo.Extensions
{
    public static class BooleanExtensions
    {
        public static bool ExceedsThreshold(int threshold, IEnumerable<bool> bools)
        {
            return bools.Count(b => b) > threshold;
        }
    }
}
