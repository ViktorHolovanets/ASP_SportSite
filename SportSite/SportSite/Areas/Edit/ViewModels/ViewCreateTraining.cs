using SportSite.Models.Db;

namespace SportSite.Areas.Edit.ViewModels
{
    public class ViewCreateTraining
    {
        public TypeTraining typeTraining { get; set; }
        public List<DayofWeek> dayofWeeks { get; set; } = new();
        public Guid IdCoach { get; set; }
        public Guid? IdClient { get; set; }
        public string? Time { get; set; }
    }
}
