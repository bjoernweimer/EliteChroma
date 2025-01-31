﻿using System;
using System.Diagnostics.CodeAnalysis;
using Colore.Data;
using Colore.Effects.Keyboard;
using EliteChroma.Chroma;
using EliteFiles.Bindings.Binds;
using EliteFiles.Status;

namespace EliteChroma.Core.Layers
{
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by ChromaController.InitChromaEffect().")]
    internal sealed class LandedLayer : LayerBase
    {
        protected override void OnRender(ChromaCanvas canvas)
        {
            if (!Game.Status.HasFlag(Flags.Landed))
            {
                _ = StopAnimation();
                return;
            }

            _ = StartAnimation();

            CustomKeyboardEffect k = canvas.Keyboard;
            k[Key.Escape] = Colors.InterfaceMode;

            Color c = PulseColor(Color.Black, Colors.VehicleThrust, TimeSpan.FromSeconds(1));
            ApplyColorToBinding(canvas.Keyboard, FlightThrust.Up, c);
        }
    }
}
