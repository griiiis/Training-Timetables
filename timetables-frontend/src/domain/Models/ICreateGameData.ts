export interface ICreateGameData {
  "selectedLevelIds": [[string]],
  "selectedCourtIds": [[string]],
  "selectedTrainersIds": [[string]]
  "selectedTimesIds": [{
    "date": string,
    "selectedTimesList": [string]
  }],
  "peoplePerCourtInputs": [number]
}