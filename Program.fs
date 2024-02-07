module Program

open Engine


type State = { x: int }

let update { x = x } = 
    let shapes = seq { Rect(x, 10, 10, 10, Red) 
                       Rect(x, 20, 10, 10, Green) }
    ({ x = x + 10 }, shapes)

run { title = "Snake"; state = { x = 0 }; update = update }


