﻿// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Game.Graphics;
using System;

namespace osu.Game.Rulesets.Mods
{
    /// <summary>
    /// The base class for gameplay modifiers.
    /// </summary>
    public abstract class Mod
    {
        /// <summary>
        /// The name of this mod.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The icon of this mod.
        /// </summary>
        public virtual FontAwesome Icon => FontAwesome.fa_question;

        /// <summary>
        /// The type of this mod.
        /// </summary>
        public virtual ModType Type => ModType.Special;

        /// <summary>
        /// The user readable description of this mod.
        /// </summary>
        public virtual string Description => string.Empty;

        /// <summary>
        /// The score multiplier of this mod.
        /// </summary>
        public abstract double ScoreMultiplier { get; }

        /// <summary>
        /// Returns if this mod is ranked.
        /// </summary>
        public virtual bool Ranked => false;

        /// <summary>
        /// The mods this mod cannot be enabled with.
        /// </summary>
        public virtual Type[] IncompatibleMods => new Type[] { };

        /// <summary>
        /// Whether we should allow fails at the 
        /// </summary>
        public virtual bool AllowFail => true;
    }
}
