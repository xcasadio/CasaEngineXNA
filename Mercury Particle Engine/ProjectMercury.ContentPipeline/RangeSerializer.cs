/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.ContentPipeline
{
    using System;
    using System.Globalization;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
    using ProjectMercury;

    /// <summary>
    /// Provides an implementation of ContentTypeSerializer methods and properties for serializing
    /// and deserializing Range objects.
    /// </summary>
    [ContentTypeSerializer]
    public sealed class RangeSerializer : ContentTypeSerializer<Range>
    {
        /// <summary>
        /// Serializes an object to intermediate XML format.
        /// </summary>
        /// <param name="output">Specifies the intermediate XML location, and provides various serialization helpers.</param>
        /// <param name="value">The strongly typed object to be serialized.</param>
        /// <param name="format">Specifies the content format for this object.</param>
        protected override void Serialize(IntermediateWriter output, Range value, ContentSerializerAttribute format)
        {
            CultureInfo invariantCulture = CultureInfo.InvariantCulture;

            output.Xml.WriteString(String.Format(invariantCulture, "{0:G}", value));
        }

        /// <summary>
        /// Deserializes a Range object from intermediate XML format.
        /// </summary>
        /// <param name="input">Location of the intermediate XML and various deserialization helpers.</param>
        /// <param name="format">Specifies the intermediate source XML format.</param>
        /// <param name="existingInstance">The strongly typed object containing the received data, or null if the
        /// deserializer should construct a new instance.</param>
        /// <returns>A deserialized Range instance.</returns>
        protected override Range Deserialize(IntermediateReader input, ContentSerializerAttribute format, Range existingInstance)
        {
            Range value = default(Range);

            CultureInfo invariantCulture = CultureInfo.InvariantCulture;

            value = Range.Parse(input.Xml.ReadString(), invariantCulture);

            return value;
        }
    }
}