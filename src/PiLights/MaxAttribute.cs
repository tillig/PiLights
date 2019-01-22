using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PiLights
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxAttribute : Attribute
    {
        public MaxAttribute(int max)
        {
            this.Maximum = max;
        }

        public int Maximum { get; }
    }
}
