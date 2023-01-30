﻿using System.ComponentModel;
using System.Xml;
using CasaEngineCommon.Extension;

namespace CasaEngine.Editor.Assets
{
    [TypeConverter(typeof(AssetBuildParamConverter))]
    public abstract class AssetBuildParam
    {



        public string Name
        {
            get;
            private set;
        }

        internal string SubName
        {
            get;
            private set;
        }

        [Browsable(false)]
        public abstract string Value
        {
            get;
        }



        protected AssetBuildParam(string name_)
        {
            if (string.IsNullOrWhiteSpace(name_) == true)
            {
                throw new ArgumentNullException("AssetBuildParams() : name is null or empty");
            }

            SetName(name_);
        }

        protected AssetBuildParam(XmlElement el_)
        {
            Load(el_);
        }



        private void SetName(string name_)
        {
            SubName = name_;
            Name = "ProcessorParameters_" + name_;
        }

        public void Load(XmlElement el_)
        {
            XmlNode node = el_.SelectSingleNode("Name");
            SetName(node.InnerText);
            node = el_.SelectSingleNode("Value");
            LoadValue(node.InnerText);
        }

        protected abstract void LoadValue(string val_);

        public void Save(XmlElement el_)
        {
            XmlElement node = el_.OwnerDocument.CreateElementWithText("Name", SubName);
            el_.AppendChild(node);
            node = el_.OwnerDocument.CreateElementWithText("Value", Value);
            el_.AppendChild(node);
        }

        public abstract bool Compare(AssetBuildParam param_);

    }


    public class AssetBuildParamColor
        : AssetBuildParam
    {
        [Description("If the texture is color-keyed, pixels of this color are replaced with transparent black.")]
        public Color ColorKey
        {
            get;
            set;
        }

        public override string Value => ColorKey.R + ", " + ColorKey.G + ", " + ColorKey.B + ", " + ColorKey.A;

        public AssetBuildParamColor(XmlElement el_)
            : base(el_)
        { }

        public AssetBuildParamColor()
            : base("ColorKeyColor")
        {
            ColorKey = new Color(255, 0, 255);
        }

        protected override void LoadValue(string val_)
        {
            string[] a = val_.Split(',');

            ColorKey = new Color(
                byte.Parse(a[0]),
                byte.Parse(a[1]),
                byte.Parse(a[2]));
        }

        public override bool Compare(AssetBuildParam param_)
        {
            AssetBuildParamColor o = (AssetBuildParamColor)param_;

            if (o != null)
            {
                return ColorKey == o.ColorKey;
            }

            return false;
        }
    }

    public class AssetBuildParamColorKeyEnabled
        : AssetBuildParam
    {
        [Description("If enabled, the source texture is color keyed. Pixels matching the value of \"Color Key Color\" are replaced with transparent black.")]
        public bool ColorKeyEnabled
        {
            get;
            set;
        }

        public override string Value => ColorKeyEnabled.ToString();

        public AssetBuildParamColorKeyEnabled()
            : base("ColorKeyEnabled")
        {
            ColorKeyEnabled = true;
        }

        public AssetBuildParamColorKeyEnabled(XmlElement el_)
            : base(el_)
        { }

        protected override void LoadValue(string val_)
        {
            ColorKeyEnabled = bool.Parse(val_);
        }

        public override bool Compare(AssetBuildParam param_)
        {
            AssetBuildParamColorKeyEnabled o = (AssetBuildParamColorKeyEnabled)param_;

            if (o != null)
            {
                return ColorKeyEnabled == o.ColorKeyEnabled;
            }

            return false;
        }
    }

