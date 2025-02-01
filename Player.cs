using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

namespace BossRush;

public class Player {
    public int height = 60;
    public int width = 30;
    public Vector2 pos;
    public float yVel = 0;
    public int speed = 6;
    public float jumpHeight = 15f;
    public bool grounded = false;
    public int xDir = 1;
    public int yDir = 0;

    public void MoveY(float movement, float step) {
        grounded = false;
        while (Math.Abs(movement) > step) {
            pos.Y += step*Math.Sign(movement);
            movement -= step*Math.Sign(movement);
            if (Collision(Game1.curroom)) {
                movement += step*Math.Sign(movement);
                pos.Y -= step*Math.Sign(movement);
                yVel = 0;
                grounded = true;
                break;
            }
        }
        pos.Y += movement;
        if (Collision(Game1.curroom)) {
            pos.Y -= movement;
            yVel = 0;
            grounded = true;
        }
    }

    public void MoveX(float movement, float step) {
        while (Math.Abs(movement) > step) {
            pos.X += step*Math.Sign(movement);
            movement -= step*Math.Sign(movement);
            if (Collision(Game1.curroom)) {
                movement += step*Math.Sign(movement);
                pos.X -= step*Math.Sign(movement);
                break;
            }
        }
        pos.X += movement;
        if (Collision(Game1.curroom)) {
            pos.X -= movement;
        }
    }

    public void Shoot(int type) {
        Game1.projectiles.Add(new Projectile{ID = type, xDir = xDir, yDir = yDir, pos = pos});
    }

    public bool Collision(int[,] room) {
        for (int r = 0; r < room.GetLength(1); r++) {
            for (int c = 0; c < room.GetLength(0); c++) {
                switch(room[c, r]) {
                    case 1:
                        if (Functions.RRcollide(pos.X, r*30, pos.Y, c*30, width, 30, height, 30)) {
                            return true;
                        }
                        break;
                }
            }
        }
        return false;
    }
}