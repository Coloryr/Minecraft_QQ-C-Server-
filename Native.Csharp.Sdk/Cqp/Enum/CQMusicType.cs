using System.ComponentModel;

namespace Native.Csharp.Sdk.Cqp.Enum
{
    /// <summary>
    /// 指示点歌时使用的音乐来源
    /// </summary>
    [DefaultValue(CQMusicType.Tencent)]
    public enum CQMusicType
    {
        /// <summary>
        /// QQ 音乐
        /// </summary>
        [Description("qq")]
        Tencent,
        /// <summary>
        /// 网易云音乐
        /// </summary>
        [Description("163")]
        Netease,
        /// <summary>
        /// 虾米音乐
        /// </summary>
        [Description("xiami")]
        XiaMi
    }
}
