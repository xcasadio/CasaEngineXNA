/*
 * Copyright © 2011 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.ContentPipeline.Modifiers
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
    using ProjectMercury.Controllers;

    /// <summary>
    /// Provides an implementation of ContentTypeSerializer methods and properties for serializing
    /// and deserializing TimedReleaseQuantityController objects.
    /// </summary>
    [ContentTypeSerializer]
    public sealed class TimedReleaseQuantityControllerSerializer : ContentTypeSerializer<TimedReleaseQuantityController>
    {
        /// <summary>
        /// Serializes an object to intermediate XML format.
        /// </summary>
        /// <param name="output">Specifies the intermediate XML location, and provides various serialization helpers.</param>
        /// <param name="value">The strongly typed object to be serialized.</param>
        /// <param name="format">Specifies the content format for this object.</param>
        protected override void Serialize(IntermediateWriter output, TimedReleaseQuantityController value, ContentSerializerAttribute format)
        {
        }

        /// <summary>
        /// Deserializes a TimedReleaseQuantityController object from intermediate XML format.
        /// </summary>
        /// <param name="input">Location of the intermediate XML and various deserialization helpers.</param>
        /// <param name="format">Specifies the intermediate source XML format.</param>
        /// <param name="existingInstance">The strongly typed object containing the received data, or null if the
        /// deserializer should construct a new instance.</param>
        /// <returns>A deserialized TimedReleaseQuantityController instance.</returns>
        protected override TimedReleaseQuantityController Deserialize(IntermediateReader input, ContentSerializerAttribute format, TimedReleaseQuantityController existingInstance)
        {
            TimedReleaseQuantityController value = existingInstance ?? new TimedReleaseQuantityController();

            return value;
        }
    }
}