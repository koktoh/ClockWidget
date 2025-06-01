using Prism.Events;

namespace ClockWidget.Events
{
    internal class SessionActiveEvent : PubSubEvent { }
    internal class SessionLogonEvent : PubSubEvent { }
    internal class SessionLogoffEvent : PubSubEvent { }
    internal class SessionLockEvent : PubSubEvent { }
    internal class SessionUnlockEvent : PubSubEvent { }
}
