using CommunityToolkit.Mvvm.Messaging.Messages;

namespace FufuLauncher.Messages;

public class BackgroundDownloadStateMessage : ValueChangedMessage<bool>
{
    public BackgroundDownloadStateMessage(bool isDownloading) : base(isDownloading)
    {
    }
}