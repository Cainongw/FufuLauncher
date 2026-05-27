using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace FufuLauncher.Models;

public partial class GachaDisplayItem : ObservableObject
{
    public string Name
    {
        get; set;
    }
    public string Type
    {
        get; set;
    }
    public int Rank
    {
        get; set;
    }

    public int Count
    {
        get; set;
    }

    public string Time
    {
        get; set;
    }

    public string LastGetTime
    {
        get; set;
    }

    [ObservableProperty] private string _imageUrl;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(ElementImageSource))] private string _elementUrl;

    public ImageSource ElementImageSource
    {
        get
        {
            if (string.IsNullOrEmpty(ElementUrl)) return null;
            try
            {
                return new BitmapImage(new Uri(ElementUrl));
            }
            catch
            {
                return null;
            }
        }
    }

    public SolidColorBrush RarityBackground => Rank switch
    {
        5 => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 198, 160, 96)),
        4 => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 149, 118, 193)),
        _ => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 102, 168, 209))
    };

    public SolidColorBrush RarityColorHex => Rank switch
    {
        5 => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 198, 160, 96)),
        4 => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 149, 118, 193)),
        _ => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 102, 168, 209))
    };

    [ObservableProperty] private PityStatus _pityStatus;

    public int PityMaximum => Rank == 5 ? 90 : 10;

    public SolidColorBrush ProgressBarColor => Rank switch
    {
        5 => Count <= 30 ? new SolidColorBrush(Windows.UI.Color.FromArgb(255, 106, 168, 209)) :
             Count <= 60 ? new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 165, 0)) :
             new SolidColorBrush(Windows.UI.Color.FromArgb(255, 220, 53, 69)),
        4 => Count <= 3 ? new SolidColorBrush(Windows.UI.Color.FromArgb(255, 106, 168, 209)) :
             Count <= 6 ? new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 165, 0)) :
             new SolidColorBrush(Windows.UI.Color.FromArgb(255, 220, 53, 69)),
        _ => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 106, 168, 209))
    };

    public SolidColorBrush PityStatusBrush => PityStatus switch
    {
        PityStatus.LostPity => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 100, 100)),
        PityStatus.Guaranteed => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 180, 100)),
        PityStatus.SmallPity => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 46, 204, 64)),
        PityStatus.Up => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 100, 200, 255)),
        _ => null
    };

    public string PityStatusText => PityStatus switch
    {
        PityStatus.LostPity => "歪",
        PityStatus.Guaranteed => "大保底",
        PityStatus.SmallPity => "小保底",
        PityStatus.Up => "UP",
        _ => ""
    };
}

public class ScrapedMetadata
{
    public string Name
    {
        get; set;
    }
    public string ImgSrc
    {
        get; set;
    }
    public string ElementSrc
    {
        get; set;
    }
    public string Type
    {
        get; set;
    }
    public string Rank
    {
        get; set;
    }
    public string ItemId
    {
        get; set;
    }
}