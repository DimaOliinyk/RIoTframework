using CourseWorkUI.Controller;
using Plugin.LocalNotification;

namespace CourseWorkUI.UI.Menues;

public static class NotificationSender
{    
    public static async Task Notify(string data) 
    {
        var request = new NotificationRequest
        {
             NotificationId = 1337, 
             Title = $"Specified data from project \"{FileController.GetProjectName()}\" received",
             Description = data,
             BadgeNumber = 42,
             CategoryType = NotificationCategoryType.Event,
        };

        await LocalNotificationCenter.Current.Show(request);
    }
}