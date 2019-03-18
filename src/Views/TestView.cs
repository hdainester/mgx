using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;

using Mgx.View;
using Mgx.Layout;
using Mgx.Control;

public class TestView : FadingView {
    private Dictionary<string, SpriteFont> mgxFonts;
    
    public TestView(ContentManager content, GraphicsDevice graphics)
    : base(content, graphics) {
        ImageItem img = new ImageItem(content.Load<Texture2D>("mgx/images/blank"));
        img.HGrow = img.VGrow = 1;
        img.Color = Color.Red;

        StackPane sp = new StackPane();
        sp.HAlign = HAlignment.Center;
        sp.VAlign = VAlignment.Center;

        VPane vpList = new VPane();
        vpList.Add(new TextItem(Content.Load<SpriteFont>("mgx/fonts/header"), "Header Font"));
        vpList.Add(new TextItem(Content.Load<SpriteFont>("mgx/fonts/header_italic"), "Header (italic) Font"));
        vpList.Add(new TextItem(Content.Load<SpriteFont>("mgx/fonts/header_bold"), "Header (bold) Font"));
        vpList.Add(new TextItem(Content.Load<SpriteFont>("mgx/fonts/content"), "Content Font"));
        vpList.Add(new TextItem(Content.Load<SpriteFont>("mgx/fonts/content_italic"), "Content (italic) Font"));
        vpList.Add(new TextItem(Content.Load<SpriteFont>("mgx/fonts/content_bold"), "Content (bold) Font"));
        vpList.Add(new TextItem(Content.Load<SpriteFont>("mgx/fonts/footer"), "Footer Font"));
        vpList.Add(new TextItem(Content.Load<SpriteFont>("mgx/fonts/footer_italic"), "Footer (italic) Font"));
        vpList.Add(new TextItem(Content.Load<SpriteFont>("mgx/fonts/footer_bold"), "Footer (bold) Font"));

        // vpList.HGrow = vpList.VGrow = 1;
        vpList.Children.ToList().ForEach(child => {
            child.HAlign = HAlignment.Center;
            child.VAlign = VAlignment.Center;
        });

        vpList.HAlign = HAlignment.Center;
        vpList.VAlign = VAlignment.Top;
        sp.HGrow = 1;

        sp.Add(img);
        sp.Add(vpList);
        MainContainer.Add(sp);
        // MainContainer.Add(vpList);
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
    }
}