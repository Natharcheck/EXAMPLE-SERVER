namespace Server.Notifications.Interface;

public interface INotice
{
    public ENoticeFrom NoticeFrom { get; set; }
    public ushort CountNotice { get; set; }
    
}