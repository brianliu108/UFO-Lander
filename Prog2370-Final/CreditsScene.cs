using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Prog2370_Final
{
    public class CreditsScene : GameScene
    {
        private SpriteBatch spriteBatch;
        private string credits;

        public CreditsScene(Game game, SpriteBatch spriteBatch, string credits) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.credits = credits; 
                                   
        }
    }
}
