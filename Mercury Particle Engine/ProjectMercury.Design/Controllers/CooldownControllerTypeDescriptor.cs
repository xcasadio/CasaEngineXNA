/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.Design.Controllers
{
    using System;
    using System.ComponentModel;
    using ProjectMercury.Controllers;

    /// <summary>
    /// Defines the custom type descriptor for the cooldown controller class.
    /// </summary>
    public sealed class CooldownControllerTypeDescriptor : AbstractTypeDescriptor<CooldownController>
    {
        private readonly Type ControllerType = typeof(CooldownController);

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
                 PropertyDescriptorFactory.Create(ControllerType.GetProperty("CooldownPeriod"),
                    new DisplayNameAttribute("Cooldown Period"),
                    new DescriptionAttribute("The cooldown period after the emitter has been triggered."))
             });
        }
    }
}