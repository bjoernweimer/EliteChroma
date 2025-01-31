﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Colore.Data;
using EliteChroma.Chroma;
using EliteChroma.Elite;
using EliteFiles.Bindings.Binds;
using EliteFiles.Journal;
using static EliteFiles.Journal.Events.StartJump;

namespace EliteChroma.Core.Layers
{
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by ChromaController.InitChromaEffect().")]
    internal sealed partial class HyperspaceLayer : LayerBase
    {
        private static readonly TimeSpan _jumpTunnelThreshold = GameState.JumpCountdownDelay.Subtract(TimeSpan.FromSeconds(1));

        private readonly ParticleField _particleField = new ParticleField();

        private DateTimeOffset? _lastTick;
        private HazardLevel _hazardLevel;
        private IReadOnlyCollection<ParticlePopulation> _population = Array.Empty<ParticlePopulation>();

        private enum HazardLevel
        {
            Low = 0,
            Medium = 1,
            High = 2,
        }

        private enum JumpPhase
        {
            PreJump = 0,
            Tunnel,
            Jump,
        }

        public override int Order => 100;

        protected override bool StartAnimation()
        {
            if (!base.StartAnimation())
            {
                return false;
            }

            if (Game.FsdJumpType == FsdJumpType.Hyperspace)
            {
                _hazardLevel = GetHazardLevel(Game.FsdJumpStarClass);
            }

            _population = Game.FsdJumpType == FsdJumpType.Hyperspace
                ? ParticlePopulation.Hyperspace
                : ParticlePopulation.Supercruise;

            return true;
        }

        protected override void OnRender(ChromaCanvas canvas)
        {
            if (Game.FsdJumpType == FsdJumpType.None)
            {
                _ = StopAnimation();
                _particleField.Clear();
                _lastTick = null;
                return;
            }

            _ = StartAnimation();

            TimeSpan deltaT = Now - (_lastTick ?? Now);
            _lastTick = Now;

            JumpPhase phase = GetJumpPhase();

            foreach (ParticlePopulation p in _population)
            {
                if (p.JumpPhase != phase)
                {
                    continue;
                }

                _particleField.Add(p.SpawnsPerSecond * deltaT.TotalSeconds, p.Color, p.ZSpeedPerSecond);
            }

            _particleField.MoveAndDraw(deltaT, canvas);

            if (!Game.InWitchSpace)
            {
                return;
            }

            RenderHazardLevel(canvas);
        }

        private static HazardLevel GetHazardLevel(string? starClass)
        {
            switch (StarClass.GetKind(starClass, out _))
            {
                case StarClass.Kind.MainSequence:
                    return HazardLevel.Low;

                case StarClass.Kind.Neutron:
                case StarClass.Kind.WhiteDwarf:
                case StarClass.Kind.BlackHole:
                    return HazardLevel.High;

                case StarClass.Kind.Unknown:
                case StarClass.Kind.BrownDwarf:
                case StarClass.Kind.Protostar:
                case StarClass.Kind.Carbon:
                case StarClass.Kind.WolfRayet:
                case StarClass.Kind.Other:
                default:
                    return HazardLevel.Medium;
            }
        }

        private JumpPhase GetJumpPhase()
        {
            TimeSpan tJump = Now - Game.FsdJumpChange;

            if (tJump < _jumpTunnelThreshold)
            {
                return JumpPhase.PreJump;
            }

            if (tJump < GameState.JumpCountdownDelay)
            {
                return JumpPhase.Tunnel;
            }

            return JumpPhase.Jump;
        }

        private void RenderHazardLevel(ChromaCanvas canvas)
        {
            Color hazardColor;
            TimeSpan period;
            PulseColorType pulseType;

            switch (_hazardLevel)
            {
                case HazardLevel.Medium:
                    hazardColor = GameColors.YellowAlert;
                    period = TimeSpan.FromSeconds(1);
                    pulseType = PulseColorType.Square;
                    break;

                case HazardLevel.High:
                    hazardColor = GameColors.RedAlert;
                    period = TimeSpan.FromSeconds(0.67);
                    pulseType = PulseColorType.Square;
                    break;

                case HazardLevel.Low:
                default:
                    hazardColor = GameColors.GreenAlert;
                    period = TimeSpan.FromSeconds(1);
                    pulseType = PulseColorType.Triangle;
                    break;
            }

            Color color = PulseColor(Color.Black, hazardColor, period, pulseType);
            ApplyColorToBinding(canvas.Keyboard, FlightMiscellaneous.HyperSuperCombination, color);
            ApplyColorToBinding(canvas.Keyboard, FlightMiscellaneous.Hyperspace, color);
        }
    }
}
