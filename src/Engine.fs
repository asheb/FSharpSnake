﻿module Engine

open System.Windows.Forms
open System.Drawing


type Key = Up | Down | Left | Right
type Color = Yellow | Green | Red
type Shape = Rect of x: int * y: int * w: int * h: int * color: Color
type Game<'state> = { title: string
                      size: int * int
                      init: 'state
                      update: 'state -> Key seq -> 'state
                      draw: 'state -> Shape seq
                      fps: int }


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


type MyForm<'state>(game: Game<'state>) as this =
    inherit Form()

    let mutable state = game.init
    let mutable input = Seq.empty

    let timer = new Timer(Interval = 1000 / game.fps)
    do
        let (w, h) = game.size
        this.Width <- w
        this.Height <- h
        this.Text <- game.title

        timer.Tick.Add <| fun _ ->
            state <- game.update state input
            input <- Seq.empty
            this.Refresh()
        timer.Start()

        this.KeyDown.Add <| fun e ->
            input <- Seq.append input (convertInput e.KeyCode)

    override _.OnPaint e =
        base.OnPaint(e)
        for shape in game.draw state do
            match shape with
            | Rect(x, y, w, h, c) -> e.Graphics.FillRectangle(new SolidBrush(dotnetColor c), x, y, w, h)


let run<'state> game = Application.Run(new MyForm<'state>(game))

