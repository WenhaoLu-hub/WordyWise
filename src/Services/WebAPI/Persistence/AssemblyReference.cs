using System.Reflection;

namespace Persistence;

public class AssemblyReference
{
    public static Assembly Assembly() => typeof(AssemblyReference).Assembly;
}