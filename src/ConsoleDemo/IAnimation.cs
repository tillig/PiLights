using System;
using System.Linq;
using System.Threading;

namespace ConsoleDemo
{
    internal interface IAnimation
    {
        void Execute(CancellationToken token);
    }
}
