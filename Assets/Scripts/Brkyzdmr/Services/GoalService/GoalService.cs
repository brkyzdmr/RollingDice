using System.Collections.Generic;
using Brkyzdmr.Services.ConfigService;
using Brkyzdmr.Services.EventService;
using Brkyzdmr.Tools.EzTween;
using UnityEngine;

namespace Brkyzdmr.Services.GoalService
{
    public class GoalService : Service, IGoalService
    {
        public Dictionary<string, Goal> goals { set; get; }
        public Dictionary<string, GoalConfig> goalLookUp { set; get; }
        public Dictionary<string, int> goalTracker { set; get; }
        private readonly IConfigService _configService;
        private readonly IEventService _eventService;
        private readonly Camera _mainCamera;

        public GoalService()
        {
            _configService = Services.GetService<IConfigService>();
            _eventService = Services.GetService<IEventService>();
            goals = new Dictionary<string, Goal>();
            goalLookUp = new Dictionary<string, GoalConfig>();
            goalTracker = new Dictionary<string, int>();
            _mainCamera = Camera.main;
        }

        public void InitializeGoalData()
        {
            var goalConfigs = _configService.currentLevelConfig.goals;
            foreach (var goalConfig in goalConfigs)
            {
                goalLookUp[goalConfig.id] = goalConfig;
                goalTracker[goalConfig.id] = goalConfig.count;
            }
        }

        public bool IsGoalObject(string id)
        {
            return goalLookUp.ContainsKey(id);
        }

        public void UpdateGoal(Goal goal, bool hasAnimation)
        {
            if (goalTracker.ContainsKey(goal.id))
            {
                var goalTargetPos = goals[goal.id].transform.position;

                if (hasAnimation)
                {
                    goals[goal.id].transform.DoMove(goalTargetPos, 0.5f)
                        .SetRelative(true)
                        .OnComplete(() =>
                        {
                            goalTracker[goal.id] += 1;
                            UpdateGoalText(goal.id);
                        });
                }
            }
        }

        public bool CheckGoalsComplete()
        {
            foreach (var goalCount in goalTracker.Values)
            {
                if (goalCount > 0) return false; // Still goals remaining
            }

            return true; // All goals met!
        }

        private void UpdateGoalText(string id)
        {
            if (goals[id] != null)
            {
                goals[id].goalCountText.text = Mathf.Max(goalTracker[id], 0).ToString();
            }
        }
    }
}