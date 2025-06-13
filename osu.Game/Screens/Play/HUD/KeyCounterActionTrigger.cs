// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;

namespace osu.Game.Screens.Play.HUD
{
    public partial class KeyCounterActionTrigger<T> : InputTrigger, IKeyBindingHandler<T>
        where T : struct
    {
        public T Action { get; }

        /// <summary>
        /// The inputs used to activate this trigger.
        /// </summary>
        public KeyCombination KeyCombination { get; }

        private KeyCombination lastKeyCombination;

        public KeyCounterActionTrigger(T action, KeyCombination keyCombination, string? name = null)
            : base(name ?? $"B{(int)(object)action + 1}")
        {
            Action = action;
            KeyCombination = keyCombination;
            lastKeyCombination = new KeyCombination(new[] { InputKey.None });
        }

        public bool OnPressed(KeyBindingPressEvent<T> e)
        {
            if (!EqualityComparer<T>.Default.Equals(e.Action, Action))
                return false;

            KeyCombination currentKeyCombination = KeyCombination.FromInputState(e.CurrentState);
            InputKey[] keys = getKeysChanged(currentKeyCombination);
            lastKeyCombination = currentKeyCombination;

            if (keys.Length == 0)
                return false;

            Activate(keys, Clock.Rate >= 0);
            return false;
        }

        public void OnReleased(KeyBindingReleaseEvent<T> e)
        {
            if (!EqualityComparer<T>.Default.Equals(e.Action, Action))
                return;

            KeyCombination currentKeyCombination = KeyCombination.FromInputState(e.CurrentState);
            InputKey[] keys = getKeysChanged(currentKeyCombination);
            lastKeyCombination = currentKeyCombination;

            if (keys.Length == 0)
                return;

            Deactivate(keys, Clock.Rate >= 0);
        }

        private InputKey[] getKeysChanged(KeyCombination currentKeyCombination)
        {
            return currentKeyCombination.Keys
                                        .Except(lastKeyCombination.Keys)
                                        .Concat(lastKeyCombination.Keys.Except(currentKeyCombination.Keys))
                                        .Where(x => KeyCombination.Keys.Contains(x))
                                        .ToArray();
        }
    }
}
