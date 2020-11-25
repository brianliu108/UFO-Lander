using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Prog2370_Final {
    public class CollisionManager : GameComponent {
        private Terrain terrain;
        private UFO ufo;
        private SoundEffect crash;
        private SoundEffect land;
        private SoundEffect gas;


        public CollisionManager(Game game,
            Terrain terrain,
            UFO ufo,
            SoundEffect crash,
            SoundEffect land,
            SoundEffect gas) : base(game) { }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }
    }
}