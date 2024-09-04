import { IGame } from "./IGame"
import { ITeam } from "./ITeam"

export interface ITeamGame {
  "gameId" : string,
  "id": string,
  "team" : ITeam
  "game" : IGame
}