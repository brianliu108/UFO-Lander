using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Prog2370_Final.Drawable {
    public class KbInputString : SimpleString {
        private KeyboardState oldState;
        public bool locked = false;

        public KbInputString(Game game, SpriteBatch spriteBatch,
            SpriteFont spriteFont, Vector2 position, Color color,
            TextAlignH horizontalAlignment = TextAlignH.Left, TextAlignV verticalAlignment = TextAlignV.Top) :
            base(game, spriteBatch, spriteFont, position, "", color, horizontalAlignment, verticalAlignment) { }

        public override void Update(GameTime gameTime) {
            KeyboardState ks = Keyboard.GetState();
            if (!locked) {
                if (ks.IsKeyDown(Keys.A) && !oldState.IsKeyDown(Keys.A)) Message += "A";
                if (ks.IsKeyDown(Keys.B) && !oldState.IsKeyDown(Keys.B)) Message += "B";
                if (ks.IsKeyDown(Keys.C) && !oldState.IsKeyDown(Keys.C)) Message += "C";
                if (ks.IsKeyDown(Keys.D) && !oldState.IsKeyDown(Keys.D)) Message += "D";
                if (ks.IsKeyDown(Keys.E) && !oldState.IsKeyDown(Keys.E)) Message += "E";
                if (ks.IsKeyDown(Keys.F) && !oldState.IsKeyDown(Keys.F)) Message += "F";
                if (ks.IsKeyDown(Keys.G) && !oldState.IsKeyDown(Keys.G)) Message += "G";
                if (ks.IsKeyDown(Keys.H) && !oldState.IsKeyDown(Keys.H)) Message += "H";
                if (ks.IsKeyDown(Keys.I) && !oldState.IsKeyDown(Keys.I)) Message += "I";
                if (ks.IsKeyDown(Keys.J) && !oldState.IsKeyDown(Keys.J)) Message += "J";
                if (ks.IsKeyDown(Keys.K) && !oldState.IsKeyDown(Keys.K)) Message += "K";
                if (ks.IsKeyDown(Keys.L) && !oldState.IsKeyDown(Keys.L)) Message += "L";
                if (ks.IsKeyDown(Keys.M) && !oldState.IsKeyDown(Keys.M)) Message += "M";
                if (ks.IsKeyDown(Keys.N) && !oldState.IsKeyDown(Keys.N)) Message += "N";
                if (ks.IsKeyDown(Keys.O) && !oldState.IsKeyDown(Keys.O)) Message += "O";
                if (ks.IsKeyDown(Keys.P) && !oldState.IsKeyDown(Keys.P)) Message += "P";
                if (ks.IsKeyDown(Keys.Q) && !oldState.IsKeyDown(Keys.Q)) Message += "Q";
                if (ks.IsKeyDown(Keys.R) && !oldState.IsKeyDown(Keys.R)) Message += "R";
                if (ks.IsKeyDown(Keys.S) && !oldState.IsKeyDown(Keys.S)) Message += "S";
                if (ks.IsKeyDown(Keys.T) && !oldState.IsKeyDown(Keys.T)) Message += "T";
                if (ks.IsKeyDown(Keys.U) && !oldState.IsKeyDown(Keys.U)) Message += "U";
                if (ks.IsKeyDown(Keys.V) && !oldState.IsKeyDown(Keys.V)) Message += "V";
                if (ks.IsKeyDown(Keys.W) && !oldState.IsKeyDown(Keys.W)) Message += "W";
                if (ks.IsKeyDown(Keys.X) && !oldState.IsKeyDown(Keys.X)) Message += "X";
                if (ks.IsKeyDown(Keys.Y) && !oldState.IsKeyDown(Keys.Y)) Message += "Y";
                if (ks.IsKeyDown(Keys.Z) && !oldState.IsKeyDown(Keys.Z)) Message += "Z";
                if (ks.IsKeyDown(Keys.Back) && !oldState.IsKeyDown(Keys.Back) && Message.Length > 0)
                    Message = Message.Substring(0, Message.Length - 1);
            }

            oldState = ks;
            base.Update(gameTime);
        }
    }
}