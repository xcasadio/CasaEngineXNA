﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CasaEngine.Graphics2D;
using CasaEngine.Game;
using CasaEngine.CoreSystems.Game;

namespace CasaEngine.FrontEnd.Screen
{
    /// <summary>
    /// 
    /// </summary>
    public class DebugScreen
        : Screen
    {
        #region Fields

        int selectedEntry = 0;
		string menuTitle;

		Renderer2DComponent m_Renderer2DComponent = null;

        #endregion

        #region Constructor

        /// <summary>
		/// 
		/// </summary>
		/// <param name="menuTitle"></param>
		/// <param name="menuName_"></param>
        public DebugScreen(string menuTitle, string menuName_)
			: base(menuName_)
		{
			this.menuTitle = menuTitle;

			TransitionOnTime = TimeSpan.FromSeconds(0.5);
			TransitionOffTime = TimeSpan.FromSeconds(0.5);

			m_Renderer2DComponent = GameHelper.GetGameComponent<Renderer2DComponent>(Engine.Instance.Game);
		}

        #endregion
    }

    
}