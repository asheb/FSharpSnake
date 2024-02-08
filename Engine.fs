module Engine

open System.Windows.Forms
open System.Drawing


type Key = Up | Down | Left | Right
type Color = Yellow | Green | Red
type Shape = Rect of x: int * y: int * w: int * h: int * color: Color
type Game<'state> = { title: string; state: 'state; update: 'state -> Key seq -> 'state * Shape seq }


let private dotnetColor = function
    | Yellow -> System.Drawing.Color.Yellow
    | Green  -> System.Drawing.Color.Green
    | Red    -> System.Drawing.Color.Red

let private convertInput = function
    | Keys.Up    -> seq { Up }
    | Keys.Down  -> seq { Down }
    | Keys.Left  -> seq { Left }
    | Keys.Right -> seq { Right }
    | _          -> Seq.empty


type MyForm<'state>(state: 'state, update) as this =
    inherit Form()

    let mutable state = state
    let mutable shapes = Seq.empty
    let mutable input = Seq.empty

    let timer = new Timer(Interval = 100)
    do
        timer.Tick.Add <| fun _ ->
            let (newState, newShapes) = update state input
            state <- newState
            shapes <- newShapes
            input <- Seq.empty
            this.Refresh()
        timer.Start()

        this.KeyDown.Add <| fun e ->
            input <- Seq.append input (convertInput e.KeyCode)

    override _.OnPaint e =
        base.OnPaint(e)
        for sh in shapes do
            match sh with
            | Rect(x, y, w, h, c) -> e.Graphics.FillRectangle(new SolidBrush(dotnetColor c), x, y, w, h)


let run<'state> game =
    Application.Run(new MyForm<'state>(game.state, game.update, Width = 800, Height = 600, Text = game.title))

