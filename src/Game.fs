module Game

open Engine


type Field = { width: int; height: int }
type CellXY = int * int
type Dir = Up | Down | Left | Right
type Snake = { length: int; cells: CellXY list }
type State = { field: Field; snake: Snake; dir: Dir }


let keyToDir = function | Key.Up -> Up | Key.Down -> Down | Key.Left -> Left | Key.Right -> Right
let move (x, y) = function | Up -> (x, y - 1) | Down -> (x, y + 1) | Left -> (x - 1, y) | Right -> (x + 1, y)


let update state input = 
    let dir = match Seq.tryLast input with | Some(key) -> keyToDir key | None -> state.dir
    let snake = { state.snake with cells = List.truncate state.snake.length (move state.snake.cells[0] dir :: state.snake.cells) }
    { state with snake = snake; dir = dir }


let draw cellSize state =
    let margin = cellSize * 2
    seq { Rect(margin, margin, cellSize * state.field.width, cellSize * state.field.height, Yellow)
          for (x, y) in state.snake.cells do Rect(margin + x * cellSize, margin + y * cellSize, cellSize, cellSize, Green) }


run { title = "Snake"
      size = (800, 600)
      init = { field = { width = 60; height = 40 }
               snake = { length = 25; cells = [ (5, 5) ] }
               dir = Right }
      update = update
      draw = draw 10
      fps = 20 }

