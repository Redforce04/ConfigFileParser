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
    public class DefaultValueAttribute : Attribute
    {
        /// <summary>
        /// Specifies the default value for the <see cref='DefaultValueAttribute'/>,
        /// which is an empty string (""). This <see langword='static'/> field is read-only.
        /// </summary>
        public static readonly DefaultValueAttribute Default = new DefaultValueAttribute();

        public DefaultValueAttribute() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='DefaultValueAttribute'/> class.
        /// </summary>
        public DefaultValueAttribute(string _defaultValue)
        {
            defaultValue = _defaultValue;
        }

        /// <summary>
        /// Gets the DefaultValue stored in this attribute.
        /// </summary>
        public virtual string DefaultValue => defaultValue;

        /// <summary>
        /// Read/Write property that directly modifies the string stored in the DefaultValue
        /// attribute. The default implementation of the <see cref="DefaultValue"/> property
        /// simply returns this value.
        /// </summary>
        protected string defaultValue { get; set; }

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is DefaultValueAttribute other && other.DefaultValue == DefaultValue;

        public override int GetHashCode() => DefaultValue?.GetHashCode() ?? 0;

        public override bool IsDefaultAttribute() => Equals(Default);
    }

}
