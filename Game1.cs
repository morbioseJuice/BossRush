using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

namespace BossRush;

public class Game1 : Game
{
    private RenderTarget2D _renderTarget;
    private const int VirtualWidth = 960;
    private const int VirtualHeight = 540;
    private int _windowWidth;
    private int _windowHeight;

    // Rooms rooms = new Rooms();

    public static int[,] curroom = Rooms.room1;

    // public Projectile[] projectiles;
    public static List<Projectile> projectiles = new List<Projectile>();

    Texture2D philTexture;
    float gravity = 0.8f;

    Player p1 = new Player();

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        // _graphics.IsFullScreen = true;
        // Window.AllowUserResizing = true;
        _graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        Window.ClientSizeChanged += OnWindowResized;
    }
    
    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
    
        _graphics.IsFullScreen = true;
        _windowWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        _windowHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        _graphics.PreferredBackBufferWidth = _windowWidth;
        _graphics.PreferredBackBufferHeight = _windowHeight;
        _graphics.ApplyChanges();

        // Create a render target with the virtual resolution
        _renderTarget = new RenderTarget2D(GraphicsDevice, VirtualWidth, VirtualHeight);

        p1.pos = new Vector2(VirtualWidth/2, VirtualHeight/2);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        philTexture = Content.Load<Texture2D>("Phil2");
    }

    protected override void Update(GameTime gameTime)
    {
        var g1State = GamePad.GetState(PlayerIndex.One); // Gets the state of the controller (Player 1)
        var kState = Keyboard.GetState(); // Gets the state of the keyboard
        if (g1State.Buttons.Back == ButtonState.Pressed || kState.IsKeyDown(Keys.Escape)) {
            Exit(); // Closes the game
        }
        if (g1State.ThumbSticks.Left.X < -0.1 || kState.IsKeyDown(Keys.A)) {
            // p1.pos.X -= p1.speed; // Moves the player left
            p1.xDir = -1;
            p1.MoveX(-p1.speed, 1);
        } else if (g1State.ThumbSticks.Left.X > 0.1 || kState.IsKeyDown(Keys.D)) {
            // p1.pos.X += p1.speed; // Moves the player right
            p1.xDir = 1;
            p1.MoveX(p1.speed, 1);
        }
        if (g1State.ThumbSticks.Left.Y < -0.1 || kState.IsKeyDown(Keys.W)) {
            //up
            p1.yDir = -1;
        } else if (g1State.ThumbSticks.Left.Y > 0.1 || kState.IsKeyDown(Keys.S)) {
            //up
            p1.yDir = 1;
        } else {
            p1.yDir = 0;
        }
        if (g1State.Buttons.A == ButtonState.Pressed || kState.IsKeyDown(Keys.Space) && p1.grounded) {
            p1.yVel = -p1.jumpHeight;
        }

        if (g1State.Buttons.X == ButtonState.Pressed || kState.IsKeyDown(Keys.J)) {
            p1.Shoot(0);
        }

        for (int i = 0; i < projectiles.Count; i++) {
            projectiles[i].Update();
        }

        // if (!p1.grounded) {
        //     p1.MoveY(p1.yVel, 15);
        //     p1.yVel -= gravity;
        // }
        if (!p1.grounded) {
            p1.yVel += gravity;
        }
        p1.MoveY(p1.yVel, 1);

        // while((float)gameTime.ElapsedGameTime.TotalMilliseconds<20);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(_renderTarget);
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        // This allows us to make colored squares
        Texture2D pixel = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1);
        pixel.SetData(new[] {Color.White});

        _spriteBatch.Begin();

        // Draws the player. Convert.ToInt32 is used because rectangles can't use floats.
        
        _spriteBatch.Draw(pixel, new Rectangle(Convert.ToInt32(p1.pos.X), Convert.ToInt32(p1.pos.Y), p1.width, p1.height), Color.Blue);
        
        for (int i = 0; i < projectiles.Count; i++) {
            _spriteBatch.Draw(pixel, new Rectangle(Convert.ToInt32(projectiles[i].pos.X), Convert.ToInt32(projectiles[i].pos.Y), projectiles[i].width, projectiles[i].height), Color.Blue);
        }
        
        //_spriteBatch.Draw(philTexture, new Rectangle(Convert.ToInt32(p1.pos.X), Convert.ToInt32(p1.pos.Y), p1.width, p1.height), Color.White);

        // Renders the room
        for (int i = 0; i < curroom.GetLength(1); i++) {
            for (int j = 0; j < curroom.GetLength(0); j++) {
                if (curroom[j, i] == 1) {
                    _spriteBatch.Draw(pixel, new Rectangle(i*30, j*30, 30, 30), Color.Black);
                }
            }
        }

        _spriteBatch.End();

        GraphicsDevice.SetRenderTarget(null);

        // Calculate the scale factor
        float scaleX = (float)_windowWidth / VirtualWidth;
        float scaleY = (float)_windowHeight / VirtualHeight;
        float scale = Math.Min(scaleX, scaleY);

        // Calculate the position to center the game screen
        int offsetX = (int)((_windowWidth - VirtualWidth * scale) / 2);
        int offsetY = (int)((_windowHeight - VirtualHeight * scale) / 2);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_renderTarget, new Rectangle(offsetX, offsetY, (int)(VirtualWidth * scale), (int)(VirtualHeight * scale)), Color.White);
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }

    private void OnWindowResized(object sender, System.EventArgs e)
    {
        _windowWidth = Window.ClientBounds.Width;
        _windowHeight = Window.ClientBounds.Height;
    }
}
