using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MopsterTeams.Enums;
using MopsterTeams.Models;
using MopsterTeams.Services;
using Newtonsoft.Json;

namespace MopsterTeams.Services
{
    public class PushNotificationActionService : IPushNotificationActionService
    {
        private readonly Dictionary<string, ENotificationAction> _actionMappings = new Dictionary<string, ENotificationAction>
        {
            { "none", ENotificationAction.None },
            { "openfacilityaddress", ENotificationAction.OpenFacilityAddress },
            { "checkin", ENotificationAction.CheckIn },
            { "checkout", ENotificationAction.CheckOut },
            { "appointment", ENotificationAction.Appointment }
        };

        public event EventHandler<PushNotificationModel> ActionTriggered = delegate { };
        public event Action RegistrationRefreshTriggered = delegate { };

        public int? GetInvocationList()
        {
            return ActionTriggered?.GetInvocationList().Count();
        }

        public void TriggerAction(string action, string id, string itemType)
        {
            //App.Resolve<IDialogService>().ShowAlertAsync(action, id, itemType);
            if (!_actionMappings.TryGetValue(action.ToLower(), out var pushAction))
                return;

            List<Exception> exceptions = new List<Exception>();
            //App.Resolve<IDialogService>().ShowAlertAsync(action, ActionTriggered?.GetInvocationList().Count().ToString(), itemType);
            foreach (var handler in ActionTriggered?.GetInvocationList())
            {
                try
                {
                    handler.DynamicInvoke(this, new PushNotificationModel() { ActionType = pushAction, ActionId = id, Parameter = itemType });
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
                throw new AggregateException(exceptions);
        }

        public void TriggerRegistrationRefresh()
        {
            RegistrationRefreshTriggered.Invoke();
        }
    }
}
