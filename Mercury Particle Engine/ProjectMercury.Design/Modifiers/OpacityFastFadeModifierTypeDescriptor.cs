/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.Design.Modifiers
{
    using System;
    using System.ComponentModel;
    using ProjectMercury.Modifiers;

    /// <summary>
    /// Defines the custom type descriptor for the opacity fast fade modifier class.
    /// </summary>
    public sealed class OpacityFastFadeModifierTypeDescriptor : AbstractTypeDescriptor<OpacityFastFadeModifier>
    {
        private readonly Type ModifierType = typeof(OpacityFastFadeModifier);

        /// <summary>
        /// Returns a collection of property descriptors for the object represented by this type descriptor.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> containing the property descriptions for the object represented by this type descriptor. The default is <see cref="F:System.ComponentModel.PropertyDescriptorCollection.Empty"/>.
        /// </returns>
        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return new PropertyDescriptorCollection(new PropertyDescriptor[]
            {
                PropertyDescriptorFactory.Create(ModifierType.GetProperty("InitialOpacity"),
                    new CategoryAttribute("Opacity Fast Fade Modifier"),
                    new DisplayNameAttribute("Initial Opacity"),
                    new DescriptionAttribute("Gets or sets the initial opacity of particles."))
            });
        }
    }
}