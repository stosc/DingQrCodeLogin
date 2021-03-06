﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DingQrCodeLogin.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DingQrCodeLogin.Models;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using System.Data;
using Dapper;

namespace DingQrCodeLogin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string qrAppId = AppConfigurtaionHelper.Configuration["DingDing:QrAppId"];
            ViewBag.AppId = qrAppId;
            return View();
        }


        public string DingLogin(string code, string state)
        {
            //state 是前端传入的，钉钉并不会修改，比如有多种登录方式的时候，一个登录方法判断登录方式可以进行不同的处理。

            OapiSnsGetuserinfoBycodeResponse response = new OapiSnsGetuserinfoBycodeResponse();
            try
            {
                string qrAppId= AppConfigurtaionHelper.Configuration["DingDing:QrAppId"];
                string qrAppSecret = AppConfigurtaionHelper.Configuration["DingDing:QrAppSecret"];
                if (string.IsNullOrWhiteSpace(qrAppId)||string.IsNullOrWhiteSpace(qrAppSecret))
                {
                    throw new Exception("请先配置钉钉扫码登录信息！");
                }

                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/sns/getuserinfo_bycode");
                OapiSnsGetuserinfoBycodeRequest req = new OapiSnsGetuserinfoBycodeRequest();
                req.TmpAuthCode = code;
                response = client.Execute(req, qrAppId, qrAppSecret);
                string name = response.UserInfo.Nick;
                string openId = response.UserInfo.Openid;
                string unionid = response.UserInfo.Unionid;
                //获取到response后就可以进行自己的登录业务处理了

                //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                //此处省略一万行代码

                //TODO: 此处处理登录逻辑，先判断openid，和姓名在数据库中的匹配，成功后等同于用户名密码登录成功；同时要记录登录成功的ip地址
                if(saveInfo(name,openId, unionid))
                {
                    return "信息注册成功!";
                }
                else
                {
                    return "信息注册失败!";
                }

            }
            catch (Exception e)
            {
                response.Errmsg = e.Message;
            }

            return response.Body;
        }

        private Boolean saveInfo(string name,string openId,string unionId)
        {
            Boolean ret = false;
            var ns = name.Split('#');
            if(ns.Length == 2)
            {
                using (IDbConnection db = DapperContext.Connection())
                {

                    var sql = $@"select `name`,`alias` from tm_reg_employee where `alias` = '{ns[1]}' and `name` = '{ns[0]}'";
                    var count = db.Query(sql).Count();
                    if (count  == 1)
                    {
                        sql = $@"update  tm_reg_employee set `openId` = '{openId}', `unioId` = '{unionId}' where `alias` = '{ns[1]}' and `name` = '{ns[0]}'";
                        db.Execute(sql);
                        ret = true;
                    }
                }
            }

            return ret;
            
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
