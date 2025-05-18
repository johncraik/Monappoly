using MonappolyLibrary.GameModels.Boards.Spaces;

namespace MonappolyLibrary.GameModels.Boards.ViewModels;

public class BoardViewModel
{
    public Board Board { get; set; }
    public List<IBoardSpace> Spaces { get; set; } = [];
}