    public class AssetBuildParamGenerateMipmaps
        : AssetBuildParam
    {
        [Description("If enabled, a full mipmap chain is generated from the source texture. Existing mipmaps are not replaced.")]
        public bool GenerateMipmaps
        {
            get;
            set;
        }

        public override string Value => GenerateMipmaps.ToString();

        public AssetBuildParamGenerateMipmaps()
            : base("GenerateMipmaps")
        {
            GenerateMipmaps = true;
        }

        public AssetBuildParamGenerateMipmaps(XmlElement el_)
            : base(el_)
        { }

        protected override void LoadValue(string val_)
        {
            GenerateMipmaps = bool.Parse(val_);
        }

        public override bool Compare(AssetBuildParam param_)
        {
            AssetBuildParamGenerateMipmaps o = (AssetBuildParamGenerateMipmaps)param_;

            if (o != null)
            {
                return GenerateMipmaps == o.GenerateMipmaps;
            }

            return false;
        }
    }

    public class AssetBuildParamPremultiplyAlpha
        : AssetBuildParam
    {
        [Description("If enabled, the texture is converted to premultiplied alpha format.")]
        public bool PremultiplyAlpha
        {
            get;
            set;
        }

        public override string Value => PremultiplyAlpha.ToString();

        public AssetBuildParamPremultiplyAlpha()
            : base("PremultiplyAlpha")
        {
            PremultiplyAlpha = true;
        }

        public AssetBuildParamPremultiplyAlpha(XmlElement el_)
            : base(el_)
        { }

        protected override void LoadValue(string val_)
        {
            PremultiplyAlpha = bool.Parse(val_);
        }

        public override bool Compare(AssetBuildParam param_)
        {
            AssetBuildParamPremultiplyAlpha o = (AssetBuildParamPremultiplyAlpha)param_;

            if (o != null)
            {
                return PremultiplyAlpha == o.PremultiplyAlpha;
            }

            return false;
        }
    }

    public class AssetBuildParamResizeToPowerOfTwo
        : AssetBuildParam
    {
        [Description("If enabled, the texture is resized to the next largest power of two, maximizing compatibility. Many graphics cards do not support textures sizes that are not a power of two.")]
        public bool ResizeToPowerOfTwo
        {
            get;
            set;
        }

        public override string Value => ResizeToPowerOfTwo.ToString();

        public AssetBuildParamResizeToPowerOfTwo()
            : base("ResizeToPowerOfTwo")
        {
            ResizeToPowerOfTwo = true;
        }

        public AssetBuildParamResizeToPowerOfTwo(XmlElement el_)
            : base(el_)
        { }

        protected override void LoadValue(string val_)
        {
            ResizeToPowerOfTwo = bool.Parse(val_);
        }

        public override bool Compare(AssetBuildParam param_)
        {
            AssetBuildParamResizeToPowerOfTwo o = (AssetBuildParamResizeToPowerOfTwo)param_;

            if (o != null)
            {
                return ResizeToPowerOfTwo == o.ResizeToPowerOfTwo;
            }

            return false;
        }
    }

    public class AssetBuildParamTextureFormat
        : AssetBuildParam
    {
        public enum TextureFormat
        {
            NoChange,
            Color,
            DxtCompressed
        }

        [Description("Specifies the SurfaceFormat type of processed textures. Textures can either remain unchanged from the source asset, converted to the Color format, or DXT compressed.")]
        public TextureFormat Format
        {
            get;
            set;
        }

        public override string Value => Enum.GetName(typeof(TextureFormat), Format);

        public AssetBuildParamTextureFormat()
            : base("TextureFormat")
        {
            Format = TextureFormat.NoChange;
        }

        public AssetBuildParamTextureFormat(XmlElement el_)
            : base(el_)
        { }

        protected override void LoadValue(string val_)
        {
            Format = (TextureFormat)Enum.Parse(typeof(TextureFormat), val_);
        }

        public override bool Compare(AssetBuildParam param_)
        {
            AssetBuildParamTextureFormat o = (AssetBuildParamTextureFormat)param_;

            if (o != null)
            {
                return Format == o.Format;
            }

            return false;
        }
    }



