using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Extensions
{
    public static class ManagementBaseObjectExtentions
    {
        public static string TryGetProperty(this System.Management.ManagementBaseObject wmiObj, string propertyName)
        {
            string retval;
            try
            {
                retval = wmiObj.GetPropertyValue(propertyName).ToString();
            }
            catch (System.Management.ManagementException)
            {
                retval = string.Empty;
            }
            return retval;
        }
    }
}
