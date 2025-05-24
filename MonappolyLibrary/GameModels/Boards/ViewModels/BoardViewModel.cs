using MonappolyLibrary.GameModels.Boards.Spaces;
using MonappolyLibrary.GameModels.MiscGameObjs;
using MonappolyLibrary.GameModels.MiscGameObjs.ViewModels;

namespace MonappolyLibrary.GameModels.Boards.ViewModels;

public class BoardViewModel
{
    public Board Board { get; set; }
    public List<BuildingPoolViewModel> Buildings { get; set; }
    public List<IBoardSpace> Spaces { get; set; } = [];
}