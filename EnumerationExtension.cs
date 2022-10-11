using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Markup;

namespace Aurora.Wpf
{
    /// <summary>
    /// enumeration extension to use the enumeration to within XMAL
    /// </summary>
    public class EnumerationExtension : MarkupExtension
    {
        private Type? m_EnumType;
        /// <summary>
        /// instantiate the class for the given enum type
        /// </summary>
        /// <param name="enumType">type of the enum</param>
        /// <exception cref="ArgumentNullException">enumType cannot be null</exception>
        public EnumerationExtension(Type? enumType)
        {
            EnumType = enumType ?? throw new ArgumentNullException(nameof(enumType));
        }

        /// <summary>
        /// property to acces the EnumType to handle 
        /// </summary>
        /// <exception cref="ArgumentException">the underlying type of the specified type must be a enum type</exception>
        public Type? EnumType
        {
            get => m_EnumType;
            private set
            {
                if (m_EnumType == value)
                    return;

                var enumType = Nullable.GetUnderlyingType(value) ?? value;

                if (enumType.IsEnum == false)
                    throw new ArgumentException("Type must be an Enum.");

                m_EnumType = value;
            }
        }

        /// <summary>
        /// Provide the value of the enum for the XAML service provider
        /// </summary>
        /// <param name="serviceProvider">Provider to handle the assignment to XAML elements</param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Debug.Assert(EnumType != null, nameof(EnumType) + " != null");
            var enumValues = Enum.GetValues(EnumType);

            return (
                from object enumValue in enumValues
                select new EnumerationMember
                {
                    Value = enumValue,
                    Description = GetDescription(enumValue) ?? string.Empty
                }).ToArray();
        }

        private string? GetDescription(object enumValue)
        {
            if (enumValue == null)
                throw new ArgumentNullException(nameof(enumValue));
            var fieldInfo = EnumType.GetField(enumValue.ToString()!);
            string? retVal = fieldInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() is DescriptionAttribute descriptionAttribute
                ? descriptionAttribute.Description
                : enumValue.ToString();
            return retVal;
        }

        /// <summary>
        /// class to describe a EnumerationMember
        /// </summary>
        public class EnumerationMember
        {
            /// <summary>
            /// description for the enum member
            /// </summary>
            public string Description { get; set; } = string.Empty;
            /// <summary>
            /// value of the enum member
            /// </summary>
            public object Value { get; set; } = string.Empty;
        }
    }
}
