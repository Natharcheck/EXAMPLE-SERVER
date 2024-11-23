using Server.Notifications.Interface;

namespace Server.Notifications.Example;

public class Notice(ENoticeFrom from, ushort count) : INotice
{
    public ENoticeFrom NoticeFrom { get; set; } = from;
    public ushort CountNotice { get; set; } = count;
}