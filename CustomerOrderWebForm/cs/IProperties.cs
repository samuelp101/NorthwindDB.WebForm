using System.Reflection;

namespace CustomerOrderWebForm
{
    public interface IProperties
    {
        PropertyInfo[] Properties { get; }
    }
}