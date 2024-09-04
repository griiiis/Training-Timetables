namespace App.DTO.v1_0;

public class CreateGamesData
{
    public List<List<Guid>>? SelectedLevelIds { get; set; }
    public List<List<Guid>>? SelectedCourtIds { get; set; }
    public List<TimeOfDaysViewModel>? SelectedTimesIds { get; set; }
    public List<List<Guid>>? SelectedTrainersIds { get; set; }
    public List<int>? PeoplePerCourtInputs { get; set; }
}

public class TimeOfDaysViewModel
{
    public DateOnly? Date { get; set; }
    public List<Guid>? SelectedTimesList { get; set; }
}