using System;
using System.Reflection;

namespace HrtzSysInfo.Extensions
{
    public static class EventExtensions
    {
        public static void FireEvent(object onMe, string invokeMe, params object[] eventParams)
        {
            var fieldInfo = onMe.GetType().GetField(invokeMe, BindingFlags.Instance | BindingFlags.NonPublic);

            if (fieldInfo == null) return;

            var eventDelagate = (MulticastDelegate)fieldInfo.GetValue(onMe);

            var delegates = eventDelagate.GetInvocationList();

            foreach (var dlg in delegates)
                dlg.Method.Invoke(dlg.Target, eventParams);
        }

        public static void FirePublicEvent(object onMe, string invokeMe, params object[] eventParams)
        {
            var fieldInfo = onMe.GetType().GetField(invokeMe, BindingFlags.Instance | BindingFlags.Public);

            if (fieldInfo == null) return;

            var eventDelagate = (MulticastDelegate)fieldInfo.GetValue(onMe);

            var delegates = eventDelagate.GetInvocationList();

            foreach (var dlg in delegates)
                dlg.Method.Invoke(dlg.Target, eventParams);
        }
    }
}
