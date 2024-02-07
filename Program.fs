module Program

open Engine


type State = { x: int; y: int }


let initialState = { x = 10; y = 10 }


let update { x = x; y = y } input = 
    let newState = match Seq.tryLast input with
                   | Some(Up)    -> { x = x; y = y - 10 }
                   | Some(Down)  -> { x = x; y = y + 10 }
                   | Some(Left)  -> { x = x - 10; y = y }
                   | Some(Right) -> { x = x + 10; y = y }
                   | None        -> { x = x; y = y }

    let shapes = seq { Rect(x, y, 10, 10, Green) }
    (newState, shapes)


run { title = "Snake"; state = initialState; update = update }


