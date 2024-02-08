module Program

open Engine


type CellXY = int * int
type Field = { width: int; height: int }
type State = { field: Field; snake: CellXY list }


let dirVec = function
    | Some(Up)    -> ( 0, -1)
    | Some(Down)  -> ( 0, +1)
    | Some(Left)  -> (-1,  0)
    | Some(Right) -> (+1,  0)
    | None        -> ( 0,  0)

let move (x, y) (dx, dy) = (x + dx, y + dy)


let draw cellSize state =
    let margin = cellSize * 2
    seq { Rect(margin, margin, cellSize * state.field.width, cellSize * state.field.height, Yellow)
          for (x, y) in state.snake do Rect(margin + x * cellSize, margin + y * cellSize, cellSize, cellSize, Green) }


let update state input = 
    let dir = dirVec (Seq.tryLast input)
    ({ state with snake = [ move state.snake[0] dir ] }, draw 10 state)


run { title = "Snake"
      state = { field = { width = 60; height = 40 }
                snake = [ (5, 5) ] }
      update = update }


