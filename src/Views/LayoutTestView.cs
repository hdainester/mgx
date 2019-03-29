using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;
using System;

using Mgx.Control;
using Mgx.Layout;
using Mgx.View;

public class LayoutTestView : FadingView {
    private StackPane spMain;
    private VPane vpList;
    private HPane hpBack;

    public LayoutTestView(ContentManager content, GraphicsDevice graphics)
    : base(content, graphics) {
        ImageItem img1 = new ImageItem(content.Load<Texture2D>("mgx/images/blank"));
        ImageItem img2 = new ImageItem(content.Load<Texture2D>("mgx/images/blank"));
        ImageItem img3 = new ImageItem(content.Load<Texture2D>("mgx/images/blank"));

        img1.HGrow = img1.VGrow = 2;
        img2.HGrow = img2.VGrow = 1;
        img3.HGrow = img3.VGrow = 2;

        img1.Color = Color.Red;
        img2.Color = Color.Yellow;
        img3.Color = Color.Green;

        hpBack = new HPane(img1, img2, img3);
        hpBack.HGrow = hpBack.VGrow = 1;
        
        vpList = new VPane(
            new TextItem(Content.Load<SpriteFont>("mgx/fonts/header"), "Header Font"),
            new TextItem(Content.Load<SpriteFont>("mgx/fonts/header_italic"), "Header (italic) Font"),
            new TextItem(Content.Load<SpriteFont>("mgx/fonts/header_bold"), "Header (bold) Font"),
            new TextItem(Content.Load<SpriteFont>("mgx/fonts/content"), "Content Font"),
            new TextItem(Content.Load<SpriteFont>("mgx/fonts/content_italic"), "Content (italic) Font"),
            new TextItem(Content.Load<SpriteFont>("mgx/fonts/content_bold"), "Content (bold) Font"),
            new TextItem(Content.Load<SpriteFont>("mgx/fonts/footer"), "Footer Font"),
            new TextItem(Content.Load<SpriteFont>("mgx/fonts/footer_italic"), "Footer (italic) Font"),
            new TextItem(Content.Load<SpriteFont>("mgx/fonts/footer_bold"), "Footer (bold) Font")
        );

        vpList.HAlign = HAlignment.Center;
        vpList.VAlign = VAlignment.Top;

        // vpList.HGrow = vpList.VGrow = 1;
        vpList.Children.ToList().ForEach(child => {
            child.HAlign = HAlignment.Center;
            child.VAlign = VAlignment.Center;
        });

        spMain = new StackPane(hpBack, vpList);
        spMain.HAlign = HAlignment.Center;
        spMain.VAlign = VAlignment.Center;
        // spMain.HGrow = 1;

        MainContainer.Add(spMain);
        // MainContainer.Add(vpList);
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        // place game logic etc. here
    }

    public override void HandleInput() {
        HandleKeyboard();
    }

    private KeyboardState prevKeyboard;
    private void HandleKeyboard() {
        KeyboardState keyboard = Keyboard.GetState();

        if(prevKeyboard != null) {
            if(prevKeyboard.IsKeyDown(Keys.Escape)
            && keyboard.IsKeyUp(Keys.Escape)) {
                Close();
            }

            if(prevKeyboard.IsKeyDown(Keys.Up)
            && keyboard.IsKeyUp(Keys.Up)) {
                if(keyboard.IsKeyDown(Keys.LeftShift))
                    spMain.VGrow = Math.Min(1, spMain.VGrow+0.1f);
                else
                    spMain.VAlign = (VAlignment)Math.Max(0, (int)spMain.VAlign-1);
            }
            
            if(prevKeyboard.IsKeyDown(Keys.Right)
            && keyboard.IsKeyUp(Keys.Right)) {
                if(keyboard.IsKeyDown(Keys.LeftShift))
                    spMain.HGrow = Math.Min(1, spMain.HGrow+0.1f);
                else
                    spMain.HAlign = (HAlignment)Math.Min(2, (int)spMain.HAlign+1);
            }

            if(prevKeyboard.IsKeyDown(Keys.Down)
            && keyboard.IsKeyUp(Keys.Down)) {
                if(keyboard.IsKeyDown(Keys.LeftShift))
                    spMain.VGrow = Math.Max(0, spMain.VGrow-0.1f);
                else
                    spMain.VAlign = (VAlignment)Math.Min(2, (int)spMain.VAlign+1);
            }

            if(prevKeyboard.IsKeyDown(Keys.Left)
            && keyboard.IsKeyUp(Keys.Left)) {
                if(keyboard.IsKeyDown(Keys.LeftShift))
                    spMain.HGrow = Math.Max(0, spMain.HGrow-0.1f);
                else
                    spMain.HAlign = (HAlignment)Math.Max(0, (int)spMain.HAlign-1);
            }
        }

        prevKeyboard = keyboard;
    }
}