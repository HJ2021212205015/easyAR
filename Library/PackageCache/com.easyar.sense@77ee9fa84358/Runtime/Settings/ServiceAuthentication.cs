//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en">EasyAR cloud service authentication data.</para>
    /// <para xml:lang="zh">EasyAR云服务鉴权数据。</para>
    /// </summary>
    [Serializable]
    public abstract class ServiceAccessData
    {
        /// <summary>
        /// <para xml:lang="en">Service AppID.</para>
        /// <para xml:lang="zh">服务AppID。</para>
        /// </summary>
        public string AppID = string.Empty;

        internal abstract ServiceAccessData TrimClone();
    }

    /// <summary>
    /// <para xml:lang="en">EasyAR cloud service authentication data which require access address.</para>
    /// <para xml:lang="zh">需要填写访问地址的EasyAR云服务鉴权数据。</para>
    /// </summary>
    [Serializable]
    public abstract class ExplicitAddressAccessData : ServiceAccessData
    {
        /// <summary>
        /// <para xml:lang="en">Server Address.</para>
        /// <para xml:lang="zh">服务地址。</para>
        /// </summary>
        public string ServerAddress = string.Empty;
    }

    /// <summary>
    /// <para xml:lang="en">EasyAR cloud service authentication data using API Key. You can get the data from EasyAR Develop Center (https://www.easyar.com).</para>
    /// <para xml:lang="zh">使用API Key的EasyAR云服务鉴权数据。有关数据请访问EasyAR开发中心（https://www.easyar.cn）获取。</para>
    /// </summary>
    [Serializable]
    public sealed class APIKeyAccessData : ExplicitAddressAccessData
    {
        /// <summary>
        /// <para xml:lang="en">API Key.</para>
        /// <para xml:lang="zh">API Key。</para>
        /// </summary>
        public string APIKey = string.Empty;
        /// <summary>
        /// <para xml:lang="en">API Secret.</para>
        /// <para xml:lang="zh">API Secret。</para>
        /// </summary>
        public string APISecret = string.Empty;

        internal override ServiceAccessData TrimClone() => new APIKeyAccessData
        {
            AppID = AppID.Trim(),
            ServerAddress = ServerAddress.Trim(),
            APIKey = APIKey.Trim(),
            APISecret = APISecret.Trim(),
        };
    }

    /// <summary>
    /// <para xml:lang="en">EasyAR cloud service authentication data using token. You can get the data from EasyAR Develop Center (https://www.easyar.com).</para>
    /// <para xml:lang="zh">使用token的EasyAR云服务鉴权数据。有关数据请访问EasyAR开发中心（https://www.easyar.cn）获取。</para>
    /// </summary>
    [Serializable]
    public sealed class TokenAccessData : ExplicitAddressAccessData
    {
        /// <summary>
        /// <para xml:lang="en">Token.</para>
        /// <para xml:lang="zh">Token。</para>
        /// </summary>
        public string Token = string.Empty;

        internal override ServiceAccessData TrimClone() => new TokenAccessData
        {
            AppID = AppID.Trim(),
            ServerAddress = ServerAddress.Trim(),
            Token = Token.Trim(),
        };
    }

    /// <summary>
    /// <para xml:lang="en">EasyAR cloud service authentication data using API Key. You can get the data from EasyAR Develop Center (https://www.easyar.com).</para>
    /// <para xml:lang="zh">使用API Key的EasyAR云服务鉴权数据。有关数据请访问EasyAR开发中心（https://www.easyar.cn）获取。</para>
    /// </summary>
    [Serializable]
    public sealed class FixedAddressAPIKeyAccessData : ServiceAccessData
    {
        /// <summary>
        /// <para xml:lang="en">API Key.</para>
        /// <para xml:lang="zh">API Key。</para>
        /// </summary>
        public string APIKey = string.Empty;
        /// <summary>
        /// <para xml:lang="en">API Secret.</para>
        /// <para xml:lang="zh">API Secret。</para>
        /// </summary>
        public string APISecret = string.Empty;

        internal override ServiceAccessData TrimClone() => new FixedAddressAPIKeyAccessData
        {
            AppID = AppID.Trim(),
            APIKey = APIKey.Trim(),
            APISecret = APISecret.Trim(),
        };
    }

    /// <summary>
    /// <para xml:lang="en">EasyAR cloud service authentication data using App Secret. You can get the data from EasyAR Develop Center (https://www.easyar.com).</para>
    /// <para xml:lang="zh">使用App Secret的EasyAR云服务鉴权数据。有关数据请访问EasyAR开发中心（https://www.easyar.cn）获取。</para>
    /// </summary>
    [Serializable]
    public sealed class AppSecretAccessData : ExplicitAddressAccessData
    {
        /// <summary>
        /// <para xml:lang="en">App Secret.</para>
        /// <para xml:lang="zh">App Secret。</para>
        /// </summary>
        public string AppSecret = string.Empty;

        internal override ServiceAccessData TrimClone() => new AppSecretAccessData
        {
            AppID = AppID.Trim(),
            ServerAddress = ServerAddress.Trim(),
            AppSecret = AppSecret.Trim(),
        };
    }
}
