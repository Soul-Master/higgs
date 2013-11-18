using System;
using System.ComponentModel;

namespace Higgs.Core.Interfaces
{
    /// <summary>
    ///  Hide all normal public method of system.object class from editor.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IObjectUnbrowsable
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();

        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object obj);
    }
}


