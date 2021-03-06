﻿using DAL;
using DAL.Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;

namespace BLL
{
    public class SyscfgBLL
    {
        /// <summary>
        /// 读取参数
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Syscfg LoadCfg(out string msg)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    dal.OpenReader("Select Section,OptionName,OptionValue From tSyscfg");
                    Syscfg cfg=new Syscfg();
                    while (dal.DataReader.Read())
                    {
                        switch (Convert.ToString(dal.DataReader["OptionName"]).TrimEnd())
                        {
                            case "Company":
                                cfg.Company = Convert.ToString(dal.DataReader["OptionValue"]).TrimEnd();
                                break;
                            case "License":
                                cfg.License = Convert.ToString(dal.DataReader["OptionValue"]).TrimEnd();
                                break;
                        }
                    }
                    cfg.SerialNo = LicenseClass.GetMacByNetworkInterface();
                    if (string.IsNullOrEmpty(cfg.SerialNo))
                    {
                        msg = "获取序列号失败";
                        return null;
                    }
                    if (cfg.License!=null && cfg.License.Length==48)
                    {
                        LicenseClass.AnalyzeLisense(cfg.License, cfg.SerialNo, out cfg.ExpDate, out cfg.ReportNumber);
                        if (cfg.ExpDate == "99999999")
                        {
                            cfg.ExpDate = "永久";
                        }
                        else
                        {
                            StringBuilder temp=new StringBuilder(32);
                            temp.AppendFormat("{0}-{1}-{2}",cfg.ExpDate.Substring(0,4),cfg.ExpDate.Substring(4,2),cfg.ExpDate.Substring(6,2));
                            cfg.ExpDate = temp.ToString();
                        }
                        if (cfg.ReportNumber == "999")
                        {
                            cfg.ReportNumber = "无限";
                        }
                    }
                    else
                    {
                        cfg.ExpDate = "无效";
                        cfg.ReportNumber = "无效";
                    }
                    msg = "success";
                    return cfg;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 保存参数
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int SaveCfg(Syscfg cfg, out string msg)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    int i;
                    dal.BeginTran();
                    dal.Execute("UPDATE dbo.tSysCfg SET OptionValue=@OptionValue WHERE OptionName=@OptionName", out i,
                        dal.CreateParameter("@OptionValue", cfg.Company),
                        dal.CreateParameter("@OptionName", "Company"));
                    dal.Execute("UPDATE dbo.tSysCfg SET OptionValue=@OptionValue WHERE OptionName=@OptionName", out i,
                        dal.CreateParameter("@OptionValue", cfg.License),
                        dal.CreateParameter("@OptionName", "License"));
                    dal.CommitTran();
                    msg = "success";
                    return 1;
                }
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }
    }
}
