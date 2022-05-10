using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class Comparer<T> : IEqualityComparer<T> where T : class
    {
        private PropertyInfo _PropertyInfo;

        public Comparer(string propertyName)
        {
            //store a reference to the property info object for use during the comparison
            _PropertyInfo = typeof(T).GetProperty(propertyName);
        }
        public bool Equals(T x, T y)
        {
            //get the current value of the comparison property of x and of y
            object xValue = _PropertyInfo.GetValue(x, null);
            object yValue = _PropertyInfo.GetValue(y, null);

            //if the xValue is null then we consider them equal if and only if yValue is null
            if (xValue == null)
                return yValue == null;

            //use the default comparer for whatever type the comparison property is.
            return xValue.Equals(yValue);
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            //If obj is null then return 0
            if (obj == null)
            {
                return 0;
            }
            //Get the ID hash code value
            int HashCode = obj.GetHashCode();
            //Get the string HashCode Value
            //Check for null refernece exception
            int OHashCode = obj == null ? 0 : obj.GetHashCode();
            return OHashCode ^ HashCode;
        }
    }
}
