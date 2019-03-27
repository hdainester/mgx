using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using Mgx.Control.Menu;
using Mgx.Control;
using Mgx.Layout;
using Mgx.View;

using System.Linq;
using System;

public class MenuTestView : FadingView {
    private SpriteFont font_c0;
    private SpriteFont font_c1;
    private SpriteFont font_f0;
    private SpriteFont font_f1;
    private Texture2D blank;

    public MenuTestView(ContentManager content, GraphicsDevice graphics)
    : base(content, graphics) {
        font_c0 = Content.Load<SpriteFont>("mgx/fonts/content");
        font_c1 = Content.Load<SpriteFont>("mgx/fonts/content_italic");
        font_f0 = Content.Load<SpriteFont>("mgx/fonts/footer");
        font_f1 = Content.Load<SpriteFont>("mgx/fonts/footer_italic");
        blank = Content.Load<Texture2D>("mgx/images/blank");

        ImageItem back0 = new ImageItem(blank);
        ImageItem back1 = new ImageItem(blank);
        ImageItem back2 = new ImageItem(blank);
        ImageItem back3 = new ImageItem(blank);
        back0.HGrow = back1.HGrow = back2.HGrow = back3.HGrow = 1;
        back0.VGrow = back1.VGrow = back2.VGrow = back3.VGrow = 1;
        back0.Alpha = back1.Alpha = back2.Alpha = back3.Alpha = 0.5f;
        back0.Color = Color.Blue;
        back1.Color = Color.LightGray;
        back2.Color = Color.Gray;
        back3.Color = Color.Black;

        ListMenu listMenu0 = new ListMenu();
        listMenu0.HAlign = HAlignment.Center;
        listMenu0.VAlign = VAlignment.Center;

        MenuItem item00 = new MenuItem("Item0", font_f0, Content.Load<Texture2D>("mgx/images/icons/keyboard"), 64, 64);
        MenuItem item01 = new MenuItem("Item1", font_f0, Content.Load<Texture2D>("mgx/images/icons/gamepad"), 64, 64);
        MenuItem item02 = new MenuItem("Item2", font_f0, Content.Load<Texture2D>("mgx/images/icons/touch"), 64, 64);

        item00.FocusGain += (sender, args) => item00.Text.Color = Color.Yellow;
        item01.FocusGain += (sender, args) => item01.Text.Color = Color.Yellow;
        item02.FocusGain += (sender, args) => item02.Text.Color = Color.Yellow;

        item00.FocusLoss += (sender, args) => item00.Text.Color = Color.White;
        item01.FocusLoss += (sender, args) => item01.Text.Color = Color.White;
        item02.FocusLoss += (sender, args) => item02.Text.Color = Color.White;

        item00.Action += (sender, args) => item00.Orientation = (Orientation)((((int)item00.Orientation)+1) % 4);
        item01.Action += (sender, args) => item01.Orientation = (Orientation)((((int)item01.Orientation)+1) % 4);
        item02.Action += (sender, args) => item02.Orientation = (Orientation)((((int)item02.Orientation)+1) % 4);

        listMenu0.AddItem(item00);
        listMenu0.AddItem(item01);
        listMenu0.AddItem(item02);
        listMenu0.KeyReleased += (sender, args) => {
            if(args.Key == Keys.R)
                listMenu0.ItemsOrientation = (Orientation)((((int)listMenu0.ItemsOrientation)+1) % 4);

            if(args.Key == Keys.A)
                listMenu0.HAlign = (HAlignment)(Math.Max(0, ((int)listMenu0.HAlign)-1));

            if(args.Key == Keys.D)
                listMenu0.HAlign = (HAlignment)(Math.Min(2, ((int)listMenu0.HAlign)+1));

            if(args.Key == Keys.W)
                listMenu0.VAlign = (VAlignment)(Math.Max(0, ((int)listMenu0.VAlign)-1));

            if(args.Key == Keys.S)
                listMenu0.VAlign = (VAlignment)(Math.Min(2, ((int)listMenu0.VAlign)+1));

            if(args.Key == Keys.Escape)
                Close();
        };

        ListMenu listMenu1 = new ListMenu();
        listMenu1.ItemsOrientation = Orientation.Vertical;
        listMenu1.HAlign = HAlignment.Center;
        listMenu1.VAlign = VAlignment.Center;

        MenuItem item10 = new MenuItem("Enabled", font_c0);
        MenuItem item11 = new MenuItem("Enabled", font_c0);
        MenuItem item12 = new MenuItem("Enabled", font_c0);

        item10.FocusGain += (sender, args) => item10.Text.Color = Color.Yellow;
        item11.FocusGain += (sender, args) => item11.Text.Color = Color.Yellow;
        item12.FocusGain += (sender, args) => item12.Text.Color = Color.Yellow;

        item10.FocusLoss += (sender, args) => item10.Text.Color = Color.White;
        item11.FocusLoss += (sender, args) => item11.Text.Color = Color.White;
        item12.FocusLoss += (sender, args) => item12.Text.Color = Color.White;

        item10.Action += (sender, args) => {
            item00.IsDisabled = !item00.IsDisabled;
            item10.Text.Text = item00.IsDisabled ? "Disabled" : "Enabled";
        };

        item11.Action += (sender, args) => {
            item01.IsDisabled = !item01.IsDisabled;
            item11.Text.Text = item01.IsDisabled ? "Disabled" : "Enabled";
        };
        
        item12.Action += (sender, args) => {
            item02.IsDisabled = !item02.IsDisabled;
            item12.Text.Text = item02.IsDisabled ? "Disabled" : "Enabled";
        };

        listMenu1.AddItem(item10);
        listMenu1.AddItem(item11);
        listMenu1.AddItem(item12);


        MenuItem itemtb0 = new MenuItem(Content.Load<Texture2D>("mgx/images/icons/entry-door"), 64, 64);
        MenuItem itemtb1 = new MenuItem(Content.Load<Texture2D>("mgx/images/icons/exit-door"), 64, 64);
        itemtb0.HAlign = HAlignment.Left;
        itemtb1.HAlign = HAlignment.Right;

        HPane listMenuTb = new HPane();
        listMenuTb.HAlign = HAlignment.Center;
        listMenuTb.VAlign = VAlignment.Center;
        listMenuTb.HGrow = 0.8f;
        listMenuTb.Add(itemtb0);
        listMenuTb.Add(itemtb1);

        VPane vpInfo = new VPane(
            new HPane(
                new TextItem(font_f0, "Toggle Orientation:"),
                new TextItem(font_f1, "r")
            ),
            new HPane(
                new TextItem(font_f0, "Change Alignment:"),
                new TextItem(font_f1, "wasd")
            ),
            new HPane(
                new TextItem(font_f0, "Traverse items:"),
                new TextItem(font_f1, "arrow-keys")
            ),
            new HPane(
                new TextItem(font_f0, "Activate:"),
                new TextItem(font_f1, "enter/left mouse")
            ),
            new HPane(
                new TextItem(font_f0, "Quit:"),
                new TextItem(font_f1, "esc")
            )
        );

        vpInfo.HGrow = 1;
        vpInfo.VAlign = VAlignment.Center;
        vpInfo.Containers.ToList().ForEach(hp => {
            hp.HGrow = 1;
            hp.Children[0].HGrow = 2;
            hp.Children[1].HGrow = 1;
        });

        TextItem tiFocus = new TextItem(font_c1, "Focus right");
        tiFocus.HAlign = HAlignment.Center;

        StackPane spTop = new StackPane(back0, vpInfo);
        StackPane spCen = new StackPane(back1, listMenu1);
        StackPane spBot = new StackPane(back2, tiFocus);

        spTop.HGrow = spCen.HGrow = spBot.HGrow = 1;
        spTop.VGrow = spCen.VGrow = 5;
        spBot.VGrow = 1;

        StackPane spRight = new StackPane(back3, listMenu0);
        spRight.HGrow = spRight.VGrow = 5;
        spRight.HAlign = HAlignment.Center;
        spRight.VAlign = VAlignment.Center;

        VPane vpLeft = new VPane(spTop, spCen, spBot);
        VPane vpRight = new VPane(listMenuTb, spRight);
        vpLeft.HGrow = vpLeft.VGrow = 1;
        vpRight.HGrow = vpRight.VGrow = 1;

        HPane hPane = new HPane(vpLeft, vpRight);
        hPane.HGrow = hPane.VGrow = 1;
        MainContainer.Add(hPane);
    }
}