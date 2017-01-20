using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Utils.Validate
{
    public class ValidateHelper
    {
        public static bool HasValue(string value) {
            if (string.IsNullOrEmpty(value)) {
                return false;
            }
            if (string.IsNullOrWhiteSpace(value)) {
                return false;
            }
            return true;
        }
    }
}
