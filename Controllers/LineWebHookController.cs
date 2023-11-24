using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using isRock.LineBot;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace isRock.Template
{
    public class LineWebHookController : isRock.LineBot.LineWebHookControllerBase
    {
        [Route("api/LineBotWebHook")]
        [HttpPost]
        public IActionResult POST()
        {
            // TODO: 請更換成你自己的 AdminUserId
            var AdminUserId = "要通知的管理者 ID，通常是你自己的 User ID";

            try
            {
                // TODO: 請更換成你自己的 ChannelAccessToken
                this.ChannelAccessToken = "APP 的 Channel secret";

                var events = ReceivedMessage.events;

                // 配合Line Verify
                if (events == null || events.Count() <= 0 || events.FirstOrDefault()?.replyToken == "00000000000000000000000000000000")
                    return Ok();

                foreach (var lineEvent in events)
                {
                    var userName = this.GetUserInfo(lineEvent.source.userId).displayName;
                    var responseMsg = "";

                    if (lineEvent.type.ToLower() == "message" && lineEvent.message.type == "text")
                    {
                        var type = lineEvent.message.text;
                        if (type == "報修")
                            responseMsg = $"親愛的 {userName} 您好，請提供要報修的房號，我們將盡速為您處理 🙏 。";
                    }

                    if (!string.IsNullOrEmpty(responseMsg))
                        this.ReplyMessage(lineEvent.replyToken, responseMsg);

                    //var types = new List<string>
                    //{
                    //    "餐費",
                    //    "交通費",
                    //    "娛樂費",
                    //    "服裝費"
                    //};
                    //isRock.LineBot.TextMessage TextMessage = new isRock.LineBot.TextMessage($"請選擇或直接輸入這筆金額QQ的記帳類別"); 
                    //foreach (var type in types)
                    //{
                    //    TextMessage.quickReply.items.Add(new isRock.LineBot.QuickReplyMessageAction(type, type));
                    //}
                    //this.ReplyMessage(lineEvent.replyToken, TextMessage);


                    //var confrim = new ConfirmTemplate();
                    //confrim.text = "請問您是否要報修？";
                    //confrim.actions.Add(new isRock.LineBot.MessageAction() { label = "是", text = "報修" });
                    //confrim.actions.Add(new isRock.LineBot.MessageAction() { label = "否", text = "否" });
                    //this.ReplyMessage(lineEvent.replyToken, confrim);
                }

                //// 取得Line Event
                //var firstEvent = this.ReceivedMessage.events.FirstOrDefault();
                //if (firstEvent == null) return Ok();

                //var userName = this.GetUserInfo(firstEvent.source.userId).displayName;

                //var responseMsg = "";

                //// 準備回覆訊息
                //if (firstEvent.type.ToLower() == "message" && firstEvent.message.type == "text") { }
                //    responseMsg = $"收到 event : {firstEvent.type} type: {firstEvent.message.type} ，{userName} 說了: {firstEvent.message.text}";
                //else if (firstEvent.type.ToLower() == "message")
                //    responseMsg = $"收到 event : {firstEvent.type} type: {firstEvent.message.type} ";
                //else
                //    responseMsg = $"收到 event : {firstEvent.type} ";

                //// 回覆訊息
                //this.ReplyMessage(firstEvent.replyToken, responseMsg);

                return Ok();
            }
            catch (Exception ex)
            {
                this.PushMessage(AdminUserId, "發生錯誤:\n" + ex.Message);
                return Ok();
            }
        }
    }
}