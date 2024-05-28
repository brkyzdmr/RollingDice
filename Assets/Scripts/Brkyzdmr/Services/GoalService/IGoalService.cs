using System.Collections.Generic;
using Brkyzdmr.Services.ConfigService;

namespace Brkyzdmr.Services.GoalService
{
    public interface IGoalService
    {
        Dictionary<string, Goal> goals { set; get; }
        Dictionary<string, GoalConfig> goalLookUp { set; get; }
        Dictionary<string, int> goalTracker { set; get; }
        void InitializeGoalData();
        bool IsGoalObject(string id);
        void UpdateGoal(Goal goal, bool hasAnimation);
        bool CheckGoalsComplete();
    }
}