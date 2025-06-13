// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Bindings;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Screens.Play.HUD;

namespace osu.Game.Skinning
{
    public partial class LegacyKeyCounter : KeyCounter
    {
        private static readonly Colour4 active_colour_top = Colour4.FromHex(@"#ffde00");
        private static readonly Colour4 active_colour_bottom = Colour4.FromHex(@"#f8009e");

        private const float transition_duration = 160;

        private Colour4 textColour;

        public Colour4 TextColour
        {
            get => textColour;
            set
            {
                textColour = value;
                overlayKeyText.Colour = value;
            }
        }

        private readonly Container keyContainer;
        private readonly OsuSpriteText overlayKeyText;
        private readonly Sprite keySprite;

        public LegacyKeyCounter(InputTrigger trigger)
            : base(trigger)
        {
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;
            Child = keyContainer = new Container
            {
                AutoSizeAxes = Axes.Both,
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Children = new Drawable[]
                {
                    keySprite = new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    },
                    new UprightAspectMaintainingContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Child = overlayKeyText = new OsuSpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Text = trigger.Name,
                            Colour = textColour,
                            Font = OsuFont.GetFont(weight: FontWeight.SemiBold),
                        },
                    },
                }
            };

            // matches longest dimension of default skin asset
            Height = Width = 46;
        }

        [BackgroundDependencyLoader]
        private void load(ISkinSource source)
        {
            Texture? keyTexture = source.GetTexture(@"inputoverlay-key");

            if (keyTexture != null)
                keySprite.Texture = keyTexture;
        }

        protected override void Activate(InputKey[] inputKeys, bool forwardPlayback = true)
        {
            base.Activate(inputKeys, forwardPlayback);

            // 132 through 148 are mouse inputs, simply let's cover all of them.
            bool isMouseInput = inputKeys.Any(inputKey => (int)inputKey > 131 && (int)inputKey < 149);

            keyContainer.ScaleTo(0.75f, transition_duration, Easing.Out);
            keySprite.Colour = isMouseInput ? active_colour_bottom : active_colour_top;
            overlayKeyText.Text = CountPresses.Value.ToString();
            overlayKeyText.Font = overlayKeyText.Font.With(weight: FontWeight.SemiBold);
        }

        protected override void Deactivate(InputKey[] inputKeys, bool forwardPlayback = true)
        {
            base.Deactivate(inputKeys, forwardPlayback);
            keyContainer.ScaleTo(1f, transition_duration, Easing.Out);
            keySprite.Colour = Colour4.White;
        }
    }
}
