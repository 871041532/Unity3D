// -----------------------------------------------------------------
// File:    AssetInfo.cs
// Author:  mouguangyi
// Date:    2016.07.12
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;
using System.Collections.Generic;
using System.IO;
//using YamlDotNet.Serialization;
//using YamlDotNet.Serialization.NamingConventions;

namespace GameBox.Service.AssetManager
{
    /// <summary>
    /// @details �ʲ���Ϣ��
    /// </summary>
    public sealed class AssetInfo
    {
        /// <summary>
        /// �ʲ���·����
        /// </summary>
        public string PackPath { get; set; }

        internal AssetInfo _Write(StringStreamWriter writer)
        {
            writer.WriteIndexString(PackPath, "/");
            return this;
        }

        internal AssetInfo _Read(StringStreamReader reader)
        {
            PackPath = reader.ReadIndexString("/");
            return this;
        }
    }

    /// <summary>
    /// @details �ʲ�����Ϣ��
    /// </summary>
    public sealed class AssetPackInfo
    {
        /// <summary>
        /// �ʲ������͡�
        /// </summary>
        public AssetPackType Type { get; set; }

        /// <summary>
        /// �����������ʲ�����
        /// </summary>
        public string[] Dependencies { get; set; }

        /// <summary>
        /// У���롣
        /// </summary>
        public string CheckCode { get; set; }

        /// <summary>
        /// �ʲ����ļ���С��
        /// </summary>
        public long Size { get; set; }

        internal AssetPackInfo _Write(StringStreamWriter writer)
        {
            writer.WriteNumber((int)Type);

            var length = (null != Dependencies ? Dependencies.Length : 0);
            writer.WriteNumber(length);
            for (var i = 0; i < length; ++i) {
                writer.WriteIndexString(Dependencies[i], "/");
            }

            writer.WriteString(CheckCode);

            writer.WriteNumber(Size);

            return this;
        }

        internal AssetPackInfo _Read(StringStreamReader reader)
        {
            Type = (AssetPackType)reader.ReadNumber();

            var length = reader.ReadNumber();
            if (length > 0) {
                Dependencies = new string[length];
                for (var i = 0; i < length; ++i) {
                    Dependencies[i] = reader.ReadIndexString("/");
                }
            }

            CheckCode = reader.ReadString();

            Size = reader.ReadNumber();

            return this;
        }
    }

    /// <summary>
    /// @details ��Դ��������ʱ��Ϣ������YAML������Ƹ�ʽ��
    /// </summary>
    public sealed class AssetManagerInfo
    {
        /// <summary>
        /// ���캯����
        /// </summary>
        public AssetManagerInfo()
        {
            Version = "0.0.0.0";
            Date = DateTime.UtcNow;
            Packs = new Dictionary<string, AssetPackInfo>();
            Assets = new Dictionary<string, AssetInfo>();
            _LoadType = AssetPackLoadType.RESOURCES;
        }

        /// <summary>
        /// �汾�š�
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// �������ڡ�
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// �ʲ���ӳ���
        /// </summary>
        public Dictionary<string, AssetPackInfo> Packs { get; set; }

        /// <summary>
        /// �ʲ�ӳ���
        /// </summary>
        public Dictionary<string, AssetInfo> Assets { get; set; }

        /// <summary>
        /// �ʲ���װ�ط�ʽ��
        /// </summary>
        //[YamlIgnore]
        internal AssetPackLoadType _LoadType;

        private AssetManagerInfo _Write(StringStreamWriter writer)
        {
            writer.WriteString(Version);

            writer.WriteString(Date.ToString());

            writer.WriteNumber(Packs.Count);
            foreach (var pair in Packs) {
                writer.WriteIndexString(pair.Key, "/");
                pair.Value._Write(writer);
            }

            writer.WriteNumber(Assets.Count);
            foreach (var pair in Assets) {
                writer.WriteIndexString(pair.Key, "/");
                pair.Value._Write(writer);
            }

            return this;
        }

        private AssetManagerInfo _Read(StringStreamReader reader)
        {
            Version = reader.ReadString();

            Date = DateTime.Parse(reader.ReadString());

            var count = reader.ReadNumber();
            for (var i = 0; i < count; ++i) {
                var key = reader.ReadIndexString("/");
                var value = new AssetPackInfo()._Read(reader);
                Packs.Add(key, value);
            }

            count = reader.ReadNumber();
            for (var i = 0; i < count; ++i) {
                var key = reader.ReadIndexString("/");
                var value = new AssetInfo()._Read(reader);
                Assets.Add(key, value);
            }

            return this;
        }

        /// <summary>
        /// ���л�AssetManagerInfo��
        /// </summary>
        /// <param name="stream">д������</param>
        /// <param name="managerInfo">Ҫ���л���AssetManagerInfo����</param>
        /// <param name="binarize">�Ƿ�����ƻ�</param>
        public static void Serialize(Stream stream, AssetManagerInfo managerInfo, bool binarize)
        {
            try {
                stream.WriteByte(Convert.ToByte(binarize));
                if (binarize) {
                    using (var writer = new StringStreamWriter(stream)) {
                        managerInfo._Write(writer);
                    }
                } else {
                    using (var writer = new StreamWriter(stream)) {
                        //var serializer = new Serializer(namingConvention: new CamelCaseNamingConvention());
                       // serializer.Serialize(writer, managerInfo);
                    }
                }
            } catch (Exception e) {
                Logger<IAssetManager>.X(e);
            }
        }

        /// <summary>
        /// ���ı����ݷ����л���AssetManagerInfoʵ����
        /// </summary>
        /// <param name="stream">��ȡ����</param>
        /// <returns>����AssetManagerInfo����</returns>
        public static AssetManagerInfo Deserialize(Stream stream)
        {
            try {
                var binarize = Convert.ToBoolean(stream.ReadByte());
                if (binarize) {
                    using (var reader = new StringStreamReader(stream)) {
                        AssetManagerInfo info = new AssetManagerInfo();
                        info._Read(reader);
                        return info;
                    }
                } else {
                    using (var reader = new StreamReader(stream)) {
                        // var deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());
                        //return deserializer.Deserialize<AssetManagerInfo>(reader);
                        return null;
                    }
                }
            } catch (Exception e) {
                Logger<IAssetManager>.X(e);
                return null;
            }
        }
    }
}