using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigFileParser.Components
{
    /// <summary>
    /// Specifies a name for a property or event.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class NameAttribute : Attribute
    {
        /// <summary>
        /// Specifies the default value for the <see cref='NameAttribute'/>,
        /// which is an empty string (""). This <see langword='static'/> field is read-only.
        /// </summary>
        public static readonly NameAttribute Default = new NameAttribute();

        public NameAttribute() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='NameAttribute'/> class.
        /// </summary>
        public NameAttribute(string name)
        {
            NameValue = name;
        }

        /// <summary>
        /// Gets the name stored in this attribute.
        /// </summary>
        public virtual string Name => NameValue;

        /// <summary>
        /// Read/Write property that directly modifies the string stored in the name
        /// attribute. The default implementation of the <see cref="Name"/> property
        /// simply returns this value.
        /// </summary>
        protected string NameValue { get; set; }

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is NameAttribute other && other.Name == Name;

        public override int GetHashCode() => Name?.GetHashCode() ?? 0;

        public override bool IsDefaultAttribute() => Equals(Default);
    }
}
