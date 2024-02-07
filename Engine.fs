module Engine

open System.Windows.Forms
open System.Drawing


type Color = Green | Red
type Shape = Rect of x: int * y: int * w: int * h: int * color: Color
type Game<'state> = { title: string; state: 'state; update: 'state -> 'state * Shape seq }


let private dotnetColor = function
    | Green -> System.Drawing.Color.Green
    | Red   -> System.Drawing.Color.Red


type MyForm<'state>(state: 'state, update) as this =
    inherit Form()

    let mutable state = state
    let mutable shapes = Seq.empty
    let timer = new Timer(Interval = 100)
    do
        timer.Tick.Add <| fun _ ->
            let (a, b) = update state
            state <- a
            shapes <- b
            this.Refresh()
        timer.Start()

    override _.OnPaint e =
        base.OnPaint(e)
        for sh in shapes do
            match sh with
            | Rect(x, y, w, h, c) -> e.Graphics.FillRectangle(new SolidBrush(dotnetColor c), x, y, w, h)


let run<'state> game =
    Application.Run(new MyForm<'state>(game.state, game.update, Width = 800, Height = 600, Text = game.title))

