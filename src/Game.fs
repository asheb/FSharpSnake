﻿module Game

open Engine
open Util


type Field = { width: int; height: int }
type CellXY = int * int
type Dir = Up | Down | Left | Right
type Snake = { length: int; cells: CellXY list; dir: Dir; }
type State = { field: Field; snake: Snake; apples: CellXY list }


let keyToDir = function | Key.Up -> Up | Key.Down -> Down | Key.Left -> Left | Key.Right -> Right
let move (x, y) = function | Up -> (x, y - 1) | Down -> (x, y + 1) | Left -> (x - 1, y) | Right -> (x + 1, y)


let update (state: State) input = 
    let dir = match Seq.tryLast input with | Some(key) -> keyToDir key | None -> state.snake.dir
    let snake = { state.snake with cells = List.truncate state.snake.length (move state.snake.cells[0] dir :: state.snake.cells)
                                   dir = dir }
    { state with snake = snake }


let draw cellSize state =
    let margin = cellSize * 2
    let drawCell x y color = Rect(margin + x * cellSize, margin + y * cellSize, cellSize, cellSize, color)
    seq { Rect(margin, margin, cellSize * state.field.width, cellSize * state.field.height, Yellow)
          for (x, y) in state.snake.cells do drawCell x y Green
          for (x, y) in state.apples do drawCell x y Red }


let field = { width = 60; height = 40 }

let randomApple _ = (rand field.width, rand field.height)

run { title = "Snake"
      size = (800, 600)
      init = { field = field
               snake = { length = 25; cells = [ (5, 5) ]; dir = Right }
               apples = List.init 5 randomApple }
      update = update
      draw = draw 10
      fps = 20 }

