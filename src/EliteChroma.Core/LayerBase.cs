﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Colore.Data;
using Colore.Effects.Keyboard;
using EliteChroma.Chroma;
using EliteChroma.Core.Internal;
using EliteChroma.Elite;
using EliteFiles.Bindings;
using EliteFiles.Bindings.Devices;

namespace EliteChroma.Core
{
    public abstract class LayerBase : EffectLayer
    {
        private GameState? _game;
        private ChromaColors? _colors;

        [SuppressMessage("Design", "CA1027:Mark enums with FlagsAttribute", Justification = "No flags")]
        protected enum PulseColorType
        {
            Triangle = 0,
            Square = 1,
            Sawtooth = 2,

            Default = Triangle,
        }

        public bool Animated { get; private set; }

        internal INativeMethods NativeMethods { get; set; } = Internal.NativeMethods.Instance;

        protected GameState Game => _game ?? throw new InvalidOperationException();

        protected ChromaColors Colors => _colors ?? throw new InvalidOperationException();

        protected DateTimeOffset Now => Game.Now;

        protected DateTimeOffset AnimationStart { get; private set; }

        protected TimeSpan AnimationElapsed => Now - AnimationStart;

        protected override void OnRender(ChromaCanvas canvas, object state)
        {
            LayerRenderState rs = (LayerRenderState)state ?? throw new ArgumentNullException(nameof(state));
            _game = rs.GameState;
            _colors = rs.Colors;

            try
            {
                OnRender(canvas);
            }
            finally
            {
                _game = null;
                _colors = null;
            }
        }

        protected abstract void OnRender(ChromaCanvas canvas);

        protected virtual bool StartAnimation()
        {
            if (Animated)
            {
                return false;
            }

            Animated = true;
            AnimationStart = Now;
            return true;
        }

        protected virtual bool StopAnimation()
        {
            if (!Animated)
            {
                return false;
            }

            Animated = false;
            return true;
        }

        protected Color PulseColor(Color c1, Color c2, TimeSpan period, PulseColorType pulseType = PulseColorType.Triangle, double offsetPct = 0)
        {
            double max = period.TotalSeconds;
            double offset = max * (offsetPct - Math.Floor(offsetPct));
            double t = ((AnimationElapsed.TotalSeconds + offset) % max) / max;

            double x = pulseType switch
            {
                PulseColorType.Triangle => t < 0.5 ? t * 2 : (1 - t) * 2,
                PulseColorType.Square => t < 0.5 ? 1 : 0,
                PulseColorType.Sawtooth => t,
                _ => throw new ArgumentOutOfRangeException(nameof(pulseType)),
            };

            double r = (c1.R / 255.0 * (1 - x)) + (c2.R / 255.0 * x);
            double g = (c1.G / 255.0 * (1 - x)) + (c2.G / 255.0 * x);
            double b = (c1.B / 255.0 * (1 - x)) + (c2.B / 255.0 * x);

            return new Color(r, g, b);
        }

        protected void ApplyColorToBinding(CustomKeyboardEffect grid, IEnumerable<string> bindingNames, Color color)
        {
            if (bindingNames == null)
            {
                throw new ArgumentNullException(nameof(bindingNames));
            }

            foreach (string bindingName in bindingNames)
            {
                ApplyColorToBinding(grid, bindingName, color);
            }
        }

        protected void ApplyColorToBinding(CustomKeyboardEffect grid, string bindingName, Color color)
        {
            BindingPreset? bindingPreset = Game.BindingPreset;

            if (bindingPreset == null)
            {
                return;
            }

            if (!bindingPreset.Bindings.TryGetValue(bindingName, out Binding binding))
            {
                return;
            }

            foreach (DeviceKeyCombination bps in new[] { binding.Primary, binding.Secondary })
            {
                if (bps.Device != Device.Keyboard || bps.Key == null)
                {
                    continue;
                }

                if (!KeyMappings.TryGetKey(bps.Key, bindingPreset.KeyboardLayout, Game.ForceEnUSKeyboardLayout, out Key key, NativeMethods))
                {
                    continue;
                }

                if (key == 0)
                {
                    continue;
                }

                if (!bps.Modifiers.Equals(Game.PressedModifiers))
                {
                    continue;
                }

                grid[key] = color;

                color = color.Transform(Colors.SecondaryBindingBrightness);
            }
        }
    }
}