    public class AssetBuildParamDebuggingOptions
        : AssetBuildParam
    {
        public enum DebuggingOptions
        {
            Auto,
            Debug,
            Optimize
        }

        [Description("")]
        public DebuggingOptions Option
        {
            get;
            set;
        }

        public override string Value => Enum.GetName(typeof(DebuggingOptions), Option);

        public AssetBuildParamDebuggingOptions()
            : base("DebuggingOptions")
        {
            Option = DebuggingOptions.Auto;
        }

        public AssetBuildParamDebuggingOptions(XmlElement el_)
            : base(el_)
        { }

        protected override void LoadValue(string val_)
        {
            Option = (DebuggingOptions)Enum.Parse(typeof(DebuggingOptions), val_);
        }

        public override bool Compare(AssetBuildParam param_)
        {
            AssetBuildParamDebuggingOptions o = (AssetBuildParamDebuggingOptions)param_;

            if (o != null)
            {
                return Option == o.Option;
            }

            return false;
        }
    }

    public class AssetBuildParamDefines
        : AssetBuildParam
    {
        [Description("")]
        public string Defines
        {
            get;
            set;
        }

        public override string Value => Defines;

        public AssetBuildParamDefines()
            : base("Defines")
        {
            Defines = string.Empty;
        }

        public AssetBuildParamDefines(XmlElement el_)
            : base(el_)
        { }

        protected override void LoadValue(string val_)
        {
            Defines = val_;
        }

        public override bool Compare(AssetBuildParam param_)
        {
            AssetBuildParamDefines o = (AssetBuildParamDefines)param_;

            if (o != null)
            {
                return Defines == o.Defines;
            }

            return false;
        }
    }



    public class AssetBuildParamFirstCharacter
        : AssetBuildParam
    {
        [Description("")]
        public string FirstCharacter
        {
            get;
            set;
        }

        public override string Value => FirstCharacter;

        public AssetBuildParamFirstCharacter()
            : base("FirstCharacter")
        {
            FirstCharacter = string.Empty;
        }

        public AssetBuildParamFirstCharacter(XmlElement el_)
            : base(el_)
        { }

        protected override void LoadValue(string val_)
        {
            FirstCharacter = val_;
        }

        public override bool Compare(AssetBuildParam param_)
        {
            AssetBuildParamFirstCharacter o = (AssetBuildParamFirstCharacter)param_;

            if (o != null)
            {
                return FirstCharacter == o.FirstCharacter;
            }

            return false;
        }
    }



    public class AssetBuildParamDefaultEffect
        : AssetBuildParam
    {
        public enum DefaultEffect
        {
            BasicEffect,
            SkinnedEffect,
            EnvironmantEffect,
            DUalTextureEffect,
            AlphaTextureEffect
        }

        [Description("")]
        public DefaultEffect Effect
        {
            get;
            set;
        }

        public override string Value => Enum.GetName(typeof(DefaultEffect), Effect);

        public AssetBuildParamDefaultEffect()
            : base("DefaultEffect")
        {
            Effect = DefaultEffect.BasicEffect;
        }

        public AssetBuildParamDefaultEffect(XmlElement el_)
            : base(el_)
        { }

        protected override void LoadValue(string val_)
        {
            Effect = (DefaultEffect)Enum.Parse(typeof(DefaultEffect), val_);
        }

        public override bool Compare(AssetBuildParam param_)
        {
            AssetBuildParamDefaultEffect o = (AssetBuildParamDefaultEffect)param_;

            if (o != null)
            {
                return Effect == o.Effect;
            }

            return false;
        }
    }

    public class AssetBuildParamPremultiplyTextureAlpha
        : AssetBuildParam
    {
        [Description("")]
        public bool PremultiplyTextureAlpha
        {
            get;
            set;
        }

        public override string Value => PremultiplyTextureAlpha.ToString();

        public AssetBuildParamPremultiplyTextureAlpha()
            : base("PremultiplyTextureAlpha")
        {
            PremultiplyTextureAlpha = false;
        }

        public AssetBuildParamPremultiplyTextureAlpha(XmlElement el_)
            : base(el_)
        { }

        protected override void LoadValue(string val_)
        {
            PremultiplyTextureAlpha = bool.Parse(val_);
        }

        public override bool Compare(AssetBuildParam param_)
        {
            AssetBuildParamPremultiplyTextureAlpha o = (AssetBuildParamPremultiplyTextureAlpha)param_;

            if (o != null)
            {
                return PremultiplyTextureAlpha == o.PremultiplyTextureAlpha;
            }

            return false;
        }
    }

