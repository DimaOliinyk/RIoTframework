using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkUI.Utilities.Exceptions;

/// <summary>
/// Exception to be thrown by Content 
/// pages when their count is more that 1
/// </summary>
public class SinglePageException : InvalidNavigationException
{
    public SinglePageException()
    {
    }

    public SinglePageException(string message) 
        : base(message)
    {
    }

    public SinglePageException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
