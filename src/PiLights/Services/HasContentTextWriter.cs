using System.IO;
using System.Text;

namespace PiLights.Services
{
    internal class HasContentTextWriter : TextWriter
    {
        public override Encoding Encoding => Null.Encoding;

        public bool HasContent { get; private set; }

        public override void Write(char value)
        {
            this.HasContent = true;
        }

        public override void Write(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                this.HasContent = true;
            }
        }

        public override void Write(char[] buffer, int index, int count)
        {
            if (count > 0)
            {
                this.HasContent = true;
            }
        }
    }
}