    public class AssetBuildParamPremultiplyVertexColor
        : AssetBuildParam
    {
        [Description("")]
        public bool PremultiplyVertexColor
        {
            get;
            set;
        }

        public override string Value => PremultiplyVertexColor.ToString();

        public AssetBuildParamPremultiplyVertexColor()
            : base("PremultiplyVertexColor")
        {
            PremultiplyVertexColor = false;
        }

        public AssetBuildParamPremultiplyVertexColor(XmlElement el_)
            : base(el_)
        { }

        protected override void LoadValue(string val_)
        {
            PremultiplyVertexColor = bool.Parse(val_);
        }

        public override bool Compare(AssetBuildParam param_)
        {
            AssetBuildParamPremultiplyVertexColor o = (AssetBuildParamPremultiplyVertexColor)param_;

            if (o != null)
            {
                return PremultiplyVertexColor == o.PremultiplyVertexColor;
            }

            return false;
        }
    }

    public class AssetBuildParamGenerateTangentFrames
        : AssetBuildParam
    {
        [Description("")]
        public bool GenerateTangentFrames
        {
            get;
            set;
        }

        public override string Value => GenerateTangentFrames.ToString();

        public AssetBuildParamGenerateTangentFrames()
            : base("GenerateTangentFrames")
        {
            GenerateTangentFrames = false;
        }

        public AssetBuildParamGenerateTangentFrames(XmlElement el_)
            : base(el_)
        { }

        protected override void LoadValue(string val_)
        {
            GenerateTangentFrames = bool.Parse(val_);
        }

        public override bool Compare(AssetBuildParam param_)
        {
            AssetBuildParamGenerateTangentFrames o = (AssetBuildParamGenerateTangentFrames)param_;

            if (o != null)
            {
                return GenerateTangentFrames == o.GenerateTangentFrames;
            }

            return false;
        }
    }

    public class AssetBuildParamScale
        : AssetBuildParam
    {
        [Description("")]
        public float Scale
        {
            get;
            set;
        }

        public override string Value => Scale.ToString();

        public AssetBuildParamScale()
            : base("Scale")
        {
            Scale = 1.0f;
        }

        public AssetBuildParamScale(XmlElement el_)
            : base(el_)
        { }

        protected override void LoadValue(string val_)
        {
            Scale = float.Parse(val_);
        }

        public override bool Compare(AssetBuildParam param_)
        {
            AssetBuildParamScale o = (AssetBuildParamScale)param_;

            if (o != null)
            {
                return Scale == o.Scale;
            }

            return false;
        }
    }

    public class AssetBuildParamSwapWindingOrder
        : AssetBuildParam
    {
        [Description("")]
        public bool SwapWindingOrder
        {
            get;
            set;
        }

        public override string Value => SwapWindingOrder.ToString();

        public AssetBuildParamSwapWindingOrder()
            : base("SwapWindingOrder")
        {
            SwapWindingOrder = false;
        }

        public AssetBuildParamSwapWindingOrder(XmlElement el_)
            : base(el_)
        { }

        protected override void LoadValue(string val_)
        {
            SwapWindingOrder = bool.Parse(val_);
        }

        public override bool Compare(AssetBuildParam param_)
        {
            AssetBuildParamSwapWindingOrder o = (AssetBuildParamSwapWindingOrder)param_;

            if (o != null)
            {
                return SwapWindingOrder == o.SwapWindingOrder;
            }

            return false;
        }
    }

