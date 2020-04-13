﻿using System;

namespace Native.Tool.IniConfig.Attribute
{
    /// <summary>
    /// 表示 IniConfig 在序列化时将表示为键的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class IniConfigKeyAttribute : System.Attribute
    {
        /// <summary>
        /// 表示 IniConfig 文件中存储的键名称
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// 初始化 <see cref="IniConfigKeyAttribute"/> 类的新实例
        /// </summary>
        public IniConfigKeyAttribute()
            : this(null)
        { }

        /// <summary>
        /// 初始化 <see cref="IniConfigKeyAttribute"/> 类的新实例
        /// </summary>
        /// <param name="keyName">用于表示键值对 "键" 的名称</param>
        public IniConfigKeyAttribute(string keyName)
        {
            this.KeyName = keyName;
        }
    }
}