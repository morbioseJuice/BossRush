using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

namespace BossRush;

public class Functions {
    public static bool RRcollide(float x1, float x2, float y1, float y2, float w1, float w2, float h1, float h2) { //rect-rect colliding
        return x1 + w1>= x2 && x1<= x2 + w2 && y1 + h1>= y2 && y1<= y2 + h2;
    }
}