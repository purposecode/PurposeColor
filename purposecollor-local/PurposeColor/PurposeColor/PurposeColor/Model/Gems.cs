using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.Model
{
	public class EventTitle
	{
		public string event_id { get; set; }
		public string event_title { get; set; }
		public string emotion_id { get; set; }
	}

	public class EventDatetime
	{
		public string event_id { get; set; }
		public string event_datetime { get; set; }
		public string emotion_id { get; set; }
	}

	public class EventDetail
	{
		public string event_id { get; set; }
		public string event_details { get; set; }
		public string emotion_id { get; set; }
	}

	public class EventMedia
	{
		public string event_id { get; set; }
		public string media_type { get; set; }
		public string event_media { get; set; }
		public string emotion_id { get; set; }
	}

	public class GemsEmotionsDetails
	{
		public string user_id { get; set; }
		public string emotion_id { get; set; }
		public string emotion_title { get; set; }
		public List<EventTitle> event_title { get; set; }
		public List<EventDatetime> event_datetime { get; set; }
		public List<EventDetail> event_details { get; set; }
		public List<EventMedia> event_media { get; set; }
	}

	public class GemsEmotionsObject
	{
		public string code { get; set; }
		public string noimageurl { get; set; }
		public string mediapath { get; set; }
		public string mediathumbpath { get; set; }
		public List<GemsEmotionsDetails> resultarray { get; set; }
	}

	public class GemsEmotionsDetailsDB
	{
		public string code { get; set; }
		public string noimageurl { get; set; }
		public string mediapath { get; set; }
		public string mediathumbpath { get; set; }
		public string user_id { get; set; }
		public string emotion_id { get; set; }
		public string emotion_title { get; set; }
	}

	public class ActionTitle
	{
		public string goalaction_id { get; set; }
		public string action_title { get; set; }
		public string goal_id { get; set; }
	}

	public class ActionDetail
	{
		public string goalaction_id { get; set; }
		public string action_details { get; set; }
		public string goal_id { get; set; }
	}

	public class ActionDatetime
	{
		public string goalaction_id { get; set; }
		public string action_datetime { get; set; }
		public string goal_id { get; set; }
	}

	public class ActionMedia
	{
		public string goalaction_id { get; set; }
		public string media_type { get; set; }
		public string action_media { get; set; }
		public string goal_id { get; set; }
	}

	public class GemsGoalsDetails
	{
		public string user_id { get; set; }
		public string goal_id { get; set; }
		public string goal_title { get; set; }
        public string goal_media { get; set; }
        public string goal_details { get; set; }
		public List<ActionTitle> action_title { get; set; }
		public List<ActionDetail> action_details { get; set; }
		public List<ActionDatetime> action_datetime { get; set; }
		public List<ActionMedia> action_media { get; set; }
	}

	public class GemsGoalsObject
	{
		public string code { get; set; }
		public string noimageurl { get; set; }
		public string mediapath { get; set; }
		public string mediathumbpath { get; set; }
		public List<GemsGoalsDetails> resultarray { get; set; }
	}

	public class GemsGoalsDetailsDB
	{
		public string code { get; set; }
		public string noimageurl { get; set; }
		public string mediapath { get; set; }
		public string mediathumbpath { get; set; }
		public string user_id { get; set; }
		public string goal_id { get; set; }
		public string goal_title { get; set; }
	}


	public enum GemType
	{
		Goal = 1,
		Event = 2,
		Action = 3,
		Emotion = 4
	}


	public class PendingActionTitle
	{
		public string goal_id { get; set; }
		public string savedgoal_id { get; set; }
		public string actionstatus_id { get; set; }
		public string goalaction_id { get; set; }
		public string action_title { get; set; }
	}

	public class PendingActionDetail
	{
		public string goal_id { get; set; }
		public string savedgoal_id { get; set; }
		public string actionstatus_id { get; set; }
		public string goalaction_id { get; set; }
		public string action_details { get; set; }
	}

	public class PendingActionDatetime
	{
		public string goal_id { get; set; }
		public string savedgoal_id { get; set; }
		public string actionstatus_id { get; set; }
		public string goalaction_id { get; set; }
		public string action_datetime { get; set; }
	}

	public class PendingShareStatu
	{
		public string goal_id { get; set; }
		public string savedgoal_id { get; set; }
		public string actionstatus_id { get; set; }
		public string goalaction_id { get; set; }
		public string share_status { get; set; }
	}

	public class PendingActionMedia
	{
		public string goal_id { get; set; }
		public string goalaction_id { get; set; }
		public string media_type { get; set; }
		public string action_media { get; set; }
	}

	public class PendingGoalsDetails
	{
		public string user_id { get; set; }
		public string goal_id { get; set; }
		public string goal_title { get; set; }
		public string goal_details { get; set; }
        public string goal_media { get; set; }
		public List<PendingActionTitle> pending_action_title { get; set; }
		public List<PendingActionDetail> pending_action_details { get; set; }
		public List<PendingActionDatetime> pending_action_datetime { get; set; }
		public List<PendingShareStatu> pending_share_status { get; set; }
		public List<PendingActionMedia> pending_action_media { get; set; }
	}

	public class PendingGoalsObject
	{
		public string code { get; set; }
		public string noimageurl { get; set; }
		public string mediapath { get; set; }
		public string mediathumbpath { get; set; }
		public List<PendingGoalsDetails> resultarray { get; set; }
	}

    public class ChangePendingGoalReturn
    {
        public string code { get; set; }
        public string text { get; set; }
    }


    public class SelectedGoalMedia
    {
        public string goal_id { get; set; }
        public string media_type { get; set; }
        public string goal_media { get; set; }
    }

    public class SelectedGoalDetails
    {
        public string goal_title { get; set; }
        public string goal_details { get; set; }
        public string goal_datetime { get; set; }
        public string user_id { get; set; }
        public string share_status { get; set; }
        public List<SelectedGoalMedia> goal_media { get; set; }
    }

    public class SelectedGoal
    {
        public string code { get; set; }
        public string text { get; set; }
        public SelectedGoalDetails resultarray { get; set; }
    }

    public class DetailsPageModel
    {
        public DetailsPageModel()
        {

        }
        public List<EventMedia> mediaArray { get; set; }
        public List<ActionMedia> actionMediaArray { get; set; }
        public string pageTitleVal { get; set; }
        public string titleVal { get; set; }
        public string desc { get; set; }
        public string Media { get; set; }
        public string NoMedia { get; set; }
        public string gemId { get; set; }
        public GemType gemType { get; set; }
		public bool fromGEMSPage {get; set; }
		public List<SelectedGoalMedia> goal_media { get; set; }
    }
}
