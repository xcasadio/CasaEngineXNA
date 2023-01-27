﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Editor.Sprite2DEditor.Event;
using CasaEngine.Gameplay.Actor.Event;

namespace Editor.Tools.Event.EventForm
{
    /// <summary>
    /// 
    /// </summary>
    internal class SoundEventWindowFactory
        : IEventWindowFactory
    {

        #region IEventWindowFactory Membres

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event_"></param>
        /// <returns></returns>
        public IAnimationEventBaseWindow CreateNewForm(EventActor event_)
        {
            return new AnimationSoundEventForm(event_);
        }

        #endregion
    }
}