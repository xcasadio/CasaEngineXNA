﻿//-----------------------------------------------------------------------------
// DebugCommandUI.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using CasaEngine.Game;
using CasaEngine.Graphics2D;
using CasaEngine.Helper;
using CasaEngine.Gameplay;
using CasaEngine.CoreSystems.Game;

#if EDITOR
//using CasaEngine.Editor.GameComponent;
#endif


namespace CasaEngine.Debugger
{
    public class DebugCommandUi
        : Microsoft.Xna.Framework.DrawableGameComponent,
        IDebugCommandHost,
        IGameComponentResizable
    {

        const int MaxLineCount = 20;

        const int MaxCommandHistory = 32;

        const string Cursor = "_";

        public const string DefaultPrompt = "CMD>";



        public bool CanSetVisible => true;

        public bool CanSetEnable => true;

        public string Prompt { get; set; }

        public bool Focused => _state != State.Closed;


        // Command window states.
        enum State
        {
            Closed,
            Opening,
            Opened,
            Closing
        }

        class CommandInfo
        {
            public CommandInfo(
                string command, string description, DebugCommandExecute callback)
            {
                this.Command = command;
                this.Description = description;
                this.Callback = callback;
            }

            // command name
            public readonly string Command;

            // Description of command.
            public readonly string Description;

            // delegate for execute the command.
            public readonly DebugCommandExecute Callback;
        }

        // Reference to DebugManager.
        private DebugManager _debugManager;

        // Current state
        private State _state = State.Closed;

        // timer for state transition.
        private float _stateTransition;

        // Registered echo listeners.
        private readonly List<IDebugEchoListner> _listenrs = new List<IDebugEchoListner>();

        // Registered command executioner.
        private readonly Stack<IDebugCommandExecutioner> _executioners = new Stack<IDebugCommandExecutioner>();

        // Registered commands
        private readonly Dictionary<string, CommandInfo> _commandTable =
                                                new Dictionary<string, CommandInfo>();

        // Current command line string and cursor position.
        private string _commandLine = String.Empty;
        private int _cursorIndex = 0;

        private readonly Queue<string> _lines = new Queue<string>();

        // Command history buffer.
        private readonly List<string> _commandHistory = new List<string>();

        // Selecting command history index.
        private int _commandHistoryIndex;

        private Renderer2DComponent _renderer2DComponent = null;

        private readonly Color _backgroundColor = new Color(0, 0, 0, 200);


        // Previous frame keyboard state.
        private KeyboardState _prevKeyState;

        // Key that pressed last frame.
        private Keys _pressedKey;

        // Timer for key repeating.
        private float _keyRepeatTimer;

        // Key repeat duration in seconds for the first key press.
        private readonly float _keyRepeatStartDuration = 0.3f;

        // Key repeat duration in seconds after the first key press.
        private readonly float _keyRepeatDuration = 0.03f;





        public DebugCommandUi(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            Prompt = DefaultPrompt;

            // Add this instance as a service.
            Game.Services.AddService(typeof(IDebugCommandHost), this);

            // Draw the command UI on top of everything
            DrawOrder = int.MaxValue;

            // Adding default commands.

            // Help command displays registered command information.
            RegisterCommand("help", "Show Command helps",
                delegate (IDebugCommandHost host, string command, IList<string> args)
                {
                    int maxLen = 0;
                    foreach (CommandInfo cmd in _commandTable.Values)
                        maxLen = System.Math.Max(maxLen, cmd.Command.Length);

                    string fmt = String.Format("{{0,-{0}}}    {{1}}", maxLen);

                    foreach (CommandInfo cmd in _commandTable.Values)
                    {
                        Echo(String.Format(fmt, cmd.Command, cmd.Description));
                    }
                });

            // Clear screen command
            RegisterCommand("cls", "Clear Screen",
                delegate (IDebugCommandHost host, string command, IList<string> args)
                {
                    _lines.Clear();
                });

            // Echo command
            RegisterCommand("echo", "Display Messages",
                delegate (IDebugCommandHost host, string command, IList<string> args)
                {
                    Echo(command.Substring(5));
                });

            // DisplayCollisions command
            RegisterCommand("displayCollisions", "Display Collisions of each sprite. Argument 'on'/'off'",
                delegate (IDebugCommandHost host, string command, IList<string> args)
                {
                    bool error = false;
                    bool state = false;

                    if (args.Count == 1)
                    {
                        if (args[0].ToLower().Equals("on") == true)
                        {
                            state = true;
                        }
                        else if (args[0].ToLower().Equals("off") == true)
                        {
                            state = false;
                        }
                        else
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        error = true;
                    }

                    if (error == true)
                    {
                        EchoError("Please use DisplayCollisions with one argument : 'on' or 'off'");
                    }
                    else
                    {
                        ShapeRendererComponent.DisplayCollisions = state;
                    }

                });

            // AnimationSpeed command
            RegisterCommand("AnimationSpeed", "Speed of animations.",
            delegate (IDebugCommandHost host, string command, IList<string> args)
            {
                bool ok = true;
                float value = 1.0f;

                if (args.Count == 1)
                {
                    args[0] = args[0].Replace(".", ",");
                    ok = float.TryParse(args[0], out value);
                }

                if (ok == false)
                {
                    EchoError("Please use AnimationSpeed with one argument");
                }
                else
                {
                    Animation2DPlayer.AnimationSpeed = value;
                }

            });

            // Character2DActor Display Debug Information command
            RegisterCommand("DisplayCharacterDebugInformation", "Display Debug Information from character.",
            delegate (IDebugCommandHost host, string command, IList<string> args)
            {
                bool error = false;
                bool state = false;

                if (args.Count == 1)
                {
                    if (args[0].ToLower().Equals("on") == true)
                    {
                        state = true;
                    }
                    else if (args[0].ToLower().Equals("off") == true)
                    {
                        state = false;
                    }
                    else
                    {
                        error = true;
                    }
                }
                else
                {
                    error = true;
                }

                if (error == true)
                {
                    EchoError("Please use DisplayCollisions with one argument : 'on' or 'off'");
                }
                else
                {
                    CharacterActor2D.DisplayDebugInformation = state;
                }
            });

            UpdateOrder = (int)ComponentUpdateOrder.DebugManager;
            DrawOrder = (int)ComponentDrawOrder.DebugManager;
        }

        public override void Initialize()
        {
            _debugManager =
                Game.Services.GetService(typeof(DebugManager)) as DebugManager;

            if (_debugManager == null)
                throw new InvalidOperationException("Coudn't find DebugManager.");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _renderer2DComponent = GameHelper.GetGameComponent<Renderer2DComponent>(Game);

            if (_renderer2DComponent == null)
            {
                throw new InvalidOperationException("DebugCommandUI.LoadContent() : Renderer2DComponent is null");
            }

            base.LoadContent();
        }



        public void RegisterCommand(
            string command, string description, DebugCommandExecute callback)
        {
            string lowerCommand = command.ToLower();
            if (_commandTable.ContainsKey(lowerCommand))
            {
                throw new InvalidOperationException(
                    String.Format("Command \"{0}\" is already registered.", command));
            }

            _commandTable.Add(
                lowerCommand, new CommandInfo(command, description, callback));
        }

        public void UnregisterCommand(string command)
        {
            string lowerCommand = command.ToLower();
            if (!_commandTable.ContainsKey(lowerCommand))
            {
                throw new InvalidOperationException(
                    String.Format("Command \"{0}\" is not registered.", command));
            }

            _commandTable.Remove(command);
        }

        public void ExecuteCommand(string command)
        {
            // Call registered executioner.
            if (_executioners.Count != 0)
            {
                _executioners.Peek().ExecuteCommand(command);
                return;
            }

            // Run the command.
            char[] spaceChars = new char[] { ' ' };

            Echo(Prompt + command);

            command = command.TrimStart(spaceChars);

            List<string> args = new List<string>(command.Split(spaceChars));
            string cmdText = args[0];
            args.RemoveAt(0);

            CommandInfo cmd;
            if (_commandTable.TryGetValue(cmdText.ToLower(), out cmd))
            {
                try
                {
                    // Call registered command delegate.
                    cmd.Callback(this, command, args);
                }
                catch (Exception e)
                {
                    // Exception occurred while running command.
                    EchoError("Unhandled Exception occurred");

                    string[] lines = e.Message.Split(new char[] { '\n' });
                    foreach (string line in lines)
                        EchoError(line);
                }
            }
            else
            {
                Echo("Unknown Command");
            }

            // Add to command history.
            _commandHistory.Add(command);
            while (_commandHistory.Count > MaxCommandHistory)
                _commandHistory.RemoveAt(0);

            _commandHistoryIndex = _commandHistory.Count;
        }

        public void RegisterEchoListner(IDebugEchoListner listner)
        {
            _listenrs.Add(listner);
        }

        public void UnregisterEchoListner(IDebugEchoListner listner)
        {
            _listenrs.Remove(listner);
        }

        public void Echo(DebugCommandMessage messageType, string text)
        {
            _lines.Enqueue(text);
            while (_lines.Count >= MaxLineCount)
                _lines.Dequeue();

            // Call registered listeners.
            foreach (IDebugEchoListner listner in _listenrs)
                listner.Echo(messageType, text);
        }

        public void Echo(string text)
        {
            Echo(DebugCommandMessage.Standard, text);
        }

        public void EchoWarning(string text)
        {
            Echo(DebugCommandMessage.Warning, text);
        }

        public void EchoError(string text)
        {
            Echo(DebugCommandMessage.Error, text);
        }

        public void PushExecutioner(IDebugCommandExecutioner executioner)
        {
            _executioners.Push(executioner);
        }

        public void PopExecutioner()
        {
            _executioners.Pop();
        }



        public void Show()
        {
            if (_state == State.Closed)
            {
                _stateTransition = 0.0f;
                _state = State.Opening;
            }
        }

        public void Hide()
        {
            if (_state == State.Opened)
            {
                _stateTransition = 1.0f;
                _state = State.Closing;
            }
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            const float openSpeed = 8.0f;
            const float closeSpeed = 8.0f;

            switch (_state)
            {
                case State.Closed:
                    if (keyState.IsKeyDown(Keys.OemQuotes))
                        Show();
                    break;
                case State.Opening:
                    _stateTransition += dt * openSpeed;
                    if (_stateTransition > 1.0f)
                    {
                        _stateTransition = 1.0f;
                        _state = State.Opened;
                    }
                    break;
                case State.Opened:
                    ProcessKeyInputs(dt);
                    break;
                case State.Closing:
                    _stateTransition -= dt * closeSpeed;
                    if (_stateTransition < 0.0f)
                    {
                        _stateTransition = 0.0f;
                        _state = State.Closed;
                    }
                    break;
            }

            _prevKeyState = keyState;

            base.Update(gameTime);
        }

        public void ProcessKeyInputs(float dt)
        {
            KeyboardState keyState = Keyboard.GetState();
            Keys[] keys = keyState.GetPressedKeys();

            bool shift = keyState.IsKeyDown(Keys.LeftShift) ||
                            keyState.IsKeyDown(Keys.RightShift);

            foreach (Keys key in keys)
            {
                if (!IsKeyPressed(key, dt)) continue;

                char ch;
                if (KeyboardUtils.KeyToString(key, shift, out ch))
                {
                    // Handle typical character input.
                    _commandLine = _commandLine.Insert(_cursorIndex, new string(ch, 1));
                    _cursorIndex++;
                }
                else
                {
                    switch (key)
                    {
                        case Keys.Back:
                            if (_cursorIndex > 0)
                                _commandLine = _commandLine.Remove(--_cursorIndex, 1);
                            break;
                        case Keys.Delete:
                            if (_cursorIndex < _commandLine.Length)
                                _commandLine = _commandLine.Remove(_cursorIndex, 1);
                            break;
                        case Keys.Left:
                            if (_cursorIndex > 0)
                                _cursorIndex--;
                            break;
                        case Keys.Right:
                            if (_cursorIndex < _commandLine.Length)
                                _cursorIndex++;
                            break;
                        case Keys.Enter:
                            // Run the command.
                            ExecuteCommand(_commandLine);
                            _commandLine = string.Empty;
                            _cursorIndex = 0;
                            break;
                        case Keys.Up:
                            // Show command history.
                            if (_commandHistory.Count > 0)
                            {
                                _commandHistoryIndex =
                                    System.Math.Max(0, _commandHistoryIndex - 1);

                                _commandLine = _commandHistory[_commandHistoryIndex];
                                _cursorIndex = _commandLine.Length;
                            }
                            break;
                        case Keys.Down:
                            // Show command history.
                            if (_commandHistory.Count > 0)
                            {
                                _commandHistoryIndex = System.Math.Min(_commandHistory.Count - 1,
                                                                _commandHistoryIndex + 1);
                                _commandLine = _commandHistory[_commandHistoryIndex];
                                _cursorIndex = _commandLine.Length;
                            }
                            break;
                        case Keys.Escape: //OemQuotes
                            Hide();
                            break;
                    }
                }
            }

        }

        bool IsKeyPressed(Keys key, float dt)
        {
            // Treat it as pressed if given key has not pressed in previous frame.
            if (_prevKeyState.IsKeyUp(key))
            {
                _keyRepeatTimer = _keyRepeatStartDuration;
                _pressedKey = key;
                return true;
            }

            // Handling key repeating if given key has pressed in previous frame.
            if (key == _pressedKey)
            {
                _keyRepeatTimer -= dt;
                if (_keyRepeatTimer <= 0.0f)
                {
                    _keyRepeatTimer += _keyRepeatDuration;
                    return true;
                }
            }

            return false;
        }

        public override void Draw(GameTime gameTime)
        {
            // Do nothing when command window is completely closed.
            if (_state == State.Closed)
                return;

            SpriteFont font = Engine.Instance.DefaultSpriteFont;
            Texture2D whiteTexture = _debugManager.WhiteTexture;
            float depth = 0.0f;

            // Compute command window size and draw.
            float w = GraphicsDevice.Viewport.Width;
            float h = GraphicsDevice.Viewport.Height;
            float topMargin = h * 0.1f;
            float leftMargin = w * 0.1f;

            /*Rectangle rect = new Rectangle();
            rect.X = (int)leftMargin;
            rect.Y = (int)topMargin;
            rect.Width = (int)(w * 0.8f);
            rect.Height = (int)(MaxLineCount * font.LineSpacing);*/

            //Todo : add transformation to add transition when closing/opening
            /*Matrix mtx = Matrix.CreateTranslation(
                        new Vector3(0, -rect.Height * (1.0f - stateTransition), 0));*/

            //spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState, mtx);

            //spriteBatch.Draw(whiteTexture, rect, new Color(0, 0, 0, 200));
            _renderer2DComponent.AddSprite2D(
                whiteTexture,
                new Vector2(leftMargin, topMargin), // position
                0.0f,
                new Vector2(w * 0.8f, MaxLineCount * font.LineSpacing), // scale
                _backgroundColor, depth + 0.001f, SpriteEffects.None);

            // Draw each lines.
            Vector2 pos = new Vector2(leftMargin, topMargin);
            foreach (string line in _lines)
            {
                //spriteBatch.DrawString(font, line, pos, Color.White);
                _renderer2DComponent.AddText2D(font, line, pos, 0.0f, Vector2.One, Color.White, depth);
                pos.Y += font.LineSpacing;
            }

            // Draw prompt string.
            string leftPart = Prompt + _commandLine.Substring(0, _cursorIndex);
            Vector2 cursorPos = pos + font.MeasureString(leftPart);
            cursorPos.Y = pos.Y;

            // spriteBatch.DrawString(font,
            //String.Format("{0}{1}", Prompt, commandLine), pos, Color.White);
            _renderer2DComponent.AddText2D(font, String.Format("{0}{1}", Prompt, _commandLine), pos, 0.0f, Vector2.One, Color.White, depth);
            //spriteBatch.DrawString(font, Cursor, cursorPos, Color.White);
            _renderer2DComponent.AddText2D(font, Cursor, cursorPos, 0.0f, Vector2.One, Color.White, depth);

            //spriteBatch.End();
        }


        public void OnResize()
        {

        }

    }
}
