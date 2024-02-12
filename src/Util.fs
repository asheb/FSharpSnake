module Util


let rand =
    let randomizer = new System.Random()
    fun max -> randomizer.Next(max)

