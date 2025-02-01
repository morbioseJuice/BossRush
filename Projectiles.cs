using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

namespace BossRush;

public class Projectile {
    public int height = 5;
    public int width = 5;
    public Vector2 pos;
    public int speed = 15;
    public int ID;
    public int xDir;
    public int yDir;

    public void Update() {
        if (ID == 0) {

            if (xDir == -1) {
                pos.X -= speed;
            }
            else if (xDir == 1) {
                pos.X += speed;
            }
            if (yDir == -1) {
                pos.Y -= speed;
            }
            else if (xDir == 1) {
                pos.Y += speed;
            }
        }
    }
}
