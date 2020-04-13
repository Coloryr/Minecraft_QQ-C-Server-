using System;

namespace Native.Tool.IniConfig.Attribute
{
    /// <summary>
    /// 表示对象不参与 IniConfig 的序列化的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IniNonSerializeAttribute : System.Attribute
    {

    }
}
