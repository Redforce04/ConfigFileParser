using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigFileParser.Components
{
    /// <summary>
    /// Specifies a description for a property or event.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ConfigNameAttribute : Attribute
    {
        /// <summary>
        /// Specifies the Config value for the <see cref='ConfigNameAttribute'/>,
        /// which is an empty string (""). This <see langword='static'/> field is read-only.
        /// </summary>
        public static readonly ConfigNameAttribute Default = new ConfigNameAttribute();

        public ConfigNameAttribute() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='ConfigNameAttribute'/> class.
        /// </summary>
        public ConfigNameAttribute(string _configName)
        {
            configName = _configName;
        }

        /// <summary>
        /// Gets the ConfigValue stored in this attribute.
        /// </summary>
        public virtual string ConfigName => configName;

        /// <summary>
        /// Read/Write property that directly modifies the string stored in the ConfigValue
        /// attribute. The Config implementation of the <see cref="ConfigName"/> property
        /// simply returns this value.
        /// </summary>
        protected string configName { get; set; }

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is ConfigNameAttribute other && other.ConfigName == ConfigName;

        public override int GetHashCode() => ConfigName?.GetHashCode() ?? 0;

        public override bool IsDefaultAttribute() => Equals(Default);
    }

}
