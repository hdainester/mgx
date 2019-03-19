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
        ImageItem img1 = new ImageItem(content.Load<Texture2D>("mgx/images/blank"));
        ImageItem img2 = new ImageItem(content.Load<Texture2D>("mgx/images/blank"));
        ImageItem img3 = new ImageItem(content.Load<Texture2D>("mgx/images/blank"));

        img1.HGrow = img1.VGrow = 2;
        img2.HGrow = img2.VGrow = 1;
        img3.HGrow = img3.VGrow = 2;

        img1.Color = Color.Red;
        img2.Color = Color.Yellow;
        img3.Color = Color.Green;

        HPane hpBack = new HPane(img1, img2, img3);
        hpBack.HGrow = hpBack.VGrow = 1;
        
        VPane vpList = new VPane(
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

        StackPane sp = new StackPane(hpBack, vpList);
        sp.HAlign = HAlignment.Center;
        sp.VAlign = VAlignment.Center;
        // sp.HGrow = 1;

        MainContainer.Add(sp);
        // MainContainer.Add(vpList);
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
    }
}