    public class AssetBuildParamXRotation
        : AssetBuildParam
    {
        [Description("")]
        public float XRotation
        {
            get;
            set;
        }

        public override string Value => XRotation.ToString();

        public AssetBuildParamXRotation()
            : base("XRotation")
        {
            XRotation = 0.0f;
        }

        public AssetBuildParamXRotation(XmlElement el_)
            : base(el_)
        { }

        protected override void LoadValue(string val_)
        {
            XRotation = float.Parse(val_);
        }

        public override bool Compare(AssetBuildParam param_)
        {
            AssetBuildParamXRotation o = (AssetBuildParamXRotation)param_;

            if (o != null)
            {
                return XRotation == o.XRotation;
            }

            return false;
        }
    }

    public class AssetBuildParamYRotation
        : AssetBuildParam
    {
        [Description("")]
        public float YRotation
        {
            get;
            set;
        }

        public override string Value => YRotation.ToString();

        public AssetBuildParamYRotation()
            : base("YRotation")
        {
            YRotation = 0.0f;
        }

        public AssetBuildParamYRotation(XmlElement el_)
            : base(el_)
        { }

        protected override void LoadValue(string val_)
        {
            YRotation = float.Parse(val_);
        }

        public override bool Compare(AssetBuildParam param_)
        {
            AssetBuildParamYRotation o = (AssetBuildParamYRotation)param_;

            if (o != null)
            {
                return YRotation == o.YRotation;
            }

            return false;
        }
    }

    public class AssetBuildParamZRotation
        : AssetBuildParam
    {
        [Description("")]
        public float ZRotation
        {
            get;
            set;
        }

        public override string Value => ZRotation.ToString();

        public AssetBuildParamZRotation()
            : base("ZRotation")
        {
            ZRotation = 0.0f;
        }

        public AssetBuildParamZRotation(XmlElement el_)
            : base(el_)
        { }

        protected override void LoadValue(string val_)
        {
            ZRotation = float.Parse(val_);
        }

        public override bool Compare(AssetBuildParam param_)
        {
            AssetBuildParamZRotation o = (AssetBuildParamZRotation)param_;

            if (o != null)
            {
                return ZRotation == o.ZRotation;
            }

            return false;
        }
    }



    public class AssetBuildParamCompressionQuality
        : AssetBuildParam
    {
        public enum CompressionQuality
        {
            Low,
            Medium,
            Best
        }

        [Description("")]
        public CompressionQuality Quality
        {
            get;
            set;
        }

        public override string Value => Enum.GetName(typeof(CompressionQuality), Quality);

        public AssetBuildParamCompressionQuality()
            : base("CompressionQuality")
        {
            Quality = CompressionQuality.Best;
        }

        public AssetBuildParamCompressionQuality(XmlElement el_)
            : base(el_)
        { }

        protected override void LoadValue(string val_)
        {
            Quality = (CompressionQuality)Enum.Parse(typeof(CompressionQuality), val_);
        }

        public override bool Compare(AssetBuildParam param_)
        {
            AssetBuildParamCompressionQuality o = (AssetBuildParamCompressionQuality)param_;

            if (o != null)
            {
                return Quality == o.Quality;
            }

            return false;
        }
    }



    public class AssetBuildParamVideoSoundTrackType
        : AssetBuildParam
    {
        public enum VideoSoundTrackType
        {
            Low,
            Medium,
            Best
        }

        [Description("")]
        public VideoSoundTrackType TrackType
        {
            get;
            set;
        }

        public override string Value => Enum.GetName(typeof(VideoSoundTrackType), TrackType);

        public AssetBuildParamVideoSoundTrackType()
            : base("VideoSoundTrackType")
        {
            TrackType = VideoSoundTrackType.Best;
        }

        public AssetBuildParamVideoSoundTrackType(XmlElement el_)
            : base(el_)
        { }

        protected override void LoadValue(string val_)
        {
            TrackType = (VideoSoundTrackType)Enum.Parse(typeof(VideoSoundTrackType), val_);
        }

        public override bool Compare(AssetBuildParam param_)
        {
            AssetBuildParamVideoSoundTrackType o = (AssetBuildParamVideoSoundTrackType)param_;

            if (o != null)
            {
                return TrackType == o.TrackType;
            }

            return false;
        }
    }

}
