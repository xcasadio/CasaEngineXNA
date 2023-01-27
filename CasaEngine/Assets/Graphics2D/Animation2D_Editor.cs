﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using CasaEngineCommon.Extension;
using CasaEngine.Gameplay.Actor.Event;
using CasaEngineCommon.Design;
using CasaEngine.Gameplay.Actor.Object;

namespace CasaEngine.Assets.Graphics2D
{
	/// <summary>
	/// 
	/// </summary>
	public partial class Animation2D
	{
		#region Fields

        static private readonly uint m_Version = 1;
        //static private uint m_UnusedID = 0;

		#endregion

		#region Properties

		/// <summary>
		/// Gets Frame Count
		/// </summary>
		public int FrameCount
		{
			get { return m_Frames.Count; }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteId_"></param>
        /// <param name="delay_"></param>
        public void AddFrame(int spriteId_, float delay_, EventActor[] events_)
        {
            Frame2D frame = new Frame2D(spriteId_, delay_);
#if EDITOR
            if (events_ != null)
            {
                frame.Events.AddRange(events_);
            }
#else
            frame.Events = events_;
#endif
            m_Frames.Add(frame);
            ComputeTotalTime();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frameIndex_"></param>
        /// <returns></returns>
        public float GetFrameTime(int frameIndex_)
        {
            return m_Frames[frameIndex_].time;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="totalElapsedTime_"></param>
		public void SetTime(float totalElapsedTime_)
		{
			m_CurrentTime = totalElapsedTime_;
			ComputeCurrentFrame();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index_"></param>
		public void DeleteFrame(int index_)
		{
			m_Frames.RemoveAt(index_);
            ComputeTotalTime();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="frameIndex_"></param>
		/// <param name="delay_"></param>
		public void SetFrameDelay(int frameIndex_, float delay_)
		{
			Frame2D frame = m_Frames[frameIndex_];
			frame.time = delay_;
			m_Frames[frameIndex_] = frame;
            ComputeTotalTime();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frameIndex_"></param>
        /// <param name="eventList_"></param>
        public void SetFrameEvents(int frameIndex_, List<EventActor> eventList_)
        {
            Frame2D frame = m_Frames[frameIndex_];
            frame.Events = eventList_;
            m_Frames[frameIndex_] = frame;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="frameIndex_"></param>
		public void SetCurrentFrame(int frameIndex_)
		{
			m_CurrentTime = m_Frames[frameIndex_].time;
			m_CurrentFrame = frameIndex_;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public int GetCurrentFrameIndex()
		{
			return m_CurrentFrame;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sprite2DID"></param>
		/// <param name="frameIndex_"></param>
		public void SetFrameSprite2D(int sprite2DID, int frameIndex_)
		{
			Frame2D frame = m_Frames[frameIndex_];
			frame.spriteID = sprite2DID;
			m_Frames[frameIndex_] = frame;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="el_"></param>
		public override void Save(XmlElement el_, SaveOption option_)
		{
            base.Save(el_, option_);

            XmlElement animNode = el_.OwnerDocument.CreateElement("Animation2D");
            el_.AppendChild(animNode);

            el_.OwnerDocument.AddAttribute(animNode, "version", m_Version.ToString());
            el_.OwnerDocument.AddAttribute(animNode, "name", m_Name);
            el_.OwnerDocument.AddAttribute(animNode, "type", Enum.GetName(typeof(Animation2DType), m_Animation2DType));

			XmlElement frameListNode = el_.OwnerDocument.CreateElement("FrameList");
            animNode.AppendChild(frameListNode);

			for (int i = 0; i < m_Frames.Count; i++)
			{
				XmlElement frameNode = el_.OwnerDocument.CreateElement("Frame");
				el_.OwnerDocument.AddAttribute(frameNode, "spriteID", m_Frames[i].spriteID.ToString());
				el_.OwnerDocument.AddAttribute(frameNode, "time", GetFrameTime(i).ToString());

                //events
                XmlElement eventListNode = el_.OwnerDocument.CreateElement("EventNodeList");
                frameNode.AppendChild(eventListNode);
                foreach (EventActor e in m_Frames[i].Events)
                {
                    XmlElement eventNode = el_.OwnerDocument.CreateElement("EventNode");
                    eventListNode.AppendChild(eventNode);
                    e.Save(eventNode, option_);
                }

				frameListNode.AppendChild(frameNode);
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="el_"></param>
        public override void Save(BinaryWriter bw_, SaveOption option_)
        {
            base.Save(bw_, option_);

            bw_.Write(m_Version);
            bw_.Write(m_Name);
            bw_.Write((int)m_Animation2DType);
            bw_.Write(m_Frames.Count);

            for (int i = 0; i < m_Frames.Count; i++)
            {
                bw_.Write(m_Frames[i].spriteID);
                bw_.Write(GetFrameTime(i));
                bw_.Write(m_Frames[i].Events.Count);

                foreach (EventActor e in m_Frames[i].Events)
                {
                    e.Save(bw_, option_);
                }
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index_"></param>
		public void MoveFrameForward(int index_)
		{
			if (index_ < m_Frames.Count - 1)
			{
				Frame2D frameTmp = m_Frames[index_ + 1];
				m_Frames[index_ + 1] = m_Frames[index_];
				m_Frames[index_] = frameTmp;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index_"></param>
		public void MoveFrameBackward(int index_)
		{
			if (index_ > 0)
			{
				Frame2D frameTmp = m_Frames[index_ - 1];
				m_Frames[index_ - 1] = m_Frames[index_];
				m_Frames[index_] = frameTmp;
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other_"></param>
        /// <returns></returns>
        public override bool CompareTo(BaseObject other_)
        {
            if (other_ is Animation2D)
            {
                Animation2D o = other_ as Animation2D;

                if (this.m_Animation2DType != o.m_Animation2DType
                    || this.m_Frames.Count != o.m_Frames.Count)
                {
                    return false;
                }

                for (int i = 0; i < m_Frames.Count; i++)
                {
                    if (m_Frames[i].CompareTo(o.m_Frames[i]) == false)
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

		#endregion
	}
}