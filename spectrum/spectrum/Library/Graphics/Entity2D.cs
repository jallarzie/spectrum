using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spectrum.Library.Geometry;
using Spectrum.Library.Collisions;
using Spectrum.Components;

namespace Spectrum.Library.Graphics
{
    public class Entity2D : Drawable, CoordinateSystem
    {
        public Entity2D(Texture2D texture)
            : this(texture, null)
        {
        }

        public Entity2D(Texture2D texture, CoordinateSystem parent)
        {
            SourceRectangle = null;

            Dirty = false;
            Texture = texture;

            Parent = parent;
            BoundingArea = new Sphere(new Vector2(0, 0), 1);
            Position = new Vector2(0, 0);
            Rotation = 0f;
            Scale = 1f;
            Tint = Color.White;

            Origin = new Vector2(0, 0);
            Layer = 0f;
            Opacity = 1f;
            Flip = SpriteEffects.None;
        }

        public int Width
        {
            get
            {
                if (Dirty) this.Update();
                return Texture.Width;
            }
        }

        public int Height
        {
            get
            {
                if (Dirty) this.Update();
                return Texture.Height;
            }
        }

        public virtual void Update()
        {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch targetSpriteBatch)
        {
            if (Dirty) this.Update();
            targetSpriteBatch.Draw(Texture, this.WorldPosition(), SourceRectangle, Tint * Opacity, this.WorldRotation(), Origin, Scale, Flip, Layer);
        }

        protected void SetTint(Color color)
        {
            if (color == Color.Red ||
                color == Color.Lime ||
                color == Color.Blue ||
                color == Color.Cyan ||
                color == Color.Magenta ||
                color == Color.Yellow ||
                color == Color.White)
            {
                Tint = color;
            }
            else
            {
                Tint = Color.DimGray; // best choice, others are too dark or look too much like the other colors
            }
        }

        public void AbsorbTint(Color color)
        {
            Color combined = Color.Black;
            // use ^ so absorbed colors are lost if already present
            // use || so absorbed colors are always contained in new color
            if (color.R > 200 || Tint.R > 200) combined.R = 255;
            if (color.G > 200 || Tint.G > 200) combined.G = 255;
            if (color.B > 200 || Tint.B > 200) combined.B = 255;
            SetTint(combined);
        }

        public void LoseTint(Color color)
        {
            Color combined = Color.Black;

            if (color.R <= 200 && Tint.R > 200) 
            {
                combined.R = 255;
            }

            if (color.G <= 200 && Tint.G > 200) 
            {
                combined.G = 255;
            }

            if (color.B <= 200 && Tint.B > 200) 
            {
                combined.B = 255;
            }

            Color oldTint = Tint;
            SetTint(combined);

            if (oldTint != Tint) 
            {
                SoundPlayer.PlayPlayerLooseColorSound();
            }
        }

        protected Texture2D Texture;
        protected bool Dirty;

        public CoordinateSystem Parent { get; set; }
        private Vector2 position;
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                BoundingArea.Shape.Center = value;
            }
        }

        public float GetHealthRatio()
        {
            return CurrentHealthPoints / (float)MaxHealthPoints;
        }

        public bool IsAlive()
        {
            return CurrentHealthPoints > 0;
        }

        public void ProcessHit(Laser laser)
        {
            Color oldTint = Tint;
            LoseTint(laser.Tint);
            if (oldTint == Tint)
            {
                int damage = laser.Damage;

                // Red effect: laser does 1.5x more damage
                if (laser.Tint.R > 200) damage = damage * 3 / 2;

                // Green effect: laser does 0.5x extra damage per second for MAX_POISON_TIME extra seconds
                if (laser.Tint.G > 200)
                {
                    CurrentPoisonTime = MAX_POISON_TIME;
                    IsPoisoned = true;
                    if (damage >= PoisonDamage * 2)
                    {
                        PoisonDamage = damage / 2;
                    }
                }

                // Blue effect: slows the entity down for MAX_SLOW_TIME seconds
                if (laser.Tint.B > 200)
                {
                    CurrentSlowTime = MAX_SLOW_TIME;
                    IsSlowed = true;
                }

                CurrentHealthPoints -= damage;
                if (CurrentHealthPoints < 0) CurrentHealthPoints = 0;
            }
        }

        // IN PROGRESS
        public void UpdateStatusEffects(GameTime gameTime)
        {
            if (IsPoisoned)
            {
                PoisonRateCounter -= gameTime.ElapsedGameTime.Seconds;
                CurrentPoisonTime -= gameTime.ElapsedGameTime.Seconds;
                if (PoisonRateCounter < 0)
                {
                    CurrentHealthPoints -= PoisonDamage;
                    if (CurrentHealthPoints < 0) CurrentHealthPoints = 0;
                    PoisonRateCounter += POISON_RATE;
                }
                if (CurrentPoisonTime < 0)
                {
                    IsPoisoned = false;
                    PoisonRateCounter = CurrentPoisonTime = PoisonDamage = 0;
                }
            }
            if (IsSlowed)
            {
                CurrentSlowTime -= gameTime.ElapsedGameTime.Seconds;
                if (CurrentSlowTime < 0)
                {
                    IsSlowed = false;
                    CurrentSlowTime = 0;
                }
            }
        }

        public float Rotation { get; set; }
        public float Scale { get; set; }
        public Rectangle? SourceRectangle { get; set; }
        public Color Tint { get; set; }
        public Area BoundingArea { get; protected set; }

        public Vector2 Origin;
        public float Layer;
        public float Opacity;
        public SpriteEffects Flip;

        public int CurrentHealthPoints;
        public int MaxHealthPoints;
        public bool IsPoisoned, IsSlowed;

        private float CurrentPoisonTime, PoisonRateCounter, CurrentSlowTime;
        private int PoisonDamage;
        private static float MAX_POISON_TIME = 3; // in seconds
        private static float POISON_RATE = 0.5f; // in seconds
        private static float MAX_SLOW_TIME = 3; // in seconds
        public static float SLOW_SPEED_MULTIPLIER = 0.5f;
    }
}
