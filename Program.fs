open System.Windows.Forms
open System.Drawing


let mutable x = 0


type MyForm =
    inherit Form

    new() as this =
        let timer = new Timer(Interval = 100)
        timer.Tick.Add <| fun _ ->
            x <- x + 10
            this.Refresh()
        timer.Start()
        {}

    override _.OnPaint e =
        base.OnPaint(e)
        e.Graphics.FillRectangle(new SolidBrush(Color.Green), x, 100, 200, 150)


let f = new MyForm(Width = 800, Height = 600, Text = "Hello")
//f.Controls.Add(new Label(Text = "Hello World!", Left = 100))
Application.Run(f)

