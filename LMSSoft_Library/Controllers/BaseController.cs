using LMSSoft_Library.App_Start;
using System;
using System.Web.Mvc;

namespace LMSSoft_Library.Controllers
{
    public class BaseController : Controller
    {
        public DataBaseContext db = new DataBaseContext();


        public void ShowMessage(MessageType type, string messageText)
        {
            TempData["UserMessage"] = new Message() { CssClass = Message.GetCssClass(type), Title = Message.GetMessageTitle(type), MessageText = messageText };
        }
        public void ShowMessage(string messageText, MessageType type = MessageType.error)
        {
            TempData["UserMessage"] = new Message() { CssClass = Message.GetCssClass(type), Title = Message.GetMessageTitle(type), MessageText = messageText };
        }
    }



    [Serializable()]
    public class Message
    {
        public string CssClass { get; internal set; }
        public string Title { get; internal set; }
        public string MessageText { get; internal set; }

        public static string GetCssClass(MessageType type)
        {
            string cssClass = "";
            switch (type)
            {
                case MessageType.info:
                    cssClass = "alert-info";
                    break;
                case MessageType.success:
                    cssClass = "alert-success";
                    break;
                case MessageType.warning:
                    cssClass = "alert-warning";
                    break;
                case MessageType.error:
                    cssClass = "alert-danger";
                    break;
            };
            return cssClass;
        }

        public static string GetMessageTitle(MessageType type)
        {
            string titleText = "";
            switch (type)
            {
                case MessageType.info:
                    titleText = MessageTitle.Information.ToString();
                    break;
                case MessageType.success:
                    titleText = MessageTitle.Success.ToString();
                    break;
                case MessageType.warning:
                    titleText = MessageTitle.Warning.ToString();
                    break;
                case MessageType.error:
                    titleText = MessageTitle.Error.ToString();
                    break;
            };
            return titleText;
        }
    }
    public enum MessageType
    {
        info,
        success,
        warning,
        error
    }
    public enum MessageTitle
    {
        Information,
        Success,
        Warning,
        Error
    }
}